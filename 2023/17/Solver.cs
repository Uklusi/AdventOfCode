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
        
        public static int WeirdDijstra(MapPoint start, Direction dir, Point target, Grid2D<int> weights){
            PriorityQueue<(Point, Direction, int), int> queue = new();

            PriorityQueue<(Point, Direction, int), int> openSet = new();
            Dictionary<(Point, Direction, int), int> distances = new(){{(start, dir, 0), 0}};
            Dictionary<(Point, Direction, int), (Point, Direction, int)> prev = new();
            openSet.Enqueue((start, dir, 0), 0);
            
            while (openSet.NotEmpty()) {
                var oldKey = openSet.Dequeue();
                var (p, d, straightCount) = oldKey;
                MapPoint current = new(p.X, p.Y);
                int oldDistance = distances[oldKey];
                if (p == target) {
                    // var walk = oldKey;
                    // List<((Point, Direction, int), int)> path = new();
                    // while (prev.ContainsKey(walk)) {
                    //     path.Add((walk, distances[walk]));
                    //     walk = prev[walk];
                    // }
                    // path.Add(((start, dir, 0), 0));
                    // path.Reverse();
                    // Log(path.ReprList("\n"));
                    return oldDistance;
                }
                foreach (MapPoint newPoint in current.Adjacent()) {
                    straightCount = oldKey.Item3;
                    Direction movement = (newPoint - current).ToDirection();
                    if (movement == -d) {
                        continue;
                    }
                    if (movement == d) {
                        straightCount++;
                    }
                    else {
                        straightCount = 1;
                    }
                    if (straightCount > 3) {
                        continue;
                    }


                    int tentativeDistance = oldDistance + weights[newPoint.ToPoint()];
                    var newKey = (newPoint.ToPoint(), movement, straightCount);
                    if (
                        !distances.ContainsKey(newKey) ||
                        distances[newKey] > tentativeDistance
                    ) {
                        distances[newKey] = tentativeDistance;
                        openSet.Enqueue(newKey, tentativeDistance);
                        prev[newKey] = oldKey;
                    }
                }
                
            }
            throw InPanic();
        }

        public static int WeirderDijstra(MapPoint start, Direction dir, Point target, Grid2D<int> weights){
            PriorityQueue<(Point, Direction, int), int> queue = new();

            PriorityQueue<(Point, Direction, int), int> openSet = new();
            Dictionary<(Point, Direction, int), int> distances = new(){{(start, dir, 0), 0}};
            // Dictionary<(Point, Direction, int), (Point, Direction, int)> prev = new();
            openSet.Enqueue((start, dir, 0), 0);
            
            while (openSet.NotEmpty()) {
                var oldKey = openSet.Dequeue();
                var (p, d, straightCount) = oldKey;
                MapPoint current = new(p.X, p.Y);
                int oldDistance = distances[oldKey];
                if (p == target) {
                    // var walk = oldKey;
                    // List<((Point, Direction, int), int)> path = new();
                    // while (prev.ContainsKey(walk)) {
                    //     path.Add((walk, distances[walk]));
                    //     walk = prev[walk];
                    // }
                    // path.Add(((start, dir, 0), 0));
                    // path.Reverse();
                    // Log(path.ReprList("\n"));
                    return oldDistance;
                }
                foreach (MapPoint newPoint in current.Adjacent()) {
                    straightCount = oldKey.Item3;
                    Direction movement = (newPoint - current).ToDirection();
                    if (movement == -d) {
                        continue;
                    }
                    else if (movement == d) {
                        straightCount++;
                        if (straightCount > 10) {
                            continue;
                        }
                    }
                    else if (straightCount < 4) {
                        continue;
                    }
                    else {
                        straightCount = 1;
                    }
                    


                    int tentativeDistance = oldDistance + weights[newPoint.ToPoint()];
                    var newKey = (newPoint.ToPoint(), movement, straightCount);
                    if (
                        !distances.ContainsKey(newKey) ||
                        distances[newKey] > tentativeDistance
                    ) {
                        distances[newKey] = tentativeDistance;
                        openSet.Enqueue(newKey, tentativeDistance);
                        // prev[newKey] = oldKey;
                    }
                }
                
            }
            throw InPanic();
        }

    }

    public static class Part1 {
        public static string solve(bool useExample) {
            
            Input reader = new Input(useExample: useExample);

            string result = "0";
            long resultInt = 0;

            var data = reader.ReadLines();
            var dataInt = data.Select(l => l.Select(c => c.ToInt()));

            Grid2D<int> map = new(dataInt);

            MapPoint.SetFrame(data);
            resultInt = WeirdDijstra(new MapPoint(0, 0), new Direction("R"), new Point(map.Shape.cols - 1, map.Shape.rows - 1), map);


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
            var dataInt = data.Select(l => l.Select(c => c.ToInt()));

            Grid2D<int> map = new(dataInt);

            MapPoint.SetFrame(data);
            resultInt = WeirderDijstra(new MapPoint(0, 0), new Direction("R"), new Point(map.Shape.cols - 1, map.Shape.rows - 1), map);



            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }

}