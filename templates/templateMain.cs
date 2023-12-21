using System;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using AoCUtils;
using AoCUtils.GridUtils;
using static AoCUtils.Constants;
using static AoCUtils.Functions;

namespace AoC {

    public class StopwatchUtil : Stopwatch {
        public string ElapsedString {
            get {
                long num = ElapsedMilliseconds;
                if (num < 1000) {
                    return $"{num} ms";
                }
                if (num < 60 * 1000) {
                    return $"{num / 1000}.{num % 1e3:000} s";
                }
                num /= 1000;
                if (num < 60 * 60) {
                    return $"{num/60} m {num%60} s";
                }
                num /= 60;
                return $"{num/60} h {num%60} m";
            }
        }
    }

    public class Logger {
        StreamWriter? logStream = null;

        public Logger(string logFile = "log", bool writeToFile = false) {
            if (writeToFile) {
                logStream = File.CreateText(logFile + ".log");
            }
        }

        ~Logger() {
            this.Close();
        }

        public void Log(params string[] args) {
            string output = string.Join(" ", args);
            if (logStream != null) {
                logStream.WriteLine(output);
                logStream.Flush();
            }
            else {
                Console.WriteLine(output);
            }
            return;
        }
        public void Log(params object[] args) {
            Log(args.ToStringArray());
        }

        public void LogEnum<T>(IEnumerable<T> en) {
            Log(en.ToStringArray());
        }

        public void Close() {
            if (logStream != null) {
                logStream.Flush();
                logStream.Close();
            }
        }
    }
    
    public class Input {

        private readonly string data = string.Empty;

        /// <summary>
        /// Helper class created in order to read input.
        /// </summary>
        public Input(bool useExample=false) {

            string inputFile = string.Empty;

            if (useExample) {
                inputFile = "example.txt";
            }
            else {
                inputFile = "input.txt";
            }

            data = File.ReadAllText(inputFile).Replace("\r\n", "\n").TrimEnd();
        }

        /// <summary>
        /// The data in its raw form. Loses all spaces at the end.
        /// </summary>
        public string Read() {
            return data;
        }

        /// <summary>
        /// The data, digested in a list of lines
        /// </summary>
        public List<string> ReadLines() {
            return data.Split('\n').ToList();
        }

        /// <summary>
        /// The data, us a list of paragraphs.
        /// A paragraph is a list of lines, separated by an empty line
        /// </summary>
        public List<List<string>> ReadParagraphs() {
            string[] blocks = data.Split("\n\n");
            List<List<string>> table = new();
            
            for (int i = 0; i < blocks.Length; i++) {
                var block = blocks[i];
                table.Add(block.Split("\n").ToList());
            }
            
            return table;
        }

        /// <summary>
        /// Given a map (a list of lines),
        /// returns the same map replacing '.' with ' ' and '#' with '█'
        /// </summary>
        public List<string> ProcessMap(List<string> AoCMap) {
            return AoCMap.Select(s => s.Replace('.', EMPTY).Replace('#', FULL)).ToList();
        }
        
        /// <summary>
        /// Reads the data as a processed map (check ProcessMap)
        /// </summary>
        public List<string> ReadProcessedMap() {
            return ProcessMap(ReadLines());
        }

        /// <summary>
        /// Reads the data as a list of lines, where each line is a list of tokens
        /// (a token is one or more characters separated by spaces)
        /// </summary>
        public List<List<string>> ReadTokens() {
            return data.Split('\n').Select(line => line.Split(' ')).ToNestedList();
        }

        public List<List<string>> ReadWords(string additionalTokens="", bool skipBlanks=true) {
            Regex re = new Regex(@"[a-zA-Z0-9" + additionalTokens + @"]+");
            IEnumerable<string> nData = data
                .Split("\n");
            if (skipBlanks) {
                nData = nData.Where(l => l != "");
            }
            return nData
                .Select(
                    line => re
                        .Matches(line)
                        .Select(m => m.Value)
                )
                .ToNestedList();
        }

        public List<List<int>> Ints(IEnumerable<string> linesWithInts) {
            Regex re = new Regex(@"(\b|-)\d+\b", RegexOptions.Compiled);
            return linesWithInts
                .Select(
                    line => re.Matches(line)
                        .Select(match => match.Value.ToInt())
                ).ToNestedList();
        }
        /// <summary>
        /// Reads the data as a list of lines, where each line is a list of all the integers
        /// appearing in that line of the data (when separated by word boundaries)
        /// </summary>
        public List<List<int>> ReadInts() => Ints(ReadLines());

        /// <summary>
        /// Reads the data as a list of lines, where each line is a couple of list,
        /// one containing all the integers appearing in that line of the data
        /// (all tokens that match exactly the Regex "-?\d+")
        /// and the other containing all the non-integer tokens
        /// </summary>
        public List<(List<int> ints, List<string> strs)> ReadIntsAndStrings() {
            Regex re = new Regex(@"^-?\d+$", RegexOptions.Compiled);
            return (
                from line in ReadTokens()
                select (
                    (
                        from token in line
                        where re.IsMatch(token)
                        select token.ToInt()
                    ).ToList(),
                    (
                        from token in line
                        where ! re.IsMatch(token)
                        select token
                    ).ToList()
                )
            ).ToList();
        }
        
        /// <summary>
        /// Reads the data as a list of tuples (Point p, char item)
        /// </summary>
        public List<(Point p, char item)> ReadMapAsPoints() {
            return ReadLines().Enumerate2D().ToList();
        }
    }

    class EntryPoint {
        static int Main(string[] args) {

            string ret = "";
            bool useExample = false;
            if (args.Length == 2 && args[1] == "--example") {
                useExample = true;
            }

            StopwatchUtil stopwatch = new();

            stopwatch.Start();

            if (args.Length == 0) {
                Console.WriteLine("Please pass the part you want to solve to the program.");
                return 1;
            }
            else if (args[0] == "1") {
                
                ret = Part1.solve(useExample);
                File.WriteAllText("output1.txt", ret);
            }
            else if (args[0] == "2") {
                ret = Part2.solve(useExample);
                File.WriteAllText("output2.txt", ret);
            }
            else {
                Console.WriteLine("Cannot recognise the number you passed to the script, exiting...");
                return 1;
            }

            stopwatch.Stop();
            string elapsedTime = $"Runtime: {stopwatch.ElapsedString}";
            Console.WriteLine(elapsedTime);

            Console.WriteLine(ret);

            return 0;
        }
    }
}