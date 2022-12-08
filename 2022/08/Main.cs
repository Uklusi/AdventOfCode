using System;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;

using static AoCUtils.Constants;

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

        public Logger(string logFile, bool writeToFile = false) {
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
            }
            else {
                Console.WriteLine(output);
            }
            return;
        }

        public void Close() {
            if (logStream != null) {
                logStream.Close();
            }
        }
    }
    
    public class Input {

        private readonly string data = string.Empty;

        public Input(bool useExample=false) {

            string inputFile = string.Empty;

            if (useExample) {
                inputFile = "example.txt";
            }
            else {
                inputFile = "input.txt";
            }

            data = File.ReadAllText(inputFile).TrimEnd();
        }

        public string Read() {
            return data;
        }

        public List<string> ReadLines() {
            return new List<string>(data.Split('\n'));
        }

        public List<List<string>> ReadDoubleLines() {
            string[] blocks = data.Split("\n\n");
            List<List<string>> table = new List<List<string>>();
            
            for (int i = 0; i < blocks.Length; i++) {
                var block = blocks[i];
                table.Add(new List<string>(block.Split("\n")));
            }
            
            return table;
        }

        public List<string> ProcessMap(List<string> AoCMap) {
            return AoCMap.Select(s => s.Replace('.', Empty).Replace('#', Full)).ToList();
        }
        
        public List<string> ReadProcessedMap() {
            return ProcessMap(ReadLines());
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