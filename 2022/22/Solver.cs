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
    using ScaledPoint = Point;
    using MapPoint = Point;
    using CubeNeigh = ValueTuple<Point, int>;

    public static class Common {
        static Logger logger = new Logger(writeToFile: true);
        public static void Log(params string[] args) => logger.Log(args);
        public static void Log(params object[] args) => logger.Log(args);
        public static void LogEnum<T>(IEnumerable<T> en) => logger.LogEnum(en);

        public static bool Parallel(Direction d1, Direction d2) {
            return (d1 == d2 || d1 == -d2);
        }

        public static Movable MoveUntilNonSpace(Grid2D<char> map, Movable pos) {
            while (map[pos] == ' ') {
                pos.Move();
            }
            return pos;
        }

        public static Movable MoveCyclic(Grid2D<char> map, Movable pos, int steps) {
            Direction d = pos.dir;
            
            foreach(int i in IntRange(steps)) {
                Movable oldPos = pos.Copy();
                pos.Move();
                char c;
                try {
                    c = map[pos];
                }
                catch {
                    c = ' ';
                }
                if (c == '#') {
                    return oldPos;
                }
                if (c == ' ') {
                    Movable newPos = d.ToInt() switch {
                        0 => new Movable(0, pos.Y, d),
                        1 => new Movable(pos.X, 0, d),
                        2 => new Movable(map.Shape.cols - 1, pos.Y, d),
                        3 => new Movable(pos.X, map.Shape.rows - 1, d),
                        _ => throw InPanic()
                    };
                    newPos = MoveUntilNonSpace(map, newPos);
                    if (map[newPos] == '#') {
                        return oldPos;
                    }
                    pos = newPos;
                }
            }

            return pos;
        }

    }

    public static class Part1 {
        public static string solve(bool useExample) {
            
            Input reader = new Input(useExample: useExample);

            string result = "0";
            long resultInt = 0;

            var data1 = reader.ReadParagraphs();

            var map1 = data1[0];
            var directions1 = data1[1][0];

            Regex numInfo = new Regex(@"\d+");
            Regex dirInfo = new Regex(@"L|R");

            List<int> nums = numInfo.Matches(directions1)
                .Select(m => m.Value.ToInt())
                .ToList();
            List<string> dirs = dirInfo.Matches(directions1)
                .Select(m => m.Value)
                .ToList();
            
            int maxH = map1.Select(s => s.Length).Max();
            Grid2D<char> map = new(
                map1.Select(s => s.PadRight(maxH))
            );

            Movable p = new Movable();
            p = MoveUntilNonSpace(map, p);
            while (map[p] == '#'){
                p.Move();
            }

            foreach ((int steps, string dir) in nums.Zip(dirs)) {
                p = MoveCyclic(map, p, steps);
                p.Turn(dir);
                p.Turn(dir);
                p.Turn(dir);
            }
            p = MoveCyclic(map, p, nums[^1]);

            resultInt = 1000 * (p.Y + 1) + 4 * (p.X + 1) + Mod(-p.dir.ToInt(), 4);
            

            


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }

    }

    public class CubeFace {
        public Grid2D<char> faceData;
        public Dictionary<Direction, (CubeFace face, int rot)> Adjacent;

        public ScaledPoint FaceRef;

        public CubeFace(IEnumerable<IEnumerable<char>> face, ScaledPoint faceRef) {
            faceData = new Grid2D<char>(face);
            Adjacent = new();
            FaceRef = faceRef;
        }

        public override string ToString()
        {
            return $"CubeFace {FaceRef}";
        }

        public void SetAdjacentFace(CubeFace face, Direction d, int rotation) {
            Adjacent[d] = (face, Mod(rotation, 4));
            face.Adjacent[(-d).Rotate(rotation)] = (this, Mod(-rotation, 4));
        }

        protected (Direction dir, int rot) SumCubeMovements(Direction d1, Direction d2) {
            // The number is the rotation index to be applied 
            // to a vector on the starting face moving towards dir
            // in order to be consistent with the orientations
            if (Parallel(d1, d2)) {
                throw InPanic();
            }
            int rot = 1;
            Direction r = new("R");
            Direction u = new("U");
            Direction l = new("L");
            Direction d = new("D");
            if (d1.IsIn(Arr(d, l))) {
                rot *= -1;
            }
            if (d2.IsIn(Arr(d, l))) {
                rot *= -1;
            }
            if (d1.IsIn(Arr(l, r))) {
                rot *= -1;
            }
            return (d2, Mod(rot, 4));
        }

        public void PropagateFaces() {
            foreach(Direction d1 in Adjacent.Keys.ToArray()) {
                var newFace = Adjacent[d1];
                foreach (Direction d2 in newFace.face.Adjacent.Keys.ToArray()) {
                    if (!Parallel(d1.Rotate(newFace.rot), d2)) {
                        (Direction d3, int rot2) = SumCubeMovements(d1, d2.Rotate(-newFace.rot));
                        if (Adjacent.ContainsKey(d3)) {
                            continue;
                        }
                        var tested = newFace.face.Adjacent[d2];
                        SetAdjacentFace(tested.face, d3, newFace.rot + rot2 + tested.rot);
                    }
                }
            }
        }
    }

    public class Cube {
        public int dimCube;
        public (int rows, int cols) cubeShape;
        public Dictionary<ScaledPoint, CubeFace> Faces;
        public Grid2D<char> map;

        public ScaledPoint CurrentFace;
        public Movable CurrentPosition;
        public Cube(bool useExample, List<string> mapData) {
            
            Faces = new();

            if (useExample) {
                dimCube = 4;
            }
            else {
                dimCube = 50;
            }

            int maxH = mapData.Select(s => s.Length).Max();
            map = new(
                mapData.Select(s => s.PadRight(maxH))
            );

            cubeShape = (map.Shape.rows / dimCube, map.Shape.cols / dimCube);
            bool firstFaceSet = false;
            CurrentFace = new Point(0,0);
            CurrentPosition = new Movable(0, 0, new Direction(0), upIsNegative: true);

            foreach(int y in IntRange(cubeShape.rows)) {
                foreach (int x in IntRange(cubeShape.cols)) {
                    ScaledPoint cubePoint = new(x, y);
                    if (IsInMap(cubePoint)) {
                        Faces[cubePoint] = SelectCubeFace(cubePoint);
                        if (!firstFaceSet) {
                            firstFaceSet = true;
                            CurrentFace = cubePoint;
                        }
                    }
                }
            }

            foreach(ScaledPoint cubePoint in Faces.Keys) {
                foreach(ScaledPoint adjPoint in cubePoint.Adjacent()) {
                    if (adjPoint.IsIn(Faces.Keys)) {
                        Faces[cubePoint].SetAdjacentFace(
                            Faces[adjPoint],
                            (adjPoint - cubePoint).ToDirection(upIsNegative: true),
                            0
                        );
                    }
                }
            }

            foreach(CubeFace face in Faces.Values) {
                face.PropagateFaces();
            }
            foreach(CubeFace face in Faces.Values) {
                face.PropagateFaces();
            }
            foreach(CubeFace face in Faces.Values) {
                face.PropagateFaces();
            }
            foreach(CubeFace face in Faces.Values) {
                face.PropagateFaces();
            }
            foreach(CubeFace face in Faces.Values) {
                face.PropagateFaces();
            }
            foreach(CubeFace face in Faces.Values) {
                face.PropagateFaces();
            }

            foreach(CubeFace face in Faces.Values) {
                if (face.Adjacent.Count() != 4) {
                    throw InPanic();
                }
            }
        }

        public Point OnMap() {
            return CurrentPosition + (FromCube(CurrentFace) - new Point());
        }

        public CubeFace SelectCubeFace(ScaledPoint cubePoint) {
            MapPoint mapPoint = FromCube(cubePoint);
            return new CubeFace( map[
                (mapPoint.Y + 1) .. (mapPoint.Y + 1 + dimCube),
                (mapPoint.X + 1) .. (mapPoint.X + 1 + dimCube)
            ], cubePoint );
        }
        
        public MapPoint FromCube(ScaledPoint cubeP) {
            return (cubeP - new ScaledPoint()) * dimCube + new MapPoint();
        }
        public ScaledPoint ToCube(MapPoint p) {
            return (p - new MapPoint()) / dimCube + new ScaledPoint();
        }

        public bool IsInMapLimit(MapPoint p) {
            return p.X.IsInInterval(0, map.Shape.cols - 1)
                && p.Y.IsInInterval(0, map.Shape.rows - 1);
        }

        public bool IsInMap(ScaledPoint p) {
            MapPoint q = FromCube(p);
            return IsInMapLimit(q) && map[q] != ' ';
        }

        public Movable RotatePositionOnCubeEdge(Movable pos, int rot) {
            Point p = pos + Vector.FromDirection(-pos.dir, upIsNegative: true) * dimCube;
            MatrixCoord m = new MatrixCoord(p.Y + 1, p.X + 1);
            m = m.Rotate(rot, (dimCube, dimCube));
            return new Movable(m.Col - 1, m.Row - 1, pos.dir.Rotate(rot), pos.UpIsNegative);
        }

        public void MovePoint(int stepNum) {
            foreach (int _ in IntRange(stepNum)) {
                Movable pos = CurrentPosition.Copy();
                CubeFace face = Faces[CurrentFace];
                pos.Move();
                char c;
                try {
                    c = face.faceData[pos];
                }
                catch {
                    // We are over the boundary
                    var (newFace, rot) = face.Adjacent[pos.dir];
                    pos = RotatePositionOnCubeEdge(pos, rot);
                    c = newFace.faceData[pos];
                    if (c != '#') {
                        CurrentFace = newFace.FaceRef;
                    }
                }
                if (c == '#') {
                    break;
                }
                else {
                    CurrentPosition = pos;
                }
                // Log(OnMap());
            }
        }

        public Movable MoveOnCube(List<int> steps, List<string> dirs) {
            // Log(OnMap());
            foreach ((int step, string dir) in steps.Zip(dirs)) {
                MovePoint(step);
                CurrentPosition.Turn(dir);
            }
            MovePoint(steps[^1]);
            Movable q = CurrentPosition.Copy();
            q.MoveTo(OnMap());
            return q;
        }

    }

    public static class Part2 {

        public static string solve(bool useExample) {

            Input reader = new Input(useExample: useExample);
            
            string result = "0";
            long resultInt = 0;

            var data1 = reader.ReadParagraphs();

            var map1 = data1[0];
            var directions1 = data1[1][0];

            Regex numInfo = new Regex(@"\d+");
            Regex dirInfo = new Regex(@"L|R");

            List<int> nums = numInfo.Matches(directions1)
                .Select(m => m.Value.ToInt())
                .ToList();
            List<string> dirs = dirInfo.Matches(directions1)
                .Select(m => m.Value)
                .ToList();

            Cube cube = new Cube(useExample, map1);

            
            
            
            Movable final = cube.MoveOnCube(nums, dirs);

            resultInt = (final.Y + 1) * 1000 + (final.X + 1) * 4 + Mod(-final.dir.ToInt(), 4);
            


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }

}