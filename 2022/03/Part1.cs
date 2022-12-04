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
        public string solve() {

            var logger = new Logger("Part1", writeToFile: false);

            var inputReader = new Input(useExample: false);
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
            logger.Close();
            return result;
        }
    }
}