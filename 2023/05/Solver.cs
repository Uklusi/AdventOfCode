using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static System.Math;

using AoCUtils;
using AoCUtils.GridUtils;
using AoCUtils.MultidimUtils;
using static AoCUtils.Constants;
using static AoCUtils.Functions;

namespace AoC {
    using static Common;

    public static class Common {
        static Logger logger = new Logger(writeToFile: false);
        public static void Log(params string[] args) => logger.Log(args);
        public static void Log(params object[] args) => logger.Log(args);
        public static void LogEnum<T>(IEnumerable<T> en) => logger.LogEnum(en);
        
        

    }

    public static class Part1 {
        public static string solve(bool useExample) {
            
            Input reader = new Input(useExample: useExample);

            string result = "0";
            long resultInt = 999999999999999999;

            var data = reader.ReadParagraphs();
            var sections = data
                .Skip(1)
                .Select(
                    par => par
                        .Select(l => l.Split(" "))
                        .Skip(1)
                        .Select(l => l.Select(long.Parse).ToList())
                        .ToList()
                )
                .ToList();

            // foreach (var par in sections) {
            //     par.Sort(
            //         (l1, l2) => l1[1].CompareTo(l2[1])
            //     );
            // }
            List<long> seeds = data
                .First()
                .First()
                .Split(" ")
                .Skip(1)
                .Select(long.Parse)
                .ToList();

            // Common.Log(seeds.ToStringArray());

            foreach (var seed in seeds) {
                long transform = seed;
                foreach (var map in sections) {
                    foreach (var line in map) {
                        long start = line[1];
                        long range = line[2];
                        long dest = line[0];
                        if (start <= transform && transform < start + range) {
                            transform = dest + (transform - start);
                            break;
                        }
                    }
                }
                resultInt = Min(resultInt, transform);
            }

            

            


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }



    public static class Part2 {
        public static string solve(bool useExample) {

            Input reader = new Input(useExample: useExample);
            
            string result = "0";
            long resultInt = 999999999999999999;

            var data = reader.ReadParagraphs();
            var sections = data
                .Skip(1)
                .Select(
                    par => par
                        .Select(l => l.Split(" "))
                        .Skip(1)
                        .Select(l => l.Select(long.Parse).ToList())
                        .ToList()
                )
                .ToList();

            // foreach (var par in sections) {
            //     par.Sort(
            //         (l1, l2) => l1[1].CompareTo(l2[1])
            //     );
            // }
            List<long> seeds = data
                .First()
                .First()
                .Split(" ")
                .Skip(1)
                .Select(long.Parse)
                .ToList();

            // Common.Log(seeds.ToStringArray());

            List<(long, long)> seedRanges = new();

            for (var i = 0; i < seeds.Count / 2; i++) {
                var seed = seeds[2*i];
                seedRanges.Add((seed, seed + seeds[2*i + 1]));
            }
            // Common.Log(seedRanges.ToStringArray());
            foreach (var map in sections) {
                List<(long, long)> newSeedRanges = new();
                for (int i = 0; i < seedRanges.Count; i++) {
                    bool lineFound = false;
                    var (startSeed, endSeed) = seedRanges[i];
                    foreach (var line in map) {
                        long start = line[1];
                        long range = line[2];
                        long end = start + range;
                        long dest = line[0];
                        if (start <= startSeed && startSeed < end) {
                            long offset = dest - start;
                            long newDest = startSeed + offset;
                            // Common.Log("Found", startSeed, endSeed, "-", start, end, "- becomes", newDest, endSeed + offset);
                            if (endSeed <= end) {
                                newSeedRanges.Add((newDest, endSeed + offset));
                            }
                            else {
                                newSeedRanges.Add((newDest, dest + range));
                                seedRanges.Add((end, endSeed));
                            }
                            lineFound = true;
                            break;
                        }
                        else if (start < endSeed && endSeed <= end) {
                            long offset = dest - start;
                            // Common.Log("Found", startSeed, endSeed, "-", start, end, "- becomes", newDest, endSeed + offset);
                            {
                                newSeedRanges.Add((dest, endSeed + offset));
                                seedRanges.Add((startSeed, start));
                            }
                            lineFound = true;
                            break;
                        }
                    }
                    if (!lineFound) {
                        newSeedRanges.Add(seedRanges[i]);
                        // Common.Log("Not found:", startSeed, endSeed);
                    }
                }
                seedRanges = newSeedRanges.ToList();
                // Common.Log("");
            }
            resultInt = seedRanges
                .Select(t => t.Item1)
                .Min();


            


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }

}