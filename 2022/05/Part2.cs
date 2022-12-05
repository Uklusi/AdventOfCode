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
            var inputData = inputReader.ReadDoubleLines();
            string result = "";

            List<char>[] stacks = Part1.readCrateDisposition(inputData[0]);

            foreach (var line in inputData[1]) {
                string[] lineWords = line.Split(" ");
                int numMoved = int.Parse(lineWords[1]);
                int pileFrom = int.Parse(lineWords[3]) -1;
                int pileTo = int.Parse(lineWords[5]) - 1;

                
                List<char> moved = stacks[pileFrom].GetRange(stacks[pileFrom].Count - numMoved, numMoved);
                stacks[pileFrom].RemoveRange(stacks[pileFrom].Count - numMoved, numMoved);
                stacks[pileTo].AddRange(moved);
            }

            result = string.Join("", stacks.Select(l => l.Last()));


            logger.Close();
            return result;
        }
    }
}