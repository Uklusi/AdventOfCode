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

        public static int CalcGalaxyDistance(Point a, Point b, List<int> expandedRows, List<int> expandedCols) {
            return a.Distance(b) +
                expandedRows.Where(i => i.IsInInterval(Min(a.Y, b.Y), Max(a.Y, b.Y))).Count() + 
                expandedCols.Where(i => i.IsInInterval(Min(a.X, b.X), Max(a.X, b.X))).Count();
        }

        public static string solve(bool useExample) {
            
            Input reader = new Input(useExample: useExample);

            string result = "0";
            long resultInt = 0;

            var data = reader.ReadLines();

            char[,] map = data.ToDoubleArray();

            List<int> expandedRows = map
                .GetRows()
                .Enumerate()
                .Where(t => t.item.All(c => c == '.'))
                .Select(t => t.index)
                .ToList();
            
            List<int> expandedCols = map
                .GetCols()
                .Enumerate()
                .Where(t => t.item.All(c => c == '.'))
                .Select(t => t.index)
                .ToList();

            List<Point> galaxies = data
                .Enumerate2D()
                .Where(t => t.item == '#')
                .Select(t => t.p)
                .ToList();

            foreach (var (i, p) in galaxies.Enumerate()) {
                foreach (var j in IntRange(i + 1, galaxies.Count)) {
                    var q = galaxies[j];
                    resultInt += CalcGalaxyDistance(p, q, expandedRows, expandedCols);
                }
            }

            


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }



    public static class Part2 {

        public static long expansion = 1000000;
        
        public static long CalcGalaxyDistance(Point a, Point b, List<int> expandedRows, List<int> expandedCols) {
            return a.Distance(b) +
                expandedRows.Where(i => i.IsInInterval(Min(a.Y, b.Y), Max(a.Y, b.Y))).Count() * (expansion - 1) + 
                expandedCols.Where(i => i.IsInInterval(Min(a.X, b.X), Max(a.X, b.X))).Count() * (expansion - 1);
        }

        public static string solve(bool useExample) {

            Input reader = new Input(useExample: useExample);
            
            string result = "0";
            long resultInt = 0;

            
            var data = reader.ReadLines();

            char[,] map = data.ToDoubleArray();

            List<int> expandedRows = map
                .GetRows()
                .Enumerate()
                .Where(t => t.item.All(c => c == '.'))
                .Select(t => t.index)
                .ToList();
            
            List<int> expandedCols = map
                .GetCols()
                .Enumerate()
                .Where(t => t.item.All(c => c == '.'))
                .Select(t => t.index)
                .ToList();

            List<Point> galaxies = data
                .Enumerate2D()
                .Where(t => t.item == '#')
                .Select(t => t.p)
                .ToList();

            foreach (var (i, p) in galaxies.Enumerate()) {
                foreach (var j in IntRange(i + 1, galaxies.Count)) {
                    var q = galaxies[j];
                    resultInt += CalcGalaxyDistance(p, q, expandedRows, expandedCols);
                }
            }


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }

}