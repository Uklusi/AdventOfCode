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
        
        public static Vector left = new Direction("L").ToVector();
        public static Vector up = new Direction("U").ToVector(upIsNegative: true);
        public static Vector right = new Direction("R").ToVector();
        public static Vector down = new Direction("D").ToVector(upIsNegative: true);

    }

    public static class Part1 {
        
        public static HashSet<Point> Borders(List<(Direction, int)> instructions) {
            var set = new HashSet<Point>(){new Point(0,0)};
            Movable current = new(0,0, new Direction("R"), upIsNegative: true);
            foreach (var (dir, n) in instructions) {
                current.dir = dir;
                foreach (int _ in IntRange(n)) {
                    current.Move();
                    set.Add(current.ToPoint());
                }
            }
            return set;
        }

        public static int Interior(HashSet<Point> borders) {
            var xs = borders.Select(p => p.X);
            var (xMin, xMax) = (xs.Min(), xs.Max() + 1);
            var ys = borders.Select(p => p.Y);
            var (yMin, yMax) = (ys.Min(), ys.Max() + 1);
            int res = 0;

            foreach (int x in IntRange(xMin, xMax)) {
                res += CountDiagonal(borders, new Point(x, yMin), xMax, yMax);
            }
            foreach (int y in IntRange(yMin + 1, yMax)) {
                res += CountDiagonal(borders, new Point(xMin, y), xMax, yMax);
            }
            return res;

        }
        public static int CountDiagonal(HashSet<Point> borders, Point start, int xMax, int yMax) {
            Point current = start;
            bool inside = false;
            int result = 0;
            while (current.X < xMax && current.Y < yMax) {
                if (borders.Contains(current)) {
                    var adj = new HashSet<Point>(current.Adjacent());
                    adj.IntersectWith(borders);
                    // adj always has 2 elements in this case, so IsSupersetOf == Equals
                    if (
                        !adj.IsSupersetOf(Arr(current + right, current + up)) &&
                        !adj.IsSupersetOf(Arr(current + left, current + down))
                    ) {
                        inside = !inside;
                    }
                    result++;
                }
                else if (inside) {
                    result++;
                }
                current += right + down;
            }
            return result;
        }

        public static string solve(bool useExample) {
            
            Input reader = new Input(useExample: useExample);

            string result = "0";
            long resultInt = 0;

            var data = reader.ReadWords();

            var formattedData = data.Select(l => (new Direction(l[0]), l[1].ToInt())).ToList();

            var borders = Borders(formattedData);

            resultInt = Interior(borders);


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }



    public static class Part2 {

        public static int IntervalCompare((int, int) a, (int, int) b ) {
            return a.Item1.CompareTo(b.Item1);
        }
        public static long IntervalLength((int, int) a) {
            return a.Item2 - a.Item1 + 1;
        }

        public static List<(int y, (int min, int max) x)> Borders(List<(Direction, int)> instructions) {
            Point current = new(0,0);
            List<(int, (int, int))> ret = new();
            foreach (var (dir, n) in instructions) {
                Vector v = dir.ToVector(true);
                Point next = current + n * v;
                if (v == left || v == right) {
                    ret.Add((current.Y, MinMax(current.X, next.X)));
                }
                current = next;
            }
            ret.Sort((a, b) => a.Item1.CompareTo(b.Item1));
            return ret;
        }

        public static bool AreIntersecting((int, int) a, (int, int) b) {
            return a.Item1.IsInInterval(b) || b.Item1.IsInInterval(a);
        }
        public static (int, int)? Intersect((int, int) a, (int, int) b) {
            if (!AreIntersecting(a, b)) {
                return null;
            }
            return (Max(a.Item1, b.Item1), Min(a.Item2, b.Item2));
        }
        public static List<(int, int)> Subtract((int, int) big, (int, int) small) {
            List<(int, int)> ret = new();
            (int, int)? maybeInters = Intersect(big, small);
            if (!maybeInters.HasValue) {
                ret.Add(big);
            }
            else {
                (int, int) inters = maybeInters.Value;
                if (inters.Item1 != big.Item1) {
                    ret.Add((big.Item1, inters.Item1));
                }
                if (inters.Item2 != big.Item2) {
                    ret.Add((inters.Item2, big.Item2));
                }
            }
            return ret;
        }

        public static (int, int) AddIntersecting((int, int) a, (int, int) b) {
            // +1 and -1 are necessary in order to check if intervals are contiguous
            if (!AreIntersecting(a, (b.Item1 - 1, b.Item2 + 1))) {
                throw new Exception();
            }
            return (Min(a.Item1, b.Item1), Max(a.Item2, b.Item2));
        }

        public static (int, int) AddLeft((int, int) oldI, (int, int) newI) {
            if (!AreIntersecting(oldI, (newI.Item1 - 1, newI.Item2 + 1))) {
                return oldI;
            }
            return AddIntersecting(oldI, newI);
        }

        public static bool AreOverlapping((int, int) a, (int, int) b) {
            return (a.Item1.IsInInterval(b) && a.Item2.IsInInterval(b)) ||
                (b.Item1.IsInInterval(a) && b.Item2.IsInInterval(a));
        }

        public static long Inside(List<(int y, (int min, int max) x)> hborders) {
            long ret = 0;
            List<(int min, int max)> intervals = new();

            var first = hborders[0];
            int currY = first.y;
            intervals.Add(first.x);
            ret = IntervalLength(first.x);
            foreach (var border in hborders.Skip(1)) {
                // Log(intervals.ReprList());
                // Log("border", border);
                int newY = border.y;
                ret += (long)(newY - currY) * intervals.Select(IntervalLength).Sum();
                currY = newY;
                bool isSubtract = intervals.Any(i => AreOverlapping(i, border.x));
                if (isSubtract) {
                    intervals = intervals.Select(i => Subtract(i, border.x)).Flatten().ToList();
                }
                else {
                    int numIntersect = intervals.Where(i => AreIntersecting(i, border.x)).Count();
                    List<(int min, int max)> newIntervals = new();
                    if (numIntersect == 0) {
                        intervals.Add(border.x);
                    }
                    else if (numIntersect == 1) {
                        intervals = intervals.Select(i => AddLeft(i, border.x)).ToList();
                    }
                    else {
                        for (int k = 0; k < intervals.Count; k++) {
                            if (AreIntersecting(intervals[k], border.x)) {
                                (int, int) newInterval = AddIntersecting(intervals[k], border.x);
                                k++;
                                newInterval = AddIntersecting(newInterval, intervals[k]);
                                newIntervals.Add(newInterval);
                            }
                            else {
                                newIntervals.Add(intervals[k]);
                            }
                        }
                        intervals = newIntervals;
                    }
                    ret += IntervalLength(border.x) - numIntersect;
                }
                intervals.Sort(IntervalCompare);
            }



            return ret;
        }
        
        public static string solve(bool useExample) {

            Input reader = new Input(useExample: useExample);
            
            string result = "0";
            long resultInt = 0;

            var data = reader.ReadWords();
            List<(Direction, int)> instructions = new();

            foreach (var l in data) {
                string newData = l[2];
                int length = Convert.ToInt32(newData[..5], 16);
                // Log(newData);
                Direction d = new(
                    newData[^1] switch {
                        '0' => 'R',
                        '1' => 'D',
                        '2' => 'L',
                        '3' => 'U',
                        _ => throw InPanic()
                    }
                );
                instructions.Add((d, length));
            }

            var hborders = Borders(instructions);
            // Log(hborders.ReprList("\n"));
            resultInt = Inside(hborders);

            


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }

}