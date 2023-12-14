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
            long resultInt = 0;

            var data = reader.ReadWords();

            string seq = data.First().First();

            Dictionary<string, List<string>> map = new();

            // Common.Log(data.Beautify());

            foreach (var line in data.Skip(1)) {
                map[line[0]] = line.Skip(1).ToList();
            }
            
            bool found = false;
            string currPos = "AAA";
            while (!found) {
                foreach (char step in seq) {
                    int index = step == 'L' ? 0 : 1;
                    currPos = map[currPos][index];
                    resultInt++;
                    if (currPos == "ZZZ") {
                        found = true;
                        break;
                    }
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
            long resultInt = 1;

            var data = reader.ReadWords();

            string seq = data.First().First();

            Dictionary<string, List<string>> map = new();

            foreach (var line in data.Skip(1)) {
                map[line[0]] = line.Skip(1).ToList();
            }
            
            List<string> currPos = map.Keys.Where(s => s[^1] == 'A').ToList();

            foreach (var p in currPos) {
                int n = 0;
                bool found = false;
                var newPos = p;
                int period = 0;
                Dictionary<(int, string), int> repetitionTracker = new();
                
                while (!found) {
                    foreach (var (stepNum, step) in seq.Enumerate()) {
                        int index = step == 'L' ? 0 : 1;
                        newPos = map[newPos][index];
                        n++;
                        if ( newPos[^1] == 'Z' ) {
                            if (repetitionTracker.ContainsKey((stepNum, newPos))) {
                                found = true;
                                period = n - repetitionTracker[(stepNum, newPos)];
                                break;
                            }
                            else {
                                repetitionTracker[(stepNum, newPos)] = n; 
                            }
                        }
                    }
                }
                Common.Log(repetitionTracker.ReprList());
                Common.Log($"Period = {period}");
                // I feel absolutely dirty
                long res = repetitionTracker.Values.First();
                resultInt *= res / Gcd(res, resultInt);
            }




            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }

}