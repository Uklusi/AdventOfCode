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

            string result = "0";
            long resultInt = 1;

            var data = reader.ReadInts();
            var tdSequences = Zip(data[0], data[1]);

            foreach (var (t, d) in tdSequences) {
                int minTime = (int)Floor((t - Sqrt(t * t - 4 * d)) / 2) + 1;
                resultInt *= t - 2 * minTime + 1;

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

            var data = reader.ReadTokens();
            long t = long.Parse(data[0].Skip(1).JoinString(""));
            long d = long.Parse(data[1].Skip(1).JoinString(""));

            long minTime = (long)Floor((t - Sqrt(t * t - 4 * d)) / 2) + 1;
            resultInt = t - 2 * minTime + 1;


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }

}