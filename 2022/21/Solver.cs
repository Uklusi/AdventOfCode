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

    public class MonkeyGroup{
        Dictionary<string, long> values;
        Dictionary<string, List<string>> functions;

        Dictionary<string, List<string>> inverse;

        public MonkeyGroup(List<(List<int> ints, List<string> strs)> data) {
            values = new();
            functions = new();
            inverse = new();
            foreach(var line in data) {
                var name = line.strs[0].TrimEnd(':');
                if (line.ints.NotEmpty()) {
                    values[name] = line.ints.First();
                }
                else {
                    functions[name] = line.strs.GetRange(1, 3);
                    foreach(string to in Arr(line.strs[1], line.strs[3])) {
                        if (!inverse.ContainsKey(to)) {
                            inverse[to] = new();
                        }
                        inverse[to].Add(name);
                    }
                }
            }
        }

        public long Calculate(string monkey) {
            if (values.ContainsKey(monkey)) {
                return values[monkey];
            }
            Func<long, long, long> f = functions[monkey][1] switch {
                "+" => (a, b) => a + b,
                "-" => (a, b) => a - b,
                "*" => (a, b) => a * b,
                "/" => (a, b) => a / b,
                _ => throw new Exception("AAAA")
            };
            values[monkey] = f(Calculate(functions[monkey][0]), Calculate(functions[monkey][2]));
            return values[monkey];
        }

        public long CalculateInverse(string monkey, string whereFrom) {
            if (monkey == "root") {
                if (functions[monkey][0] != whereFrom) {
                    return Calculate(functions[monkey][0]);
                }
                return Calculate(functions[monkey][2]);
            }

            if (inverse[monkey].Count != 1) {
                throw new Exception("AAAA");
            }
            string from = inverse[monkey][0];
            long fromNum = CalculateInverse(from, monkey);
            if (from == "root") {
                return fromNum;
            }

            var fData = functions[from];

            if (fData[0] == monkey) {
                long other = Calculate(fData[2]);
                return (
                    fData[1] switch {
                        "+" => fromNum - other,
                        "-" => fromNum + other,
                        "*" => fromNum / other,
                        "/" => fromNum * other,
                        _ => throw new Exception("AAAA")
                    }
                );
            }
            else {
                long other = Calculate(fData[0]);
                return (
                    fData[1] switch {
                        "+" => fromNum - other,
                        "-" => other - fromNum,
                        "*" => fromNum / other,
                        "/" => other / fromNum,
                        _ => throw new Exception("AAAA")
                    }
                );
            }

        }
    }

    public static class Part1 {
        public static string solve(bool useExample) {
            
            Input reader = new Input(useExample: useExample);

            string result = "";
            long resultInt = 0;

            List<(List<int> ints, List<string> strs)> data = reader.ReadIntsAndStrings();

            MonkeyGroup solver = new(data);

            resultInt = solver.Calculate("root");

            


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

            List<(List<int> ints, List<string> strs)> data = reader.ReadIntsAndStrings();

            MonkeyGroup solver = new(data);

            resultInt = solver.CalculateInverse("humn", "humn");





            


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }

}