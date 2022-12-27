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
        
        private static Dictionary<char, int> _fromSnafu = new() {
            {'=', -2},
            {'-', -1},
            {'0',  0},
            {'1',  1},
            {'2',  2}
        };
        private static Dictionary<int, char> _toSnafu = new() {
            {-2, '='},
            {-1, '-'},
            { 0, '0'},
            { 1, '1'},
            { 2, '2'}
        };

        public static long FromSnafu(string snafu) {
            long n = 0;
            foreach (char snafuDigit in snafu) {
                n *= 5;
                n += _fromSnafu[snafuDigit];
            }
            return n;
        }
        private static int SnafuMod(long n) {
            int i = Mod(n, 5);
            if (i > 2) {
                return i - 5;
            }
            return i;
        }

        public static string ToSnafu(long n) {
            string s = "";
            while (n > 0) {
                int i = SnafuMod(n);
                n -= i;
                s += _toSnafu[i];
                n /= 5;
            }
            return s.Reverse().JoinString();
        }

    }

    public static class Part1 {
        public static string solve(bool useExample) {
            
            Input reader = new Input(useExample: useExample);

            string result = "0";
            long resultInt = 0;

            var lines = reader.ReadLines();
            foreach (string line in lines) {
                resultInt += FromSnafu(line);
            }

            result = ToSnafu(resultInt);

            


            
            return result;
        }
    }



    public static class Part2 {
        public static string solve(bool useExample) {

            Input reader = new Input(useExample: useExample);
            
            string result = "0";
            long resultInt = 0;

            


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }

}