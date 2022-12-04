using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static System.Math;

using AoCUtils;
using static AoCUtils.Constants;
using static AoCUtils.Functions;

namespace AoC {

    public class Part1 {

        public bool IsContained(int[] range1, int[] range2) {
            return (range1[0] <= range2[0]) && (range2[1] <= range1[1]);
        }

        public string solve() {

            var logger = new Logger("Part1", writeToFile: false);

            var inputReader = new Input(useExample: false);
            var inputData = inputReader.ReadLines();
            string result = "";
            int resultInt = 0;

            foreach (var line in inputData) {
                var rangesArray = line.Split(",");
                int[] limits1 = rangesArray[0].Split("-").Select(int.Parse).ToArray();
                int[] limits2 = rangesArray[1].Split("-").Select(int.Parse).ToArray();

                if (IsContained(limits1, limits2) || IsContained(limits2, limits1)) {
                    resultInt += 1;
                }


            }


            result = resultInt.ToString();
            logger.Close();
            return result;
        }
    }
}