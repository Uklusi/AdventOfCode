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
        
        

    }

    public static class Part1 {
        public static string solve(bool useExample) {
            
            Input reader = new Input(useExample: useExample);

            string result = "";
            int resultInt = 0;

            var sensorData = reader.ReadInts();

            Dictionary<Point, Point> relativeBeacon = new();
            Dictionary<Point, int> relativeDistance = new();
            foreach (var line in sensorData) {
                Point sensor = new(line[0], line[1]);
                Point beacon = new(line[2], line[3]);
                relativeBeacon[sensor] = beacon;
                relativeDistance[sensor] = sensor.Distance(beacon);
            }

            int rowToCheck;
            if (useExample) {
                rowToCheck = 10;
            }
            else {
                rowToCheck = 2_000_000;
            }

            int minCheck = relativeBeacon.First().Key.X;
            int maxCheck = minCheck;

            foreach(var sensor in relativeBeacon.Keys) {
                minCheck = Min(
                    minCheck,
                    sensor.X - relativeDistance[sensor] + Abs(sensor.Y - rowToCheck)
                );
                maxCheck = Max(
                    maxCheck,
                    sensor.X + relativeDistance[sensor] - Abs(sensor.Y - rowToCheck)
                );
            }

            foreach (int i in IntRange(minCheck, maxCheck + 1)) {
                bool isIn = false;
                Point toCheck = new(i, rowToCheck);
                foreach (var sensor in relativeBeacon.Keys) {
                    if (relativeDistance[sensor] >= sensor.Distance(toCheck)) {
                        isIn = true;
                        break;
                    }
                }
                if (isIn && !toCheck.IsIn(relativeBeacon.Values)) {
                    resultInt++;
                }
            }


            


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }



    public static class Part2 {
        public static (int, int) MinMax(Point sensor, int distance, int row, int limit) {
            int effectiveRange = distance - Abs(sensor.Y - row);
            if (effectiveRange < 0) {
                return (-1, -1);
            }
            return (
                Max(sensor.X - effectiveRange, 0),
                Min(sensor.X + effectiveRange, limit)
            );
        }

        public static string solve(bool useExample) {

            Input reader = new Input(useExample: useExample);
            
            string result = "";
            int resultInt = 0;

            var sensorData = reader.ReadInts();

            Dictionary<Point, Point> relativeBeacon = new();
            Dictionary<Point, int> relativeDistance = new();
            foreach (var line in sensorData) {
                Point sensor = new(line[0], line[1]);
                Point beacon = new(line[2], line[3]);
                relativeBeacon[sensor] = beacon;
                relativeDistance[sensor] = sensor.Distance(beacon);
            }

            int limit;
            if (useExample) {
                limit = 20;
            }
            else {
                limit = 4_000_000;
            }

            (int, int) resultTuple = (-1, -1);

            foreach(var (sensor, distance) in relativeDistance) {
            }

            foreach (int i in IntRange(0, limit + 1)) {
                List<(int, int)> listLimits = new();
                foreach( var (sensor, distance) in relativeDistance) {
                    var (min, max) = MinMax(sensor, distance, i, limit);
                    if (min != -1) {
                        listLimits.Add((min, max));
                    }
                }
                listLimits.Sort();
                int maxLimit = 0;
                if (listLimits[0].Item1 != 0) {
                    resultTuple = (0, i);
                    break;
                }
                foreach ( var t in listLimits) {
                    if (maxLimit < t.Item1 - 1) {
                        resultTuple = (maxLimit + 1, i);
                        break;
                    }
                    maxLimit = Max(maxLimit, t.Item2);
                }
                if (resultTuple != (-1, -1)) {
                    break;
                }
                if (maxLimit != limit) {
                    resultTuple = (limit, i);
                    break;
                }
            }

            result = (resultTuple.Item1 * 4_000_000L + resultTuple.Item2).ToString();


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }

}