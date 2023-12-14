using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static System.Math;

using AoCUtils;
using AoCUtils.GridUtils;
using AoCUtils.MultidimUtils;
using static AoCUtils.Constants;
using static AoCUtils.Functions;
using System.Collections.Immutable;

namespace AoC {
    using static Common;

    public static class Common {
        static Logger logger = new Logger(writeToFile: false);
        public static void Log(params string[] args) => logger.Log(args);
        public static void Log(params object[] args) => logger.Log(args);
        public static void LogEnum<T>(IEnumerable<T> en) => logger.LogEnum(en);
        
        public static int CountNumberWins(IEnumerable<int> line, bool useExample) {
            int numPossibleWinning = useExample ? 5 : 10;

            return line
                .Skip(1) // skipping the id number
                .Take(numPossibleWinning)
                .Intersect(line.Skip(numPossibleWinning + 1))
                .Count();
        }

    }

    public static class Part1 {
        public static string solve(bool useExample) {
            
            Input reader = new Input(useExample: useExample);

            string result = "0";
            long resultInt = 0;

            foreach (var line in reader.ReadInts()) {
                int numWinning = CountNumberWins(line, useExample: useExample);
                if (numWinning > 0) {
                    resultInt += (int)Pow(2, numWinning - 1);
                }
            }

            


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }



    public static class Part2 {
        public static string solve(bool useExample) {

            Input reader = new Input(useExample: useExample);
            
            string result = "0";
            long resultInt = 0;

            Counter<int> additionalCopies = new();

            foreach (var line in reader.ReadInts()) {
                int id = line.First();
                int numWinning = CountNumberWins(line, useExample: useExample);
                int numAdd = additionalCopies[id] + 1;
                for (int i = 1; i < numWinning + 1; i++) {
                    additionalCopies[id + i] += numAdd;
                }
                resultInt += numAdd;
            }
            


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }

}