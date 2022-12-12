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
        public static string solve(bool useExample) {

            var inputReader = new Input(useExample: useExample);
            var inputData = inputReader.ReadLines();
            string result = "";
            int resultInt = 0;

            foreach (var line in inputData) {
                
                int l = line.Length;
                HashSet<char> firstHalf = line.Where((c, i) => i < l/2 ).ToHashSet();
                HashSet<char> secondHalf = line.Where((c, i) => i >= l/2 ).ToHashSet();

                char element = (
                    from c in firstHalf
                    where secondHalf.Contains(c)
                    select c
                ).ToArray()[0];

                if (char.IsLower(element)) {
                    resultInt += (int)element - (int)'a' + 1;
                }
                else if (char.IsUpper(element)) {
                    resultInt += (int)element - (int)'A' + 27;
                }
                else {
                    throw new ArgumentException("Wrong char");
                }





            }


            result = resultInt.ToString();
            return result;
        }
    }

    public class Part2 {
        public static string solve(bool useExample) {

            var inputReader = new Input(useExample: useExample);
            var inputData = inputReader.ReadLines().Enumerate();
            string result = "";
            int resultInt = 0;
            string[] elfGroup = new string[3];

            foreach (var (i, line) in inputData) {
                
                if (i % 3 != 2) {
                    elfGroup[i % 3] = line;
                    continue;
                }
                elfGroup[2] = line;

                char element = (
                    from c in elfGroup[0]
                    where elfGroup[1].Contains(c) && elfGroup[2].Contains(c)
                    select c
                ).ToArray()[0];

                if (char.IsLower(element)) {
                    resultInt += (int)element - (int)'a' + 1;
                }
                else if (char.IsUpper(element)) {
                    resultInt += (int)element - (int)'A' + 27;
                }
                else {
                    throw new ArgumentException("Wrong char");
                }





            }


            result = resultInt.ToString();
            return result;
        }
    }
}