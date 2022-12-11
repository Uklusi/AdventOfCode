using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Diagnostics.CodeAnalysis;
using static System.Math;

using AoCUtils;
using AoCUtils.GridUtils;
using static AoCUtils.Constants;
using static AoCUtils.Functions;

namespace AoC {
    using static Common;

    public static class Common {
        static Logger logger = new Logger(writeToFile: false);
        public static void Log(params string[] args) => logger.Log(args);
        
        

    }

    public class Monkey {
        public List<long> items;
        Func<long, long> operation;
        Func<long, int> test;

        public int modulo;

        public Monkey(List<string> monkeyData) {
            items = new();
            items = monkeyData[1].Split(": ")[1].Split(", ").Select(s => s.ToLong()).ToList();
            string[] opString = monkeyData[2].Split(" = ")[1].Split();
            operation = (opString[1], opString[2]) switch {
                ("*", "old") => (n => n*n),
                ("+", "old") => (n => n+n),
                ("*", var s) => (n => n * s.ToInt()),
                ("+", var s) => (n => n + s.ToInt()),
                _ => throw new ArgumentException()
            };
            int d = monkeyData[3].Split()[^1].ToInt();
            int toTrue = monkeyData[4].Split()[^1].ToInt();
            int toFalse = monkeyData[5].Split()[^1].ToInt();
            int testf(long n) {
                if (n % d == 0) {
                    return toTrue;
                }
                return toFalse;
            }
            test = testf;
            modulo = d;
        }

        public List<(long, int)> ProcessTurn() {
            List<(long, int)> rets = new();
            foreach (long item in items) {
                long newItem = operation(item) / 3;
                int newMonkey = test(newItem);
                rets.Add((newItem, newMonkey));
            }
            items.Clear();
            return rets;
        }
        public List<(long, int)> ProcessTurnExploding(long modulo) {
            List<(long, int)> rets = new();
            foreach (long item in items) {
                long newItem = operation(item) % modulo;
                if (newItem < 0) {
                    Log(newItem.ToString());
                }
                int newMonkey = test(newItem);
                rets.Add((newItem, newMonkey));
            }
            items.Clear();
            return rets;
        }
    
    }

    public static class Part1 {
        public static string solve(bool useExample) {
            
            Input reader = new Input(useExample: useExample);

            string result = "";
            int resultInt = 0;

            var data = reader.ReadParagraphs();

            List<Monkey> monkeys = new();
            foreach (var monkeyData in data) {
                monkeys.Add(new Monkey(monkeyData));
            }
            int[] handled = new int[monkeys.Count];

            int numrounds = 20;
            // int numrounds = 1;
            foreach (int _ in IntRange(numrounds)) {
                foreach (int i in IntRange(monkeys.Count)) {
                    Monkey monkey = monkeys[i];
                    handled[i] += monkey.items.Count;
                    var processed = monkey.ProcessTurn();
                    foreach (var (newItem, newMonkey) in processed) {
                        monkeys[newMonkey].items.Add(newItem);
                    }
                    
                }
            }
            Array.Sort(handled, new Reverse<int>());
            resultInt = handled[0] * handled[1];

            


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }



    public static class Part2 {
        public static string solve(bool useExample) {

            Input reader = new Input(useExample: useExample);
            
            string result = "";
            long resultInt = 0;

            var data = reader.ReadParagraphs();

            long modulo = 1;

            List<Monkey> monkeys = new();
            foreach (var monkeyData in data) {
                Monkey monkey = new Monkey(monkeyData);
                monkeys.Add(monkey);
                modulo *= monkey.modulo;
            }
            long[] handled = new long[monkeys.Count];

            int numrounds = 10000;
            // int numrounds = 1000;
            foreach (int _ in IntRange(numrounds)) {
                foreach (int i in IntRange(monkeys.Count)) {
                    Monkey monkey = monkeys[i];
                    handled[i] += monkey.items.Count;
                    var processed = monkey.ProcessTurnExploding(modulo);
                    foreach (var (newItem, newMonkey) in processed) {
                        monkeys[newMonkey].items.Add(newItem);
                    }
                    
                }
            }
            Log(handled.Select(i => i.ToString()).ToArray());
            Array.Sort(handled, new Reverse<long>());
            resultInt = handled[0] * handled[1];
            


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }

}