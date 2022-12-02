using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using AoCUtils;
using static AoCUtils.Constants;
using static AoCUtils.Functions;

namespace AoC {

    public class Part2 {
        public string solve() {

            var logger = new Logger("Part2", writeToFile: false);

            var inputReader = new Input(useExample: false);
            var inputData = inputReader.ReadLines();
            string result = "";

            int totScore = 0;

            foreach (var line in inputData) {
                string[] plays = line.Split(" ");
                string opp = plays[0];
                string my = plays[1];
                totScore += my switch {
                    "Y" => 3,
                    "Z" => 6,
                    _ => 0
                };
                int playScoreTmp = opp switch {
                    "A" => 1,
                    "B" => 2,
                    "C" => 3,
                    _ => throw new Exception()
                };

                int modifier = my switch {
                    "X" => -1,
                    "Z" => 1,
                    _ => 0
                };

                totScore += Mod(playScoreTmp + modifier - 1, 3) + 1;
            }


            result = totScore.ToString();



            logger.Close();
            return result;
        }
    }
}