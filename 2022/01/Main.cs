using System;
using System.IO;
using static AoCUtils.Functions;

namespace AoC {
    public class Logger {
        StreamWriter? logStream = null;

        public void Open(string logFile, bool writeToFile = false) {
            if (writeToFile) {
                logStream = File.CreateText(logFile + ".txt");
            }
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
        public string Read(bool useExample=false) {
            
            var inputFile = "input.txt";
            if (useExample) {
                inputFile = "example.txt";
            }
            var inputPath = Path.Combine(".", inputFile);

            return File.ReadAllText(inputPath).Trim();
        }
    }

    class EntryPoint {
        static int Main(string[] args) {

            string ret = "";

            if (args.Length == 0) {
                Console.WriteLine("Please pass the part you want to solve to the program.");
                return 1;
            }
            else if (args[0] == "1") {
                var part1 = new Part1();
                ret = part1.solve();
                File.WriteAllText("output1.txt", ret);
            }
            else if (args[0] == "2") {
                var part2 = new Part2();
                ret = part2.solve();
                File.WriteAllText("output2.txt", ret);
            }
            else {
                Console.WriteLine("Cannot recognise the number you passed to the script, exiting...");
                return 1;
            }

            Console.WriteLine(ret);

            return 0;
        }
    }
}