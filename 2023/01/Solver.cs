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

            var lines = reader.ReadLines();

            foreach (string line in lines) {
                string temp = "";
                foreach (char c in line) {
                    if (char.IsNumber(c)) {
                        temp += c.ToString();
                        break;
                    }
                }
                foreach (char c in line.Reverse()) {
                    if (char.IsNumber(c)) {
                        temp += c.ToString();
                        break;
                    }
                }
                resultInt += temp.ToInt();
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

            Dictionary<string, string> numberMap = new() {
                {"one", "1"},
                {"two", "2"},
                {"three", "3"},
                {"four", "4"},
                {"five", "5"},
                {"six", "6"},
                {"seven", "7"},
                {"eight", "8"},
                {"nine", "9"}
            };

            string numberReString = @"\d|" + string.Join('|', numberMap.Keys);

            Regex numberRe = new(numberReString);
            var lines = reader.ReadLines();
            foreach (string line in lines) {
                string s1 = numberRe.Match(line).Value;
                string s2 = "";
                for (int i = line.Length - 1; i >= 0; i--) {
                    var subLine = line.Substring(i);
                    if (numberRe.IsMatch(subLine)) {
                        s2 = numberRe.Match(subLine).Value;
                        break;
                    }
                }
                if (numberMap.ContainsKey(s1)) {
                    s1 = numberMap[s1];
                }
                if (numberMap.ContainsKey(s2)) {
                    s2 = numberMap[s2];
                }
                resultInt += $"{s1}{s2}".ToInt();
            }


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }

}