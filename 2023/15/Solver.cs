using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using static System.Math;

using AoCUtils;
using AoCUtils.GridUtils;
using AoCUtils.MultidimUtils;
using static AoCUtils.Constants;
using static AoCUtils.Functions;
using System.Reflection.Emit;
using System.Collections;

namespace AoC {
    using static Common;

    public static class Common {
        static Logger logger = new Logger(writeToFile: false);
        public static void Log(params string[] args) => logger.Log(args);
        public static void Log(params object[] args) => logger.Log(args);
        public static void LogEnum<T>(IEnumerable<T> en) => logger.LogEnum(en);
        
        public static int HashStep(int n, char c) {
            return (n + (int)c) * 17 % 256;
        }

        public static int Hash(string s) {
            return s.Aggregate(0, HashStep);
        }

    }

    public static class Part1 {
        public static string solve(bool useExample) {
            
            Input reader = new Input(useExample: useExample);

            string result = "0";
            long resultInt = 0;

            var data = reader.Read().Split(",");

            foreach (var s in data) {
                resultInt += Hash(s);
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

            var data = reader.Read().Split(",");

            // Curse you OrderedDictionary >:(
            OrderedDictionary[] boxes = new OrderedDictionary[256];
            foreach (int i in IntRange(256)) {
                boxes[i] = new OrderedDictionary();
            }

            foreach (var s in data) {
                string label;
                char op;
                int focal;
                if (char.IsAsciiDigit(s[^1])) {
                    label = s[..^2];
                    op = s[^2];
                    focal = s[^1].ToInt();
                }
                else {
                    label = s[..^1];
                    op = s[^1];
                    focal = 0;
                }
                int box = Hash(label);
                if (op == '=') {
                    boxes[box][label] = focal;
                }
                else {
                    // if label is not in there is no changes
                    boxes[box].Remove(label);
                }
            }
            
            foreach (var (i, box) in boxes.Enumerate()) {
                int j = 0;
                foreach (DictionaryEntry kv in box) {
                    int focal = (int?)kv.Value ?? throw new Exception();
                    resultInt += focal * (j+1) * (i+1);
                    j++;
                }
            }

            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }

}