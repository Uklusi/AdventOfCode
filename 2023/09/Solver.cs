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
        
        public static List<int> CalcDiffs(List<int> l) {
            return l
                .Skip(1)
                .ZipSelect(l, (a, b) => a - b).ToList();
        }

    }

    public static class Part1 {
        public static string solve(bool useExample) {
            
            Input reader = new Input(useExample: useExample);

            string result = "0";
            long resultInt = 0;

            var data = reader.ReadInts();

            foreach (var line in data) {
                var temp = line.ToList();
                while (temp.Any(i => i != 0)) {
                    // Common.Log(temp.Stringify());
                    resultInt += temp[^1];
                    temp = CalcDiffs(temp);
                }

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

            var data = reader.ReadInts();

            foreach (var line in data) {
                var temp = line.ToList();
                var sign = 1;
                while (temp.Any(i => i != 0)) {
                    // Common.Log(temp.Stringify());
                    resultInt += sign * temp[0];
                    temp = CalcDiffs(temp);
                    sign = -sign;
                }

            }


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }

}