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

        public static List<int>[] CreateArray(int size) {
            List<int>[] ret = new List<int>[size];
            foreach (int i in IntRange(size)) {
                ret[i] = new();
            }
            return ret;
        }
        
        public static (
            List<int>[] cubeByRows,
            List<int>[] cubeByCols,
            List<int>[] roundByRows,
            List<int>[] roundByCols
        ) GetRocks(List<string> data, (int cols, int rows) size) {
            List<int>[] cubeByRows = CreateArray(size.rows);
            List<int>[] cubeByCols = CreateArray(size.cols);
            List<int>[] roundByRows = CreateArray(size.rows);
            List<int>[] roundByCols = CreateArray(size.cols);

            foreach (var (p, c) in data.Enumerate2D()) {
                if (c == '#') {
                    cubeByRows[p.Y].Add(p.X);
                    cubeByCols[p.X].Add(p.Y);
                }
                else if (c == 'O') {
                    roundByRows[p.Y].Add(p.X);
                    roundByCols[p.X].Add(p.Y);
                }
            }

            return (cubeByRows, cubeByCols, roundByRows, roundByCols);
        }

        
        public static List<int>[] ReverseData(List<int>[] data, int innerSize) {
            var ret = data.Copy();
            foreach (int i in IntRange(ret.Length)) {
                ret[i] = ret[i].Select(n => innerSize - n - 1).ToList();
                ret[i].Reverse()
                ;
            }
            return ret;
        }

        public static (
            List<int>[] thisDir,
            List<int>[] otherDir
        ) Move(List<int>[] cubes, List<int>[] rounds, int thisSize, int otherSize, bool reverse = false) {
            
            var thisDir = CreateArray(thisSize);
            var otherDir = CreateArray(otherSize);
            if (reverse) {
                cubes = ReverseData(cubes, otherSize);
                rounds = ReverseData(rounds, otherSize);
            }
            foreach (int i in IntRange(thisSize)) {
                int lastOccupied = -1;
                foreach (int roundPos in rounds[i]) {
                    lastOccupied = Max(
                        lastOccupied,
                        cubes[i]
                            .Where(p => p < roundPos)
                            .LastOrDefault(-1)
                    );
                    lastOccupied++;
                    thisDir[i].Add(lastOccupied);
                    otherDir[lastOccupied].Add(i);
                }
            }
            if (reverse) {
                thisDir = ReverseData(thisDir, otherSize);
                otherDir = otherDir.Reverse().ToArray();
            }
            return (thisDir, otherDir);
        }

        public static (
            List<int>[] roundByRows,
            List<int>[] roundByCols
        ) MoveVertical(
            List<int>[] cubeByCols,
            List<int>[] roundByCols,
            (int cols, int rows) size,
            bool down = false
        ) {
            (
                List<int>[] byCols,
                List<int>[] byRows
            ) t = Move(cubeByCols, roundByCols, size.cols, size.rows, down);
            return (t.byRows, t.byCols);
        }

        public static (
            List<int>[] roundByRows,
            List<int>[] roundByCols
        ) MoveHorizontal(
            List<int>[] cubeByRows,
            List<int>[] roundByRows,
            (int cols, int rows) size,
            bool right = false
        ) {
            (
                List<int>[] byRows,
                List<int>[] byCols
            ) t = Move(cubeByRows, roundByRows, size.rows, size.cols, right);
            return t;
        }

        public static void LogState(List<int>[] byRows) {
            Log(
                byRows.Select(
                    r => IntRange(r.LastOrDefault(-1) + 1)
                        .Select(n => n.IsIn(r) ? "O" : "·")
                        .JoinString()
                ).JoinString("\n")
            );
        }
    }

    public static class Part1 {
        public static string solve(bool useExample) {
            
            Input reader = new Input(useExample: useExample);

            string result = "0";
            long resultInt = 0;

            var data = reader.ReadLines();
            int rows = data.Count;
            int cols = data[0].Length;

            (List<int>[] byRows, List<int>[] byCols) cubes;
            (List<int>[] byRows, List<int>[] byCols) rounds;

            (cubes.byRows, cubes.byCols, rounds.byRows, rounds.byCols) = GetRocks(data, (cols, rows));


            rounds = MoveVertical(cubes.byCols, rounds.byCols, (cols, rows));
            // LogState(rounds.roundByRows);
            // Log("\n-------\n");
            // LogState(rounds.roundByCols);
            foreach (var (i, row) in rounds.byRows.Enumerate()) {
                resultInt += (rows - i) * row.Count;
            }

            


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }



    public static class Part2 {
        public static string EncodeState((List<int>[], List<int>[]) rounds) {
            return rounds.Item1.Select(l => l.JoinString(',')).JoinString('|');
        }
        public static string solve(bool useExample) {

            Input reader = new Input(useExample: useExample);
            
            string result = "0";
            long resultInt = 0;

            var data = reader.ReadLines();
            int rows = data.Count;
            int cols = data[0].Length;
            (int, int) size = (cols, rows);
            int numCycles = 1000000000;
            Dictionary<string, int> count = new();

            (List<int>[] byRows, List<int>[] byCols) cubes;
            (List<int>[] byRows, List<int>[] byCols) rounds;

            (cubes.byRows, cubes.byCols, rounds.byRows, rounds.byCols) = GetRocks(data, size);

            count[EncodeState(rounds)] = 0;
            int numRetrace = -1;
            foreach (int i in IntRange(1, numCycles + 1)) {
                rounds = MoveVertical(cubes.byCols, rounds.byCols, size, down: false);
                rounds = MoveHorizontal(cubes.byRows, rounds.byRows, size, right: false);
                rounds = MoveVertical(cubes.byCols, rounds.byCols, size, down: true);
                rounds = MoveHorizontal(cubes.byRows, rounds.byRows, size, right: true);
                string state = EncodeState(rounds);
                if (count.ContainsKey(state)) {
                    int period = i - count[state];
                    numRetrace = Mod(numCycles - i, period);
                    break;
                }
                else {
                    count[state] = i;
                }
            }
            foreach (int i in IntRange(numRetrace)) {
                rounds = MoveVertical(cubes.byCols, rounds.byCols, size, down: false);
                rounds = MoveHorizontal(cubes.byRows, rounds.byRows, size, right: false);
                rounds = MoveVertical(cubes.byCols, rounds.byCols, size, down: true);
                rounds = MoveHorizontal(cubes.byRows, rounds.byRows, size, right: true);
            }
            foreach (var (i, row) in rounds.byRows.Enumerate()) {
                resultInt += (rows - i) * row.Count;
            }

            


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }

}