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

namespace AoC {
    using static Common;

    public static class Common {
        static Logger logger = new Logger(writeToFile: false);
        public static void Log(params string[] args) => logger.Log(args);
        public static void Log(params object[] args) => logger.Log(args);
        public static void LogEnum<T>(IEnumerable<T> en) => logger.LogEnum(en);
        
        public static int CountChar(char[] s, char target) {
            return s.Where(c => c == target).Count();
        }

        public static Dictionary<(string, string), long> MemoPossibilities = new();
        
        public static long CountPossibilities(char[] row, int[] rowType, int countDebug = 0) {
            (string, string) key = (row.JoinString(""), rowType.ToStringArray().JoinString(","));
            if (MemoPossibilities.ContainsKey(key)) {
                return MemoPossibilities[key];
            }
            int numDefective = CountChar(row, '#');
            int numUncertain = CountChar(row, '?');
            int targetDefective = rowType.Sum();

            while (true) {
                countDebug ++;
                // if (countDebug > 25) {
                //     Log(numDefective, numUncertain, targetDefective);
                //     LogEnum(row);
                //     LogEnum(rowType);
                // }
                // if (countDebug > 30) {
                //     throw new Exception();
                // }

                if (numDefective > targetDefective || numDefective + numUncertain < targetDefective) {
                    MemoPossibilities[key] = 0;
                    return 0;
                }
                if (row.Length == 0) {
                    MemoPossibilities[key] = 1;
                    return 1;
                }

                // Log(i, row[i]);
                
                if (row[0] == '.') {
                    MemoPossibilities[key] = CountPossibilities(row[1..], rowType, countDebug:countDebug);
                    return MemoPossibilities[key];
                }
                else if (row[0] == '#') {
                    int currSectType = rowType[0];
                    foreach (int j in IntRange(currSectType)) {
                        if (row[j] == '.') {
                            return 0;
                        }
                    }
                    if (currSectType == row.Length) {
                        return (rowType[1..].Sum() == 0).ToInt();
                    }
                    if (row[currSectType] == '#') {
                        return 0;
                    }
                    MemoPossibilities[key] = CountPossibilities(row[(currSectType + 1)..], rowType[1..], countDebug:countDebug);
                    return MemoPossibilities[key];
                }
                else if (row[0] == '?') {
                    // LogEnum(row);
                    // LogEnum(rowType);
                    var newRow = row.ToArray();
                    newRow[0] = '#';
                    long possibilitiesDefective = CountPossibilities(newRow, rowType, countDebug:countDebug);
                    newRow[0] = '.';
                    MemoPossibilities[key] = possibilitiesDefective + CountPossibilities(newRow, rowType, countDebug:countDebug);
                    return MemoPossibilities[key];
                }
            }
        }

    }

    public static class Part1 {
        public static string solve(bool useExample) {
            
            Input reader = new Input(useExample: useExample);

            string result = "0";
            long resultInt = 0;

            var data = reader.ReadTokens();

            foreach (var line in data) {
                var row = line[0].ToCharArray();
                int[] rowType = line[1]
                    .Split(',')
                    .Select(s => s.ToInt())
                    .ToArray();
                resultInt += CountPossibilities(row, rowType);
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

            var data = reader.ReadTokens();

            foreach (var line in data) {
                var row = line[0];
                char[] trueRow = Repeat(row, 5)
                    .JoinString("?")
                    .ToCharArray();
                var rowType = line[1]
                    .Split(',')
                    .Select(s => s.ToInt());
                int[] trueRowType = Repeat(rowType, 5)
                    .Flatten()
                    .ToArray();
                resultInt += CountPossibilities(trueRow, trueRowType);
            }


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }

}