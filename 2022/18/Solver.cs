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
        
        

    }

    public static class Part1 {
        public static string solve(bool useExample) {
            
            Input reader = new Input(useExample: useExample);

            string result = "";
            int resultInt = 0;

            List<List<int>> coords = reader.ReadInts();
            HashSet<PointMultiDim> points = new();

            foreach(List<int> line in coords) {
                points.Add(new PointMultiDim(line));
            }
            foreach(PointMultiDim point in points) {
                resultInt += point
                    .Adjacent()
                    .Where(p => !points.Contains(p))
                    .Count();
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

            List<List<int>> coords = reader.ReadInts();
            HashSet<PointMultiDim> points = new();

            foreach(List<int> line in coords) {
                points.Add(new PointMultiDim(line));
            }

            HashSet<PointMultiDim> surfacePoints = new();
            foreach(PointMultiDim point in points) {
                surfacePoints.UnionWith(
                    point
                    .Adjacent()
                    .Where(p => !points.Contains(p))
                );
            }



            int minX = points
                .Select(p => p.Coordinates[0])
                .Min();
            
            PointMultiDim p0 = points
                .Where(p => p.Coordinates[0] == minX)
                .First();
            PointMultiDim start = p0 + new VectorMultiDim(-1, 0, 0);

            HashSet<PointMultiDim> toCheck = new();
            toCheck.Add(start);
            HashSet<PointMultiDim> doneCheck = new();
            
            while (toCheck.NotEmpty()) {
                doneCheck.UnionWith(toCheck);

                HashSet<PointMultiDim> newCheck = new();
                foreach (PointMultiDim p in toCheck) {
                    var check1 = p.Adjacent().Where(
                        x =>
                            !points.Contains(x)
                            && !doneCheck.Contains(x)
                    );
                    newCheck.UnionWith(
                        check1.Where(surfacePoints.Contains)
                    );
                    foreach (PointMultiDim q in check1) {
                        newCheck.UnionWith(
                            q.Adjacent()
                            .Where(
                                x =>
                                    !points.Contains(x)
                                    && !doneCheck.Contains(x)
                                    && surfacePoints.Contains(x)    
                            )
                        );
                    }
                }
                toCheck.Clear();
                toCheck = newCheck;
            }

            resultInt = points.Select(
                p => p.Adjacent().Where(doneCheck.Contains).Count()
            ).Sum();

            


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }

}