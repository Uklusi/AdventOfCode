namespace AoC {

    public class Part2 {
        public string solve() {

            var logger = new Logger();
            logger.Open("Part2", writeToFile: false);

            string inputData = (new Input()).Read(useExample: false);
            string result = "";

            string[] elves = inputData.Split("\n\n");
            int[] calories = new int[elves.Length];

            for (var i = 0; i < elves.Length; i++) {
                string elf = elves[i].Trim();
                string[] elfFoods = elf.Split("\n");
                var total = 0;
                foreach (var food in elfFoods) {
                    total += int.Parse(food.Trim());
                }
                calories[i] = total;
            }

            var sumMaxNums = 0;
            
            Array.Sort(calories, (i, j) => j.CompareTo(i));

            sumMaxNums = calories[0] + calories[1] + calories[2];
            result = sumMaxNums.ToString();

            logger.Close();
            return result;
        }
    }
}