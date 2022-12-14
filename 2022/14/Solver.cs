using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static System.Math;

using AoCUtils;
using AoCUtils.GridUtils;
using static AoCUtils.Constants;
using static AoCUtils.Functions;

namespace AoC {
    using static Common;

    public static class Common {
        static Logger logger = new Logger(writeToFile: false);
        public static void Log(params string[] args) => logger.Log(args);
        public static void Log(params object[] args) => logger.Log(args);
        public static void LogEnum<T>(IEnumerable<T> en) => logger.LogEnum(en);


        public static HashSet<Point> CreatePaths(List<string> input) {
            HashSet<Point> paths = new();

            foreach (string line in input) {
                IEnumerable<string> extremes = line.Split(" -> ");
                foreach ((string sStart, string sEnd) in extremes.Zip(extremes.Skip(1))) {
                    Point start = new Point(
                        sStart.Split(",").First().ToInt(),
                        sStart.Split(",").Last().ToInt()
                    );
                    Point end = new Point(
                        sEnd.Split(",").First().ToInt(),
                        sEnd.Split(",").Last().ToInt()
                    );
                    paths.Add(start);
                    Point current = start;
                    Vector dir0 = (end - start);
                    Vector dir = dir0.Normalized();
                    foreach (var _ in IntRange(dir0.Length())) {
                        current = current + dir;
                        paths.Add(current);
                    }
                }
            }

            return paths;
        }
        
        public static (bool, Point) FallOneSand(HashSet<Point> paths, HashSet<Point> sands, int maxY) {
            Point start = new(500, 0);
            Point current = start;
            bool stopped = false;

            while (!stopped && current.Y < maxY) {
                Point newPoint = current + new Vector(0, 1);
                if (paths.Contains(newPoint) || sands.Contains(newPoint)) {
                    newPoint = current + new Vector(-1, 1);
                        if (paths.Contains(newPoint) || sands.Contains(newPoint)) {
                        newPoint = current + new Vector(1, 1);
                        if (paths.Contains(newPoint) || sands.Contains(newPoint)) {
                            stopped = true;
                            break;
                        }
                    }
                }
                current = newPoint;
            }

            return (stopped, current);
        }

        public static HashSet<Point> FallAllSand(HashSet<Point> paths) {
            HashSet<Point> sands = new();
            int maxY = paths.Select(p => p.Y).Max();

            bool stopped = true;
            Point newSand;

            while (stopped) {
                (stopped, newSand) = FallOneSand(paths, sands, maxY);
                if (stopped) {
                    sands.Add(newSand);
                }
                if (newSand == new Point(500, 0)) {
                    break;
                }
            }

            return sands;
        }


    }

    public static class Part1 {
        public static string solve(bool useExample) {
            
            Input reader = new Input(useExample: useExample);

            string result = "";
            int resultInt = 0;

            var input = reader.ReadLines();

            var paths = CreatePaths(input.ToList());
            var sands = FallAllSand(paths);
            resultInt = sands.Count;



            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }



    public static class Part2 {
        public static string solve(bool useExample) {

            Input reader = new Input(useExample: useExample);
            
            string result = "";
            int resultInt = 0;

            
            var input = reader.ReadLines();

            var paths = CreatePaths(input.ToList());
            int maxY = paths.Select(p => p.Y).Max();
            foreach(int i in IntRange(500 - maxY - 10, 500 + maxY + 10)) {
                paths.Add(new Point(i, maxY + 2));
            }
            var sands = FallAllSand(paths);
            resultInt = sands.Count;


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }

}