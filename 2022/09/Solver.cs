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
    using static AoC.Common;

    public static class Common {
        static Logger logger = new Logger("log");
        public static void Log(params string[] args) => logger.Log(args);

        public static Point MoveTowards(this Point tail, Point head) {
            if ((head != tail && ! head.Adjacent(corners: true).Contains(tail))) {
                Vector v = (head - tail).Normalized();
                tail += new Vector(Sign(v.X), Sign(v.Y));
            }
            return tail;
        }
        public static HashSet<Point> TailVisited(List<string> instructions, int numKnots) {
            Movable head = new();
            Point[] tails = new Point[numKnots];
            tails[0] = head.ToPoint();
            for (int i = 1; i < numKnots; i++) {
                tails[i] = new Point();
            }
            HashSet<Point> tailVisited = new();
            tailVisited.Add(tails[^1]);
            foreach (string line in instructions) {
                string[] tokens = line.Split(" ");
                string dir = tokens[0];
                int numTimes = tokens[1].ToInt();
                for (int i = 0; i < numTimes; i++) {
                    head.Move(1, new Direction(dir));
                    tails[0] = head.ToPoint();
                    for (int j = 1; j < numKnots; j++) {
                        Point newT = tails[j].MoveTowards(tails[j-1]);
                        tails[j] = newT;
                    }
                    tailVisited.Add(tails[^1]);
                }
            }
            return tailVisited;
        }

    }

    public static class Part1 {

        public static string solve(bool useExample) {
            
            var inputReader = new Input(useExample: useExample);

            string result = "";
            int resultInt = 0;

            var instructions = inputReader.ReadLines();
            
            resultInt = TailVisited(instructions, 2).Count;

            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }

    public static class Part2 {

        public static string solve(bool useExample) {

            var inputReader = new Input(useExample: useExample);
            
            string result = "";
            int resultInt = 0;

            var instructions = inputReader.ReadLines();

            resultInt = TailVisited(instructions, 10).Count;

            


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }

}