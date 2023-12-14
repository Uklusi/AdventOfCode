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
        static Logger logger = new Logger(writeToFile: true);
        public static void Log(params string[] args) => logger.Log(args);
        public static void Log(params object[] args) => logger.Log(args);
        public static void LogEnum<T>(IEnumerable<T> en) => logger.LogEnum(en);
        
        

    }

    public class Valley {
        public int limit;
        public List<Movable> blizzards;
        public HashSet<MapPoint> currents;

        public MapPoint start;
        public MapPoint end;

        public Valley(List<string> mapData) {
            Movable.SetFrame(mapData);
            Movable.SetOccupied(p => MapPoint.Frame?[p.Y]?[p.X] == '#');

            int xStart = mapData[0].IndexOf('.');
            int xEnd = mapData[^1].IndexOf('.');
            start = new MapPoint(xStart, 0);
            end = new MapPoint(xEnd, mapData.Count - 1);

            currents = Arr<MapPoint>().ToHashSet();
            blizzards = new();
            limit = Max(mapData[0].Length, mapData.Count);

            foreach (var (y, l) in mapData.Enumerate()) {
                foreach (var (x, c) in l.Enumerate()) {
                    if (!c.IsIn(Arr('#', '.'))) {
                        Movable m = new Movable(x, y, new Direction(c), upIsNegative: true);
                        blizzards.Add(m);
                    }
                }
            }
        }

        public void MoveCircular(Movable m) {
            MapPoint p = m.ToMapPoint();
            m.Move();
            if (m == p) {
                m.Move(-limit, m.dir);
            }
        }

        public void Step() {
            HashSet<MapPoint> possibilities = currents.ToHashSet();
            foreach (MapPoint p in currents) {
                possibilities.UnionWith(p.Adjacent());
            }
            foreach (Movable m in blizzards) {
                MoveCircular(m);
                possibilities.Remove(m.ToMapPoint());
            }
            currents.Clear();
            currents = possibilities;
        }

        public int StepsToEnd() {
            currents.Clear();
            currents.Add(start);
            int steps = 0;
            while (!currents.Contains(end)) {
                Step();
                steps++;
            }
            return steps;
        }

        public int StepsToStart() {
            currents.Clear();
            currents.Add(end);
            int steps = 0;
            while (!currents.Contains(start)) {
                Step();
                steps++;
            }
            return steps;
        }
    }

    public static class Part1 {
        public static string solve(bool useExample) {
            
            Input reader = new Input(useExample: useExample);

            string result = "0";
            long resultInt = 0;

            var mapData = reader.ReadLines();

            Valley valley = new(mapData);

            resultInt = valley.StepsToEnd();

            


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

            var mapData = reader.ReadLines();

            Valley valley = new(mapData);

            resultInt += valley.StepsToEnd();
            resultInt += valley.StepsToStart();
            resultInt += valley.StepsToEnd();

            


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }

}