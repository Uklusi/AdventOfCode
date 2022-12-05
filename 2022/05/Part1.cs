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
        Logger logger = new Logger("Part1", writeToFile: false);
        
        public static List<char>[] readCrateDisposition(List<string> cratesScheme) {

            cratesScheme.Reverse();
            int n = (cratesScheme[0].Length + 1) / 4;

            List<char>[] stacks = new List<char>[n];

            foreach ((int i, string line) in cratesScheme.WithIndex()) {
                if (i == 0) {
                    for (int k = 0; k < n; k++) {
                        stacks[k] = new List<char>();
                    }
                }
                else {
                    for (int k = 0; k < n; k++) {
                        char currentCrate = line[4 * k + 1];
                        if (currentCrate != ' ') {
                            stacks[k].Add(currentCrate);
                        }
                    }
                }
            }
            return stacks;
        }

        public string solve() {

            

            var inputReader = new Input(useExample: false);
            var inputData = inputReader.ReadDoubleLines();
            string result = "";


            List<char>[] stacks = readCrateDisposition(inputData[0]);

            foreach (var line in inputData[1]) {
                string[] lineWords = line.Split(" ");
                int numMoved = int.Parse(lineWords[1]);
                int pileFrom = int.Parse(lineWords[3]) -1;
                int pileTo = int.Parse(lineWords[5]) - 1;

                for (int i = 0; i < numMoved; i++) {
                    char moved = stacks[pileFrom][^1];
                    stacks[pileFrom].RemoveAt(stacks[pileFrom].Count - 1);
                    stacks[pileTo].Add(moved);
                }
            }

            result = string.Join("", stacks.Select(l => l.Last()));

            


            logger.Close();
            return result;
        }
    }
}