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
        
        

    }

    public static class Part1 {
        public static string solve(bool useExample) {
            
            Input reader = new Input(useExample: useExample);

            string result = "";
            int resultInt = 0;

            var data = reader.ReadIntsAndStrings();

            int cycle = 0;
            int register = 1;
            int[] toControl = IntRange(20, 221, 40).ToArray(); 
            foreach( var (ints, ops) in data) {
                if (ops[0] == "noop") {
                    cycle += 1;
                    if (cycle.IsIn(toControl)) {
                        resultInt += cycle * register;
                    }
                }
                else {
                    cycle += 1;
                    if (cycle.IsIn(toControl)) {
                        resultInt += cycle * register;
                    }
                    cycle += 1;
                    if (cycle.IsIn(toControl)) {
                        resultInt += cycle * register;
                    }
                    register += ints[0];
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
            
            string result = "";
            int resultInt = 0;

            var data = reader.ReadInts();
            
            int register = 1;
            int currentLineNum = 0;
            bool isAdding = false;
            foreach (int cycle in IntRange(240)) {
                int n = cycle % 40;
                if (n == 0) {
                    result += "\n";
                }
                if (n.IsIn(IntRange(register - 1, register + 2))) {
                    result += FULL;
                }
                else {
                    result += EMPTY;
                }

                var line = data[currentLineNum];

                if (isAdding) {
                    isAdding = false;
                    register += line[0];
                    currentLineNum++;
                }
                else if (line.Count == 0) {
                    currentLineNum++;
                }
                else {
                    isAdding = true;
                }
            }

            


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }

}