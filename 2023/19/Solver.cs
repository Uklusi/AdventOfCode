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
    using TInterval = ValueTuple<int, int>;
    using TIntervalState = Dictionary<string, (int, int)>;


    public static class Common {
        static Logger logger = new Logger(writeToFile: false);
        public static void Log(params string[] args) => logger.Log(args);
        public static void Log(params object[] args) => logger.Log(args);
        public static void LogEnum<T>(IEnumerable<T> en) => logger.LogEnum(en);
        
        

    }

    public static class Part1 {
        public static string solve(bool useExample) {
            
            Input reader = new Input(useExample: useExample);

            string result = "0";
            long resultInt = 0;

            var data = reader.ReadParagraphs();
            var rulesRaw = data[0];
            var partsRaw = data[1];

            Dictionary<string, List<(Func<Dictionary<string, int>, bool>, string)>> rules = new();
            List<Dictionary<string, int>> parts = new();

            foreach (var rawRule in rulesRaw) {
                var s1 = rawRule.Split("{");
                string name = s1[0];
                rules[name] = new();
                var s2 = s1[1][..^1].Split(",");
                foreach (var workflow in s2) {
                    if (workflow.Contains(':')) {
                        string cond = workflow.Split(':')[0];
                        string key = cond[0..1];
                        char type = cond[1];
                        int num = cond[2..].ToInt();
                        string label = workflow.Split(':')[1];
                        bool f(Dictionary<string, int> d) {
                            // Log(key, type, num);
                            if (type == '<')
                            { return d[key] < num; }
                            else
                            { return d[key] > num; }
                        }
                        rules[name].Add((f, label));
                    }
                    else {
                        rules[name].Add((d => true, workflow));
                    }
                }

            }

            foreach (var rawPart in partsRaw) {
                parts.Add(new());
                var s = rawPart[1..^1];
                var partData = s.Split(',');
                foreach (var component in partData) {
                    string key = component.Split('=')[0];
                    int val = component.Split('=')[1].ToInt();
                    parts[^1][key] = val;
                }
            }

            foreach (var part in parts) {
                // Log(part.ReprDictInline());
                string currentRule = "in";
                while (!currentRule.IsIn("A", "R")) {
                    // Log(currentRule);
                    foreach (var (f, label) in rules[currentRule]) {
                        if (f(part)) {
                            currentRule = label;
                            break;
                        }
                    }
                }
                // Log(currentRule);
                if (currentRule == "A") {
                    resultInt += part.Values.Sum();
                }
            }


            


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }



    public static class Part2 {


        public static bool AreIntersecting(TInterval a, TInterval b) {
            return a.Item1.IsInInterval(b) || b.Item1.IsInInterval(a);
        }
        public static TInterval? Intersect(TInterval a, TInterval b) {
            if (!AreIntersecting(a, b)) {
                return null;
            }
            return (Max(a.Item1, b.Item1), Min(a.Item2, b.Item2));
        }

        public static List<TInterval> Subtract(TInterval big, TInterval small) {
            List<TInterval> ret = new();
            TInterval? maybeInters = Intersect(big, small);
            if (!maybeInters.HasValue) {
                ret.Add(big);
            }
            else {
                TInterval inters = maybeInters.Value;
                if (inters.Item1 != big.Item1) {
                    ret.Add((big.Item1, inters.Item1 - 1));
                }
                if (inters.Item2 != big.Item2) {
                    ret.Add((inters.Item2 + 1, big.Item2));
                }
            }
            return ret;
        }

        public static string solve(bool useExample) {

            Input reader = new Input(useExample: useExample);
            
            string result = "0";
            long resultInt = 0;

            var data = reader.ReadParagraphs();
            var rulesRaw = data[0];
            var partsRaw = data[1];

            Dictionary<string, List<(string key, TInterval interval, string dest)>> rules = new();

            foreach (var rawRule in rulesRaw) {
                var s1 = rawRule.Split("{");
                string name = s1[0];
                rules[name] = new();
                var s2 = s1[1][..^1].Split(",");
                foreach (var workflow in s2) {
                    if (workflow.Contains(':')) {
                        string cond = workflow.Split(':')[0];
                        string key = cond[0..1];
                        char type = cond[1];
                        int num = cond[2..].ToInt();
                        string label = workflow.Split(':')[1];
                        (int, int) interval = type == '<' ? (1, num - 1) : (num + 1, 4000);
                        rules[name].Add((key, interval, label));
                    }
                    else {
                        rules[name].Add(("x", (1, 4000), workflow));
                    }
                }
                // string firstDest = rules[name][0].dest;
                // if (rules[name].Select(t => t.dest).All(s => s == firstDest)) {
                //     rules[name] = new(){("x", (1, 4000), firstDest)};
                // }
            }

            List<(TIntervalState state, string label)> parts = new();
            TIntervalState startPart = new(){
                {"x", (1, 4000)},
                {"m", (1, 4000)},
                {"a", (1, 4000)},
                {"s", (1, 4000)},
            };
            parts.Add((startPart, "in"));

            while (parts.NotEmpty()) {
                // Log(parts.Select(s => s.state.ReprDictInline() + " " + s.label).ReprList(",\n "));
                List<(TIntervalState, string)> processNext = new();
                foreach (var (state, label) in parts) {
                    if (label == "A") {
                        resultInt += state.Values.Aggregate((long)1, (a, b) => a * (b.Item2 - b.Item1 + 1));
                        continue;
                    }
                    else if (label == "R") {
                        continue;
                    }
                    TIntervalState? toBeProcessed = state;
                    foreach (var (partType, interval, dest) in rules[label]) {
                        if (toBeProcessed is not null) {
                            var i = Intersect(toBeProcessed[partType], interval);
                            if (i.HasValue) {
                                TIntervalState newState = new(toBeProcessed){
                                    [partType] = i.Value
                                };
                                processNext.Add((newState, dest));
                            }
                            var j = Subtract(state[partType], interval);
                            if (j.Empty()) {
                                toBeProcessed = null;
                            }
                            else {
                                toBeProcessed[partType] = j[0];
                            }
                        }
                    }
                }
                parts = processNext;
            }



            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }

}