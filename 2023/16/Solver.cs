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
        
        public static DefaultDictionary<(Point, Direction), bool> visited = new();

        public static (Point, Direction) GetState(Movable m){
            return (m.ToPoint(), m.dir);
        }

        public static bool TryMoveBeam(ref Movable current) {
            Point prev = current.ToPoint();
            current.Move();
            return prev != current.ToPoint();
        }

        public static (Movable, Movable?) SplitBeam(char split, Movable current) {
            char currBeamType = '|';
            if (current.dir.IsIn(Arr(new Direction("L"), new Direction("R")))) {
                currBeamType = '-';
            }
            if (currBeamType == split) {
                return (current, null);
            }
            else if (split == '|') {
                return (
                    new Movable(current.X, current.Y, new Direction("U"), current.UpIsNegative),
                    new Movable(current.X, current.Y, new Direction("D"), current.UpIsNegative)
                );
            }
            else {
                return (
                    new Movable(current.X, current.Y, new Direction("L"), current.UpIsNegative),
                    new Movable(current.X, current.Y, new Direction("R"), current.UpIsNegative)
                );
            }
        }

        public static Movable MirrorBeam(char mirror, Movable current) {
            Direction newDir;
            if (mirror == '/') {
                if (current.dir == new Direction('U')) {
                    newDir = new Direction('R');
                }
                else if (current.dir == new Direction('R')) {
                    newDir = new Direction('U');
                }
                else if (current.dir == new Direction('D')) {
                    newDir = new Direction('L');
                }
                else if (current.dir == new Direction('L')) {
                    newDir = new Direction('D');
                }
                else {
                    throw new Exception("WAT");
                }
            }
            else {
                if (current.dir == new Direction('U')) {
                    newDir = new Direction('L');
                }
                else if (current.dir == new Direction('R')) {
                    newDir = new Direction('D');
                }
                else if (current.dir == new Direction('D')) {
                    newDir = new Direction('R');
                }
                else if (current.dir == new Direction('L')) {
                    newDir = new Direction('U');
                }
                else {
                    throw new Exception("WAT");
                }
            }
            return new Movable(current.X, current.Y, newDir, current.UpIsNegative);
        }

        public static void StepBeam(Grid2D<char> map, Movable current) {
            if (visited[GetState(current)]) {
                return;
            }
            visited[GetState(current)] = true;

            char currentChar = map[current];
            if (currentChar == '.') {
                if (TryMoveBeam(ref current)) {
                    StepBeam(map, current);
                }
                return;
            }
            else if (currentChar.IsIn('|', '-')) {
                (Movable, Movable?) splitted = SplitBeam(currentChar, current);
                if (TryMoveBeam(ref splitted.Item1)) {
                    StepBeam(map, splitted.Item1);
                }
                if (splitted.Item2 is null) {
                    return;
                }
                else if (TryMoveBeam(ref splitted.Item2)) {
                    StepBeam(map, splitted.Item2);
                }
                return;
            }
            else if (currentChar.IsIn('\\', '/')) {
                current = MirrorBeam(currentChar, current);
                if (TryMoveBeam(ref current)) {
                    StepBeam(map, current);
                }
                return;
            }

        }

    }

    public static class Part1 {
        public static string solve(bool useExample) {
            
            Input reader = new Input(useExample: useExample);

            string result = "0";
            long resultInt = 0;

            var data = reader.ReadLines();

            Grid2D<char> map = new(data);

            Movable.SetFrame(data);

            StepBeam(map, new Movable(0, 0, new Direction('R'), true));

            resultInt = visited.Keys.Select(t => t.Item1).ToHashSet().Count;


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }



    public static class Part2 {
        public static string solve(bool useExample) {

            Input reader = new Input(useExample: useExample);
            
            string result = "0";
            long resultInt = 0;

            var data = reader.ReadLines();
            
            Grid2D<char> map = new(data);

            Movable.SetFrame(data);
            long count = 0;

            foreach (int x in IntRange(map.Shape.cols)) {
                // Log(x);
                Common.visited = new();
                StepBeam(map, new Movable(x, 0, new Direction('D'), true));
                count = visited.Keys.Select(t => t.Item1).ToHashSet().Count;
                resultInt = Max(resultInt, count);
                Common.visited = new();
                StepBeam(map, new Movable(x, map.Shape.rows - 1, new Direction('U'), true));
                count = visited.Keys.Select(t => t.Item1).ToHashSet().Count;
                resultInt = Max(resultInt, count);
            }
            foreach (int y in IntRange(map.Shape.rows)) {
                Common.visited = new();
                StepBeam(map, new Movable(0, y, new Direction('R'), true));
                count = visited.Keys.Select(t => t.Item1).ToHashSet().Count;
                resultInt = Max(resultInt, count);
                Common.visited = new();
                StepBeam(map, new Movable(map.Shape.cols - 1, y, new Direction('L'), true));
                count = visited.Keys.Select(t => t.Item1).ToHashSet().Count;
                resultInt = Max(resultInt, count);
            }

            


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }

}