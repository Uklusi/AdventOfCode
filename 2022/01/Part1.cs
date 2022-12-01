namespace AoC {

    public class Part1 {
        public string solve() {

            var logger = new Logger();
            logger.Open("Part1", writeToFile: false);

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

            var maxNum = 0;
            foreach (var calorie in calories) {
                if (calorie > maxNum) {
                    maxNum = calorie;
                }
            }

            result = maxNum.ToString();

            logger.Close();
            return result;
        }
    }
}