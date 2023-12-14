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
    using RobotNums = VectorMultiDim;

    public static class Common {
        static Logger logger = new Logger(writeToFile: false);
        public static void Log(params string[] args) => logger.Log(args);
        public static void Log(params object[] args) => logger.Log(args);
        public static void LogEnum<T>(IEnumerable<T> en) => logger.LogEnum(en);

        public static int Triangular(int n) => n < 0 ? 0 : n * (n + 1) / 2;

        // Assuming to start with materials materials, robotnums robots
        // and that a Geode robot costs cost
        // then we need (return value) turns before we have enough obsidian/ore
        // to build the next geode
        // assuming of course that we build an obsidian and ore robot each turn
        public static int MinTimeUntilCanConstructNewGeode(
            Materials materials,
            RobotNums robotNums,
            Materials cost
        ) {
            return IntRange(10)
                .Where(i => cost.IsLessOrEqual(
                    materials + i * robotNums + Triangular(i - 1) * new Materials(1, 0, 1, 0)
                ))
                .First();
        }
    }

    public static class Ext {
        public static bool IsLessOrEqual(this VectorMultiDim l, VectorMultiDim r) {
            if (l.Dimension != r.Dimension) {
                throw new ArgumentException();
            }
            return l.Components
                .Zip(r.Components)
                .SelectTuple((i, j) => i <= j)
                .All();
        }

        public static bool IsNotLess(this VectorMultiDim l, VectorMultiDim r) {
            return !l.IsLessOrEqual(r);
        }
    }

    public class Factory {
        public int id;
        Materials OreRobotCost;
        Materials ClyRobotCost;
        Materials ObsRobotCost;
        Materials GeoRobotCost;

        Materials MaxRobotCost;
        Materials[] RobotCosts;

        public Factory(List<int> blueprint) {
            id = blueprint[0];
            OreRobotCost = new Materials(blueprint[1], 0, 0, 0);
            ClyRobotCost = new Materials(blueprint[2], 0, 0, 0);
            ObsRobotCost = new Materials(blueprint[3], blueprint[4], 0, 0);
            GeoRobotCost = new Materials(blueprint[5], 0, blueprint[6], 0);
            RobotCosts = Arr(OreRobotCost, ClyRobotCost, ObsRobotCost, GeoRobotCost);

            MaxRobotCost = new Materials(
                Arr(blueprint[1], blueprint[2], blueprint[3], blueprint[5]).Max(),
                blueprint[4],
                blueprint[6],
                9999
            );
        }

        public int Estimate(
            Materials currentMaterials,
            RobotNums currentRobots,
            Materials geodeCost,
            int currentTime
        ) {
            int untilNewGeode = MinTimeUntilCanConstructNewGeode(
                currentMaterials,
                currentRobots,
                geodeCost
            );
            return currentMaterials.W
                + currentTime * currentRobots.W
                + Triangular(currentTime - 1 - untilNewGeode);
        }

        public static bool[] Ok {get => Arr(false, false, false, false);}

        public int Step(
            Materials currentMaterials,
            RobotNums currentRobots,
            IEnumerable<bool> wasNotConstructed,
            int currentTime = 24,
            int maxGeodes = 0
        ) {
            if (currentTime == 1) {
                return (currentMaterials + currentRobots).W;
            }

            if (wasNotConstructed.All()) {
                return (currentMaterials + currentTime * currentRobots).W;
            }

            if ( Estimate(currentMaterials, currentRobots, GeoRobotCost, currentTime) <= maxGeodes ) {
                return -1;
            }

            // We should not build a robot that would put our production
            // over the maximum we are capable of spending
            IEnumerable<bool> shouldBeConstructed = currentRobots.Components
                .Zip(MaxRobotCost.Components)
                .SelectTuple((robotNum, maxCost) => robotNum < maxCost);

            // We can build a robot its cost is <= than our current materials
            IEnumerable<bool> canBeConstructed = RobotCosts
                .Select(cost => cost.IsLessOrEqual(currentMaterials));

            // We can skip construction if 
            // We don't have enogh resources to build a particular kind of robot
            // and we still don't have enough of them
            bool allowedToConstructNothing = canBeConstructed
                .Zip(shouldBeConstructed)
                .SelectTuple((a, b) => !a && b)
                .AnyBool();

            // We should not build a robot if
            // We cannot pay for it
            // or we don't need it
            // or we didn't build it at a previous step
            IEnumerable<bool> shouldNotBuild = canBeConstructed
                .Zip(shouldBeConstructed, wasNotConstructed)
                .Select(tuple => !tuple.First || !tuple.Second || tuple.Third);

            foreach( (int i, bool shouldNot) in shouldNotBuild.Enumerate().Reverse() ) {
                if (shouldNot) {
                    continue;
                }
                int newGeodes = Step(
                    currentMaterials + currentRobots - RobotCosts[i],
                    currentRobots + RobotNums.FromAxis(i, 4),
                    Factory.Ok,
                    currentTime - 1,
                    maxGeodes
                );
                maxGeodes = Max(maxGeodes, newGeodes);
            }

            IEnumerable<bool> wasSkipped = canBeConstructed
                .Zip(shouldBeConstructed, wasNotConstructed)
                .Select(tuple => tuple.First || !tuple.Second || tuple.Third);

            // If we can build no robot
            if (allowedToConstructNothing) {
                int newGeodes = Step(
                    currentMaterials + currentRobots,
                    currentRobots,
                    wasSkipped,
                    currentTime - 1,
                    maxGeodes
                );
                maxGeodes = Max(maxGeodes, newGeodes);
            }

            return maxGeodes;
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
                resultInt += factory.id * factory.Step(
                    new VectorMultiDim(0, 0, 0, 0),
                    new VectorMultiDim(1, 0, 0, 0),
                    Factory.Ok,
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

            string result = "0";
            int resultInt = 1;

            var blueprintData = reader.ReadInts();

            foreach (var line in blueprintData.Take(3)) {
                Factory factory = new(line);
                resultInt *= factory.Step(
                    new VectorMultiDim(0, 0, 0, 0),
                    new VectorMultiDim(1, 0, 0, 0),
                    Factory.Ok,
                    32
                );
            }


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }

}