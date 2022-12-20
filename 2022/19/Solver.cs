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
    using Materials = VectorMultiDim;
    using Robots = VectorMultiDim;

    public static class Common {
        static Logger logger = new Logger(writeToFile: false);
        public static void Log(params string[] args) => logger.Log(args);
        public static void Log(params object[] args) => logger.Log(args);
        public static void LogEnum<T>(IEnumerable<T> en) => logger.LogEnum(en);

    }

    public static class Ext {
        public static bool IsLessOrEqual(this VectorMultiDim l, VectorMultiDim r) {
            if (l.Dimension != r.Dimension) {
                throw new ArgumentException();
            }
            return l.Components.ZipApply(r.Components, (i, j) => i <= j).All();
        }

        public static int DivideBy(this VectorMultiDim l, VectorMultiDim r) {
            if (l.Dimension != r.Dimension) {
                throw new ArgumentException();
            }
            
            var lc = l.Components;
            var rc = r.Components;
            if (lc.ZipApply(rc, (i, j) => i==0 && j==0).All()) {
                return 0;
            }
            return lc.ZipApply(rc, (i, j) => j != 0 ? i / j : int.MaxValue).Min();
        }
    }

    public class Factory {
        public int id;
        Materials OreMat;
        Materials ClayMat;
        Materials ObsMat;
        Materials GeodeMat;
        Materials[] RobotCosts;
        // int StartTime;
        public Factory(List<int> blueprint) {
            id = blueprint[0];
            OreMat = new Materials(blueprint[1], 0,0,0);
            ClayMat = new Materials(blueprint[2], 0,0,0);
            ObsMat = new Materials(blueprint[3], blueprint[4], 0,0);
            GeodeMat = new Materials(blueprint[5], 0, blueprint[6],0);
            RobotCosts = Arr(OreMat, ClayMat, ObsMat, GeodeMat);
            // StartTime = 24;
        }

        public List<int[]> SelectConstructed(
            Materials resources,
            bool[] constructionBlocked
        ) {
            List<int[]> possibilities = new(){Arr(0,0,0,0)};

            foreach ((int i, Materials mat) in RobotCosts.Enumerate().Reverse()) {
                List<int[]> newList = new();
                foreach (int[] poss in possibilities) {
                    Materials newRes = resources
                    - RobotCosts
                        .ZipApply(poss, (v, k) => v * k)
                        .Aggregate((r1, r2) => r1 + r2);
                    foreach (int k in IntRange(newRes / mat + 1)) {
                        int[] newPoss = poss.Copy();
                        newPoss[i] = k;
                        newList.Add(newPoss);
                    }
                }
                possibilities.Clear();
                possibilities = newList;
            }
            return possibilities;
        }

        public int NumGeodes(
            VectorMultiDim resources,
            VectorMultiDim numRobots,
            bool[] constructionBlocked,
            int time
        ){
            if (time == 1) {
                return numRobots.Components[3];
            }
            resources += numRobots;
            int maxGeodes = 0;
            foreach(int[] l in SelectConstructed(resources, constructionBlocked)) {
                VectorMultiDim newRobots = numRobots + new VectorMultiDim(l);
                VectorMultiDim newRes = RobotCosts
                    .ZipApply(l, (v, k) => v* k)
                    .Aggregate(resources, (r, v) => r - v);

                maxGeodes = Max(maxGeodes, NumGeodes(newRes, newRobots, constructionBlocked, time - 1));
            }
            
            return maxGeodes + numRobots.Components[3];
        }
    }

    public static class Part1 {
        public static string solve(bool useExample) {
            
            Input reader = new Input(useExample: useExample);

            string result = "0";
            int resultInt = 0;

            var blueprintData = reader.ReadInts();

            foreach (var line in blueprintData) {
                Factory factory = new(line);
                resultInt += factory.id * factory.NumGeodes(
                    new VectorMultiDim(0, 0, 0, 0),
                    new VectorMultiDim(1, 0, 0, 0),
                    Arr(false),
                    24
                );
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
            
            string result = "";
            int resultInt = 0;

            


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }

}