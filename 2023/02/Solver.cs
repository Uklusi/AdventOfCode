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

        public static int gameOk(string game) {

            int targetRed = 12;
            int targetGreen = 13;
            int targetBlue = 14;

            Regex idRegex = new(@"\d+");
            Regex countColorRe = new(@"(\d+) (red|blue|green)");

            int gameId = idRegex.Match(game).Value.ToInt();
            string afterGame = game.Substring(game.IndexOf(':') + 1);
            string[] sessions = afterGame.Split(';');
            foreach (var session in sessions) {
                foreach (Match m in countColorRe.Matches(session)) {
                    string color = m.Groups[2].Value;
                    int quant = m.Groups[1].Value.ToInt();
                    if (
                        (color == "red" && quant > targetRed) || 
                        (color == "green" && quant > targetGreen) || 
                        (color == "blue" && quant > targetBlue)
                    )    
                    {
                        return 0;
                    }
                }
            }
            return gameId;
        }
        public static string solve(bool useExample) {
            
            Input reader = new Input(useExample: useExample);

            string result = "0";
            long resultInt = 0;

            // int targetRed = 12;
            // int targetGreen = 13;
            // int targetBlue = 14;

            // Regex idRegex = new(@"\d+");
            // Regex countColorRe = new(@"(\d+) (red|blue|green)");

            var lines = reader.ReadLines();
            foreach (var line in lines) {
                resultInt += gameOk(line);
            }

            


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }



    public static class Part2 {
        public static int gameOk(string game) {
            int targetRed = 0;
            int targetGreen = 0;
            int targetBlue = 0;

            Regex countColorRe = new(@"(\d+) (red|blue|green)");

            string afterGame = game.Substring(game.IndexOf(':') + 1);
            string[] sessions = afterGame.Split(';');
            foreach (var session in sessions) {
                foreach (Match m in countColorRe.Matches(session)) {
                    string color = m.Groups[2].Value;
                    int quant = m.Groups[1].Value.ToInt();
                    if (color == "red") {
                        targetRed = Max(targetRed, quant);
                    }
                    else if (color == "green") {
                        targetGreen = Max(targetGreen, quant);
                    }
                    else {
                        targetBlue = Max(targetBlue, quant);
                    }
                }
            }
            return targetRed * targetGreen * targetBlue;
        }

        public static string solve(bool useExample) {

            Input reader = new Input(useExample: useExample);
            
            string result = "0";
            long resultInt = 0;

            var lines = reader.ReadLines();
            foreach (var line in lines) {
                resultInt += gameOk(line);
            }
            


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }

}