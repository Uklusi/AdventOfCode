﻿using System;
using System.IO;
using System.Collections.Generic;

using AoCUtils;
using static AoCUtils.Functions;

namespace AoC {
    public class Logger {
        StreamWriter? logStream = null;

        public Logger(string logFile, bool writeToFile = false) {
            if (writeToFile) {
                logStream = File.CreateText(logFile + ".txt");
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

            data = File.ReadAllText(inputFile).Trim();
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