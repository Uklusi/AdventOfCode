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
                
                

            }


            result = resultInt.ToString();
            logger.Close();
            return result;
        }
    }
}