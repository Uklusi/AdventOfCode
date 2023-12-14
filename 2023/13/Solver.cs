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
        
        public static bool EnumEqual<T>(IEnumerable<T> l1, IEnumerable<T> l2) where T : IEquatable<T>{
            if (l1.Count() != l2.Count()) {
                return false;
            }
            return l1.Zip(l2).All(t => t.First.Equals(t.Second));
        }

        

    }

    public static class Part1 {
        public static string solve(bool useExample) {
            
            Input reader = new Input(useExample: useExample);

            string result = "0";
            long resultInt = 0;

            List<Grid2D<char>> data = new();

            foreach (var p in reader.ReadParagraphs()) {
                data.Add(new Grid2D<char>(p));
            }

            foreach (var map in data) {
                bool found = false;
                var rowsList = map.GetRows().ToJaggedArray();
                var colsList = map.GetCols().ToJaggedArray();
                foreach (int x in IntRange(1, map.Shape.cols)) {
                    if (
                        colsList[..x]
                        .Reverse()
                        .Zip(colsList[x..])
                        .All(t => EnumEqual(t.First, t.Second))
                    ) {
                        found = true;
                        resultInt += x;
                        break;
                    }
                }
                if (!found) {
                    foreach (int y in IntRange(1, map.Shape.rows)) {
                        if (
                            rowsList[..y]
                            .Reverse()
                            .Zip(rowsList[y..])
                            .All(t => EnumEqual(t.First, t.Second))
                        ) {
                            resultInt += 100 * y;
                        }
                    }
                }
            }


            


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }



    public static class Part2 {

        public static int NumDifferences<T>(IEnumerable<T> l1, IEnumerable<T> l2) where T : IEquatable<T>{
            return l1.Zip(l2).Where(t => !t.First.Equals(t.Second)).Count();
        }

        public static string solve(bool useExample) {

            Input reader = new Input(useExample: useExample);
            
            string result = "0";
            long resultInt = 0;

            
            List<Grid2D<char>> data = new();

            foreach (var p in reader.ReadParagraphs()) {
                data.Add(new Grid2D<char>(p));
            }

            foreach (var map in data) {
                bool found = false;
                var rowsList = map.GetRows().ToJaggedArray();
                var colsList = map.GetCols().ToJaggedArray();
                foreach (int x in IntRange(1, map.Shape.cols)) {
                    if (
                        colsList[..x]
                        .Reverse()
                        .Zip(colsList[x..])
                        .Select(t => NumDifferences(t.First, t.Second))
                        .Sum() == 1
                    ) {
                        found = true;
                        resultInt += x;
                        break;
                    }
                }
                if (!found) {
                    foreach (int y in IntRange(1, map.Shape.rows)) {
                        if (
                            rowsList[..y]
                            .Reverse()
                            .Zip(rowsList[y..])
                            .Select(t => NumDifferences(t.First, t.Second))
                            .Sum() == 1
                        ) {
                            resultInt += 100 * y;
                        }
                    }
                }
            }


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }

}