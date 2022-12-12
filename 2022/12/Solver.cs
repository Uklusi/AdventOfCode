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

        public static int GetHeight(Map map, MapPoint p) {
            return map[p] switch {
            'S' => (int)'a',
            'E' => (int)'z',
            var s => (int)s
        };}
        
        public static Dictionary<MapPoint, int> BFS(Map map, MapPoint start, MapPoint end) {
            Dictionary<MapPoint, int> visited = new();
            visited.Add(start, 0);

            Queue<MapPoint> toVisitNeighs = new();
            toVisitNeighs.Enqueue(start);

            while (toVisitNeighs.Count > 0 && ! visited.ContainsKey(end)) {
                MapPoint p = toVisitNeighs.Dequeue();
                int height = GetHeight(map, p);

                foreach ( MapPoint q in p.Adjacent() ) {
                    int newHeight = GetHeight(map, q);
                    if (! visited.ContainsKey(q) && newHeight <= height + 1) {
                        int steps = visited[p] + 1;
                        visited.Add(q, steps);
                        toVisitNeighs.Enqueue(q);
                        if (q == end) {
                            return visited;
                        }
                    }
                }
            }

            return visited;


        }

        public static int BFSInverse(Map map, MapPoint start) {
            Dictionary<MapPoint, int> visited = new();
            visited.Add(start, 0);

            Queue<MapPoint> toVisitNeighs = new();
            toVisitNeighs.Enqueue(start);

            while (toVisitNeighs.Count > 0) {
                MapPoint p = toVisitNeighs.Dequeue();
                int height = GetHeight(map, p);

                foreach ( MapPoint q in p.Adjacent() ) {
                    int newHeight = GetHeight(map, q);
                    if (! visited.ContainsKey(q) && newHeight >= height - 1) {
                        int steps = visited[p] + 1;
                        visited.Add(q, steps);
                        toVisitNeighs.Enqueue(q);
                        if (newHeight == (int)'a') {
                            return steps;
                        }
                    }
                }
            }

            return -1;


        }
        

    }
    public class Map {
                public char[,] _map;
                public Map(char[,] map){
                    _map = map;
                }

                public char this[Point p] => _map[p.Y, p.X];
            }

    public static class Part1 {
        public static string solve(bool useExample) {
            
            Input reader = new Input(useExample: useExample);

            string result = "";
            int resultInt = 0;

            var data = reader.ReadLines();

            Map map = new Map(data.ToDoubleArray());

            MapPoint.SetFrame(data);

            (int rowIndex, var row) = data.Enumerate().Where(r => r.item.Contains('S')).GetOne();
            int colIndex = row.Enumerate().Where(c => c.item == 'S').GetOne().index;
            MapPoint start = new(colIndex, rowIndex);

            (rowIndex, row) = data.Enumerate().Where(r => r.item.Contains('E')).GetOne();
            colIndex = row.Enumerate().Where(c => c.item == 'E').GetOne().index;
            MapPoint end = new(colIndex, rowIndex);

            resultInt = BFS(map, start, end)[end];




            


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

            var data = reader.ReadLines();

            Map map = new Map(data.ToDoubleArray());

            MapPoint.SetFrame(data);

            (int rowIndex, var row) = data.Enumerate().Where(r => r.item.Contains('S')).GetOne();
            int colIndex = row.Enumerate().Where(c => c.item == 'S').GetOne().index;
            MapPoint start = new(colIndex, rowIndex);

            (rowIndex, row) = data.Enumerate().Where(r => r.item.Contains('E')).GetOne();
            colIndex = row.Enumerate().Where(c => c.item == 'E').GetOne().index;
            MapPoint end = new(colIndex, rowIndex);

            resultInt = BFSInverse(map, end);


            


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }

}