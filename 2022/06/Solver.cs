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

    public static class Common {
        
    }

    public static class Part1 {

        public static int GetSolution(string inputData, int space) {
            for (int i = 0; i < inputData.Length; i++) {
                string toCheck = inputData[i..(i+space)];
                if (toCheck.ToHashSet().Count == space) {
                    return i + space;
                }
            }
            throw new Exception("This shouldn't happen");
        }

        static Logger logger = new Logger("Part1", writeToFile: false);

        public static string solve(bool useExample) {
            
            var inputReader = new Input(useExample: useExample);
            var inputData = inputReader.Read();
            string result = "";
            int resultInt = GetSolution(inputData, 4);

            

            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            logger.Close();
            return result;
        }
    }



    public static class Part2 {

        static Logger logger = new Logger("Part2", writeToFile: false);

        public static string solve(bool useExample) {
            
            var inputReader = new Input(useExample: useExample);
            var inputData = inputReader.Read();
            string result = "";
            int resultInt = Part1.GetSolution(inputData, 14);

            


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            logger.Close();
            return result;
        }
    }

}