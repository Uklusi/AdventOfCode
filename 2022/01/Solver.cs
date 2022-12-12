namespace AoC {

    public class Part1 {
        public static string solve(bool useExample) {

            var inputReader = new Input(useExample: useExample);

            string inputData = inputReader.Read();
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

            return result;
        }
    }

    public class Part2 {
        public static string solve(bool useExample) {

            var inputReader = new Input(useExample: useExample);
            var inputData = inputReader.Read();
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

            return result;
        }
    }
}