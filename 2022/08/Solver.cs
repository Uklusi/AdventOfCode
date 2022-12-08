using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static System.Math;

using AoCUtils;
using AoCUtils.GridUtils;
using static AoCUtils.Constants;
using static AoCUtils.Functions;

namespace AoC {

    public static class Common {

        static Logger logger = new("common");

        public static HashSet<Point> findUnblocked(IEnumerable<IEnumerable<char>> image) {
            char[][] imageArr = image.Select(l => l.ToArray()).ToArray();
            HashSet<Point> set = new();
            int doTheThing(int x, int y, char c, int highest, HashSet<Point> set) {
                int n = int.Parse(c.ToString());
                if (n > highest) {
                    highest = n;

                    set.Add(new Point(x, y));
                }
                return highest;
            }
            foreach ((int y, IEnumerable<char> line) in image.Enumerate()) {
                int highest = -1;
                foreach( (int x, char c) in line.Enumerate() ){
                    highest = doTheThing(x, y, c, highest, set);
                }
            }
            foreach ((int y, IEnumerable<char> line) in image.Enumerate()) {
                int highest = -1;
                foreach( (int x, char c) in line.Enumerate().Reverse() ){
                    highest = doTheThing(x, y, c, highest, set);
                }
            }
            for (int x = 0; x < imageArr[0].Length; x++) {
                int highest = -1;
                for(int y = 0; y < imageArr.Length; y++){
                    char c = imageArr[y][x];
                    highest = doTheThing(x, y, c, highest, set);
                }
            }
            for (int x = 0; x < imageArr[0].Length; x++) {
                int highest = -1;
                for(int y = imageArr.Length - 1; y >= 0; y--){
                    char c = imageArr[y][x];
                    highest = doTheThing(x, y, c, highest, set);
                }
            }
            return set;
        }

        public static int calculateScore(int x0, int y0, List<string> map) {
            int posTree = map[y0][x0].ToString().ToInt();
            int count;
            int res = 1;
            count = 0;
            for (int y = y0 + 1; y < map.Count; y++) {
                int n = map[y][x0].ToString().ToInt();
                count++;
                if (n>=posTree) {
                    break;
                }
            }
            res *= count;
            count = 0;
            for (int y = y0 - 1 ; y >= 0; y--) {
                int n = map[y][x0].ToString().ToInt();
                count++;
                if (n>=posTree) {
                    break;
                }
            }
            res *= count;
            count = 0;
            for (int x = x0 + 1; x < map[0].Length; x++) {
                int n = map[y0][x].ToString().ToInt();
                count++;
                if (n>=posTree) {
                    break;
                }
            }
            res *= count;
            count = 0;
            for (int x = x0 - 1; x >= 0; x--) {
                int n = map[y0][x].ToString().ToInt();
                count++;
                if (n>=posTree) {
                    break;
                }
            }
            res *= count;
            return res;
        }
        
    }

    public static class Part1 {

        public static Logger logger = new Logger("Part1", writeToFile: false);

        public static string solve(bool useExample) {
            
            var inputReader = new Input(useExample: useExample);

            string result = "";
            int resultInt = 0;

            var data = inputReader.ReadLines();

            HashSet<Point> set = Common.findUnblocked(data);

            resultInt = set.Count;



            


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            logger.Close();
            return result;
        }
    }



    public static class Part2 {

        static Logger logger = new Logger("Part2", writeToFile: false);

        public static string solve(bool useExample) {

            var inputReader = new Input(useExample: useExample);
            
            string result = "";
            int resultInt = 0;
            var data =inputReader.ReadLines();

            for(int y = 0; y < data.Count; y++) {
                for (int x = 0; x< data[0].Count(); x++) {
                    int res = Common.calculateScore(x, y, data);
                    // logger.Log($"({x}, {y}) {res}");
                    resultInt = Max(resultInt, res);
                }
            }

            


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            logger.Close();
            return result;
        }
    }

}