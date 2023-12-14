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
using System.Collections.Immutable;

namespace AoC {
    using static Common;

    public static class Common {
        static Logger logger = new Logger(writeToFile: false);
        public static void Log(params string[] args) => logger.Log(args);
        public static void Log(params object[] args) => logger.Log(args);
        public static void LogEnum<T>(IEnumerable<T> en) => logger.LogEnum(en);
        
        public static readonly Vector up = Vector.FromDirection(new Direction("U"), upIsNegative: true);
        public static readonly Vector down = Vector.FromDirection(new Direction("D"), upIsNegative: true);
        public static readonly Vector left = Vector.FromDirection(new Direction("L"), upIsNegative: true);
        public static readonly Vector right = Vector.FromDirection(new Direction("R"), upIsNegative: true);

        public static ImmutableHashSet<Vector> GetDirs(char c) {
            return c switch
            {
                '|' => ImmutableHashSet.Create(up, down),
                '-' => ImmutableHashSet.Create(left, right),
                'L' => ImmutableHashSet.Create(up, right),
                'J' => ImmutableHashSet.Create(up, left),
                'F' => ImmutableHashSet.Create(down, right),
                '7' => ImmutableHashSet.Create(down, left),
                _ => ImmutableHashSet.Create<Vector>(),
            };
        }

        public static Point GetNext(Point prev, Point curr, Grid2D<char> grid) {
            char currType = grid[curr];
            Vector prevMove = curr - prev;
            var newDirs = GetDirs(currType);
            if (! newDirs.Contains(-prevMove)) {
                throw new ArgumentException($"Not found: {-prevMove}");
            }
            return curr + GetDirs(currType).Remove(-prevMove).First();
        }

        public static List<Point> GetNeighs(Point start, Grid2D<char> grid) {
            return new List<Vector> {up, down, left, right}
                .Where(v => grid.Inbound(start + v) && GetDirs(grid[start + v]).Contains(-v))
                .Select(v => start + v)
                .ToList();
        }
    }

    public static class Part1 {
        public static string solve(bool useExample) {
            
            Input reader = new Input(useExample: useExample);

            string result = "0";
            long resultInt = 0;

            var data = reader.ReadLines();

            Grid2D<char> grid = new(data);

            Point start = new();

            foreach (var (p, c) in data.Enumerate2D()) {
                if (c == 'S') {
                    start = p;
                    break;
                }
            }

            if (start == new Point()) {
                throw new Exception();
            }

            var l = GetNeighs(start, grid);
            resultInt++;
            Point prevA = start;
            Point prevB = start;
            Point a = l[0];
            Point b = l[1];
            while (a != b) {
                // Common.Log(prevA, a, prevB, b);
                (prevA, a) = (a, GetNext(prevA, a, grid));
                (prevB, b) = (b, GetNext(prevB, b, grid));
                resultInt++;
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

            
            var data = reader.ReadLines();

            Grid2D<char> grid = new(data);

            Point start = new();

            foreach (var (p, c) in data.Enumerate2D()) {
                if (c == 'S') {
                    start = p;
                    break;
                }
            }

            if (start == new Point()) {
                throw new Exception();
            }


            Point prevA = start;
            Point a = GetNeighs(start, grid).First();
            HashSet<Point> visited = new() {start, a};

            while (a != start) {
                // Common.Log(prevA, a, prevB, b);
                (prevA, a) = (a, GetNext(prevA, a, grid));
                visited.Add(a);
            }

            bool inside = false;
            bool uncertain = false;
            ImmutableHashSet<Vector> startDirs = GetNeighs(start, grid).Select(p => p - start).ToImmutableHashSet();
            Vector otherDir = new();

            foreach (var (p, c) in grid._grid.GetRows().Enumerate2D()) {
                if (visited.Contains(p)) {
                    ImmutableHashSet<Vector> dirs = (p == start) ? startDirs : GetDirs(c);
                    if (dirs.Contains(right) && dirs.Contains(left)) {}
                    else if (dirs.Contains(right) && !dirs.Contains(left)) {
                        uncertain = true;
                        otherDir = dirs.Remove(right).First();
                    }
                    else if (uncertain) {
                        uncertain = false;
                        if (otherDir != dirs.Remove(left).First()) {
                            inside = !inside;
                        }
                    }
                    else {
                        inside = !inside;
                    }
                }
                else if (inside) {
                    resultInt++;
                }
            }

            


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }

}