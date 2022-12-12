using System.Collections.Generic;
using static AoCUtils.Functions;

namespace AoC {

    public class Part1 {
        
        static int calcScore(string opp, string my, Dictionary<string, string> translator) {
            int score = 0;
            score = my switch {
                "X" => 1,
                "Y" => 2,
                "Z" => 3,
                _ => throw new Exception()
            };

            string newMy = translator[my];

            int calcWin(string a, string b) {
                if (a == b) {
                    return 3;
                }
                
                return (a, b) switch {
                    ("A", "B") => 6,
                    ("B", "C") => 6,
                    ("C", "A") => 6,
                    _          => 0
                };
            }

            score += calcWin(opp, newMy);
            return score;



        }
        public static string solve(bool useExample) {

            var inputReader = new Input(useExample: useExample);
            var inputData = inputReader.ReadLines();
            string result = "";

            int totScore = 0;

            var translator = new Dictionary<string, string>{
                {"X", "A"},
                {"Y", "B"},
                {"Z", "C"}
            };

            foreach (var line in inputData) {
                string[] plays = line.Split(" ");
                string opp = plays[0];
                string my = plays[1];
                totScore += calcScore(opp, my, translator);
            }


            result = totScore.ToString();



            return result;
        }
    }

    public class Part2 {
        public static string solve(bool useExample) {

            var inputReader = new Input(useExample: useExample);
            var inputData = inputReader.ReadLines();
            string result = "";

            int totScore = 0;

            foreach (var line in inputData) {
                string[] plays = line.Split(" ");
                string opp = plays[0];
                string my = plays[1];
                totScore += my switch {
                    "Y" => 3,
                    "Z" => 6,
                    _ => 0
                };
                int playScoreTmp = opp switch {
                    "A" => 1,
                    "B" => 2,
                    "C" => 3,
                    _ => throw new Exception()
                };

                int modifier = my switch {
                    "X" => -1,
                    "Z" => 1,
                    _ => 0
                };

                totScore += Mod(playScoreTmp + modifier - 1, 3) + 1;
            }


            result = totScore.ToString();


            return result;
        }
    }
}