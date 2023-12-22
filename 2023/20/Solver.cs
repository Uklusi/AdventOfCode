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
    using TModule = ValueTuple<char, string[]>;

    public static class Common {
        static Logger logger = new Logger(writeToFile: true);
        public static void Log(params string[] args) => logger.Log(args);
        public static void Log(params object[] args) => logger.Log(args);
        public static void LogEnum<T>(IEnumerable<T> en) => logger.LogEnum(en);
        
        

    }

    public static class Part1 {
        public static string solve(bool useExample) {
            
            Input reader = new Input(useExample: useExample);

            string result = "0";
            long resultInt = 0;

            var data = reader.ReadLines();

            Dictionary<string, TModule> modules = new();
            Dictionary<string, Dictionary<string, bool>> conjStates = new();
            Dictionary<string, bool> flipStates = new();

            foreach (var line in data) {
                var l = line.Split(" -> ");
                string name = l[0];
                var type = name[0];
                if (type != 'b') { // broadcaster
                    name = name[1..];
                }
                var outs = l[1].Split(", ");
                modules[name] = (type, outs);
            }
            foreach (var module in modules) {
                string name = module.Key;
                var (type, outs) = module.Value;
                if (type == '%') { // Flip
                    flipStates[name] = false;
                }
                if (type == '&') {
                    conjStates[name] = new();
                    foreach (var othermodule in modules) {
                        if (othermodule.Value.Item2.Contains(name)) {
                            conjStates[name][othermodule.Key] = false;
                        }
                    }
                }
            }
            foreach (var unknown in modules.Values.SelectMany(m => m.Item2).Except(modules.Keys).ToArray()) {
                modules[unknown] = ('o', Arr<string>());
            }
            Queue<(string from, string to, bool pulseType)> pulses = new();
            Dictionary<bool, long> countPulses = new(){{false, 0}, {true, 0}};

            void ProcessFlip(string name, bool pulseType) {
                if (pulseType) {
                    return;
                }
                flipStates[name] = !flipStates[name];
                foreach (var target in modules[name].Item2) {
                    pulses.Enqueue((name, target, flipStates[name]));
                }
                return;
            }
            void ProcessConj(string from, string name, bool pulseType) {
                conjStates[name][from] = pulseType;
                bool res = conjStates[name].Values.All();
                foreach (var target in modules[name].Item2) {
                    pulses.Enqueue((name, target, !res));
                }
                return;
            }
            foreach (int i in IntRange(1000)) {
                pulses.Enqueue(("button", "broadcaster", false));
                while (pulses.Count != 0) {
                    var pulse = pulses.Dequeue();
                    countPulses[pulse.pulseType]++;
                    var module = modules[pulse.to];
                    if (module.Item1 == 'b') {
                        foreach (var name in module.Item2) {
                            pulses.Enqueue((pulse.to, name, pulse.pulseType));
                        }
                    }
                    else if (module.Item1 == '%') {
                        ProcessFlip(pulse.to, pulse.pulseType);
                    }
                    else if (module.Item1 == '&') {
                        ProcessConj(pulse.from, pulse.to, pulse.pulseType);
                    }
                }
            }

            resultInt = countPulses[false] * countPulses[true];


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

            var data = reader.ReadLines();

            Dictionary<string, TModule> modules = new();
            Dictionary<string, Dictionary<string, bool>> conjStates = new();
            Dictionary<string, bool> flipStates = new();

            foreach (var line in data) {
                var l = line.Split(" -> ");
                string name = l[0];
                var type = name[0];
                if (type != 'b') { // broadcaster
                    name = name[1..];
                }
                var outs = l[1].Split(", ");
                modules[name] = (type, outs);
            }
            foreach (var module in modules) {
                string name = module.Key;
                var (type, outs) = module.Value;
                if (type == '%') { // Flip
                    flipStates[name] = false;
                }
                if (type == '&') {
                    conjStates[name] = new();
                    foreach (var othermodule in modules) {
                        if (othermodule.Value.Item2.Contains(name)) {
                            conjStates[name][othermodule.Key] = false;
                        }
                    }
                }
            }
            foreach (var unknown in modules.Values.SelectMany(m => m.Item2).Except(modules.Keys).ToArray()) {
                modules[unknown] = ('o', Arr<string>());
            }
            Queue<(string from, string to, bool pulseType)> pulses = new();
            // Dictionary<bool, long> countPulses = new(){{false, 0}, {true, 0}};
            List<(string, int)> highPulses = new();

            void ProcessFlip(string name, bool pulseType) {
                if (pulseType) {
                    return;
                }
                flipStates[name] = !flipStates[name];
                foreach (var target in modules[name].Item2) {
                    pulses.Enqueue((name, target, flipStates[name]));
                }
                return;
            }
            void ProcessConj(string from, string name, bool pulseType) {
                conjStates[name][from] = pulseType;
                bool res = conjStates[name].Values.All();
                foreach (var target in modules[name].Item2) {
                    pulses.Enqueue((name, target, !res));
                }
                return;
            }
            foreach (int i in IntRange(1, 30000)) {
                pulses.Enqueue(("button", "broadcaster", false));
                while (pulses.Count != 0) {
                    var pulse = pulses.Dequeue();
                    var module = modules[pulse.to];
                    if (pulse.to == "bn" && pulse.pulseType == true) {
                        highPulses.Add((pulse.from, i));
                    }

                    if (module.Item1 == 'b') {
                        foreach (var name in module.Item2) {
                            pulses.Enqueue((pulse.to, name, pulse.pulseType));
                        }
                    }
                    else if (module.Item1 == '%') {
                        ProcessFlip(pulse.to, pulse.pulseType);
                    }
                    else if (module.Item1 == '&') {
                        ProcessConj(pulse.from, pulse.to, pulse.pulseType);
                    }
                }
            }

            foreach (var moduleName in modules.Keys) {
                var module = modules[moduleName];
                Log($"{moduleName}: {module.Item1} - {module.Item2.ReprList()}");
            }
            Log("");
            foreach (var moduleName in modules.Keys) {
                var module = modules[moduleName];
                var from = modules.Where(m => m.Value.Item2.Contains(moduleName)).Select(m => m.Key);
                Log($"To {moduleName}: {from.ReprList()}");
            }
            Log("");
            highPulses.Sort();
            foreach (var t in highPulses) {
                Log(t);
            }


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }

}