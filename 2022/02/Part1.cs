using System.Collections.Generic;

namespace AoC {

    public class Part1 {
        
        int calcScore(string opp, string my, Dictionary<string, string> translator) {
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
        public string solve() {

            var logger = new Logger("Part1", writeToFile: false);

            var inputReader = new Input(useExample: false);
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



            logger.Close();
            return result;
        }
    }
}