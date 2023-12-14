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

        public static (int, Point) getMapNumber(Dictionary<Point, char> map, Point currentPos) {
            // Common.Log(currentPos);
            Vector vNext = new(1, 0);
            Point newPos = currentPos - vNext;
            while (map.ContainsKey(newPos) && char.IsDigit(map[newPos])) {
                currentPos = newPos;
                newPos = currentPos - vNext;
            }
            string numStr = "";
            while (map.ContainsKey(currentPos) && char.IsDigit(map[currentPos])) {
                numStr += map[currentPos].ToString();
                currentPos += vNext;
            }
            // Common.Log(numStr);
            return (numStr.ToInt(), currentPos);

        }
    }

    public static class Part1 {
        public static string solve(bool useExample) {
            
            Input reader = new Input(useExample: useExample);

            string result = "0";
            long resultInt = 0;

            Dictionary<Point, char> map = new();
            List<Point> parts = new();

            foreach (var (y, line) in reader.ReadLines().Enumerate()) {
                foreach (var (x, c) in line.Enumerate()) {
                    if (c != '.') {
                        map[new Point(x, y)] = c;
                        if (! char.IsDigit(c)) {
                            parts.Add(new Point(x, y));
                        }
                    }
                }
            }

            HashSet<Point> visited = new();

            foreach (Point p in parts) {
                foreach (Point q in p.Adjacent(corners: true)) {
                    if (map.ContainsKey(q)) {
                        var (num, startPos) = getMapNumber(map, q);
                        if (! visited.Contains(startPos)) {
                            // Common.Log(startPos, num);
                            visited.Add(startPos);
                            resultInt += num;
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
        public static string solve(bool useExample) {

            Input reader = new Input(useExample: useExample);
            
            string result = "0";
            long resultInt = 0;

            Dictionary<Point, char> map = new();
            List<Point> tentativeGears = new();

            foreach (var (y, line) in reader.ReadLines().Enumerate()) {
                foreach (var (x, c) in line.Enumerate()) {
                    if (c != '.') {
                        map[new Point(x, y)] = c;
                        if (c == '*') {
                            tentativeGears.Add(new Point(x, y));
                        }
                    }
                }
            }

            foreach (Point p in tentativeGears) {
                Dictionary<Point, int> visited = new();
                foreach (Point q in p.Adjacent(corners: true)) {
                    if (map.ContainsKey(q)) {
                        var (num, startPos) = getMapNumber(map, q);
                        if (! visited.ContainsKey(startPos)) {
                            // Common.Log(startPos, num);
                            visited[startPos] = num;
                        }
                    }
                }
                if (visited.Count == 2) {
                    resultInt += visited.Values.Aggregate((a, b) => a * b);
                }
            }


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }

}