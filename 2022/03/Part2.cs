using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static System.Math;

using AoCUtils;
using static AoCUtils.Constants;
using static AoCUtils.Functions;

namespace AoC {

    public class Part2 {
        public string solve() {

            var logger = new Logger("Part2", writeToFile: false);

            var inputReader = new Input(useExample: false);
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
            logger.Close();
            return result;
        }
    }
}