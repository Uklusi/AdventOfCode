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

        
        
        

    }

    public static class Part1 {
        private readonly static char[] CardOrder = {
            '2',
            '3',
            '4',
            '5',
            '6',
            '7',
            '8',
            '9',
            'T',
            'J',
            'Q',
            'K',
            'A'
        };

        public static int CompareCards(char lCard, char rCard) {
            return Array.IndexOf(CardOrder, lCard) - Array.IndexOf(CardOrder, rCard);
        }

        public static int[] CalcHandType(string hand) {
            Counter<char> count = new();
            foreach (char card in hand) {
                count[card] += 1;
            }

            int[] ret = count.Values.ToArray();
            Array.Sort(ret, new Reverse<int>());
            // Log(hand, ret.ToStringArray().JoinString());
            return ret;
        }

        private static Dictionary<string, int[]> HandCache = new();

        public static int CompareHands(string lHand, string rHand) {
            if (!HandCache.ContainsKey(lHand)) {
                HandCache[lHand] = CalcHandType(lHand);
            }
            if (!HandCache.ContainsKey(rHand)) {
                HandCache[rHand] = CalcHandType(rHand);
            }

            int[] lType = HandCache[lHand];
            int[] rType = HandCache[rHand];

            int comp = lType
                .Zip(rType)
                .Select(t => t.First.CompareTo(t.Second))
                .Where(n => n != 0)
                .FirstOrDefault(0);
            
            if (comp != 0) {
                return comp;
            }

            return lHand
                .Zip(rHand)
                .Select(t => CompareCards(t.First, t.Second))
                .Where(n => n != 0)
                .FirstOrDefault(0);
        }

        public static string solve(bool useExample) {
            
            Input reader = new Input(useExample: useExample);

            string result = "0";
            long resultInt = 0;

            var data = reader.ReadTokens();
            
            Dictionary<string, int> bidValues = new();

            foreach (var l in data) {
                bidValues[l[0]] = l[1].ToInt();
            }

            var hands = bidValues.Keys.ToList();

            hands.Sort(CompareHands);

            foreach (var (i, h) in hands.Enumerate()) {
                resultInt += (i + 1) * bidValues[h];
            }



            


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }



    public static class Part2 {
        private readonly static char[] CardOrder = {
            'J',
            '2',
            '3',
            '4',
            '5',
            '6',
            '7',
            '8',
            '9',
            'T',
            'Q',
            'K',
            'A'
        };

        public static int CompareCards(char lCard, char rCard) {
            return Array.IndexOf(CardOrder, lCard).CompareTo(Array.IndexOf(CardOrder, rCard));
        }

        public static int[] CalcHandType(string hand) {
            Counter<char> count = new();
            int jokers = 0;
            foreach (char card in hand) {
                if (card != 'J') {
                    count[card] += 1;
                }
                else {
                    jokers++;
                }
            }

            int[] ret = count.Values.ToArray();
            Array.Sort(ret, new Reverse<int>());
            if (ret.Length == 0) {
                ret = new int[]{0};
            }
            ret[0] += jokers;
            return ret;
        }

        private static Dictionary<string, int[]> HandCache = new();

        public static int CompareHands(string lHand, string rHand) {
            if (!HandCache.ContainsKey(lHand)) {
                HandCache[lHand] = CalcHandType(lHand);
            }
            if (!HandCache.ContainsKey(rHand)) {
                HandCache[rHand] = CalcHandType(rHand);
            }

            int[] lType = HandCache[lHand];
            int[] rType = HandCache[rHand];

            int comp = lType
                .Zip(rType)
                .Select(t => t.First.CompareTo(t.Second))
                .Where(n => n != 0)
                .FirstOrDefault(0);

            // Common.Log(lHand, rHand, lType.ToStringArray().JoinString(), rType.ToStringArray().JoinString(), comp);
            
            if (comp != 0) {
                return comp;
            }

            comp = lHand
                .Zip(rHand)
                .Select(t => CompareCards(t.First, t.Second))
                .Where(n => n != 0)
                .FirstOrDefault(0);

            // Common.Log(lHand, rHand, comp);
            return comp;
        }

        public static string solve(bool useExample) {

            Input reader = new Input(useExample: useExample);
            
            string result = "0";
            long resultInt = 0;

            var data = reader.ReadTokens();
            
            Dictionary<string, int> bidValues = new();

            foreach (var l in data) {
                bidValues[l[0]] = l[1].ToInt();
            }

            var hands = bidValues.Keys.ToList();

            // Common.Log(CompareHands("AT6Q4", "AT6Q4"));
            hands.Sort(CompareHands);

            foreach (var (i, h) in hands.Enumerate()) {
                resultInt += (i + 1) * bidValues[h];
            }

            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }

}