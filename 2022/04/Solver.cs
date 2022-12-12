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

        public static bool IsContained(int[] range1, int[] range2) {
            return (range1[0] <= range2[0]) && (range2[1] <= range1[1]);
        }

        public static string solve(bool useExample) {

            var inputReader = new Input(useExample: useExample);
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
            return result;
        }
    }

    public class Part2 {

        public static bool IsOverlapping(int[] range1, int[] range2) {
            return (
                (range1[0] <= range2[0] && range2[0] <= range1[1]) ||
                (range2[0] <= range1[0] && range1[0] <= range2[1])
            );
        }

        public static string solve(bool useExample) {

            var inputReader = new Input(useExample: useExample);
            var inputData = inputReader.ReadLines();
            string result = "";
            int resultInt = 0;

            foreach (var line in inputData) {
                var rangesArray = line.Split(",");
                int[] limits1 = rangesArray[0].Split("-").Select(int.Parse).ToArray();
                int[] limits2 = rangesArray[1].Split("-").Select(int.Parse).ToArray();

                if (IsOverlapping(limits1, limits2)) {
                    resultInt += 1;
                }


            }


            result = resultInt.ToString();
            return result;
        }
    }
}