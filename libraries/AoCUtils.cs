using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static System.Math;

namespace AoCUtils {
    using FrameType = IEnumerable<IEnumerable<char>>;
    using static Functions;
    using static Constants;

    public class Reverse<T> : Comparer<T> where T : IComparable {
        public override int Compare(T? x, T? y){
            return y?.CompareTo(x) ?? -1;
        }
    }

    public static class MyExtensions {
        public static IEnumerable<(int index, T item)> Enumerate<T>(this IEnumerable<T> self) =>
            self.Select((item, index) => (index, item));
        
        public static T GetOne<T>(this IEnumerable<T> self) {
            if (self.Count() > 0) {
                return self.ToList()[0];
            }
            throw new IndexOutOfRangeException("Cannot get one element from empty enumberable");
        }

        public static int ToInt(this string s) => int.Parse(s);

        public static bool IsInInterval(this int testing, int lowerLimit, int upperLimit) {
            return (lowerLimit <= testing && testing <= upperLimit);
        }
        public static bool IsInInterval(this int testing, IEnumerable<int> limits) {
            if (limits.Count() != 2) {
                throw new ArgumentException("An interval needs to have two extremes");
            }
            int[] limitsArr = limits.ToArray();
            int lowerLimit = limitsArr[0];
            int upperLimit = limitsArr[1];
            return testing.IsInInterval(lowerLimit, upperLimit);
        }

        public static string JoinString<T>(this IEnumerable<T> iType, char c) {
            return String.Join("", iType);
        }
        public static string JoinString<T>(this IEnumerable<T> iType, string s) {
            return String.Join(s, iType);
        }
        public static string JoinString<T>(this IEnumerable<T> iType) {
            return iType.JoinString("");
        }

    }

    public static class Constants {
        public const char Solid = '\u2588';
        public const char Full = Solid;
        public const char Empty = ' ';
        public const char Path = '·';
    }

    public static class Functions {
        public static int Mod(int a, int m) {
            return ((a % m) + m) % m;
        }

        public static int Gcd(int a, int b) {
            if (a < 0 || b < 0 || (a == 0 && b == 0)){
                throw new ArgumentException("Cannot calculate gcd between the provided numbers");
            }

            (a, b) = (Min(a, b), Max(a, b));
            if (a == 0) {
                return b;
            }
            return Gcd(b % a, a);
        }

        public static Func<T, U> MemoizeNonRecursive<T, U>(Func<T, U> f) where T : IEquatable<T> {
            Dictionary<T, U> memoryCache = new();

            U MemoizedFunction(T input) {
                if (memoryCache.ContainsKey(input)) {
                    return memoryCache[input];
                }
                U value = f(input);
                memoryCache[input] = value;
                return value;
            }

            return MemoizedFunction;
        }
        public static Func<T1, T2, U> MemoizeNonRecursive<T1, T2, U>(Func<T1, T2, U> f)
            where T1 : IEquatable<T1>
            where T2 : IEquatable<T2> {
                var g = MemoizeNonRecursive<(T1, T2), U>(x => f(x.Item1, x.Item2));
                return (T1 a, T2 b) => g((a, b));
        } 
    }

    namespace GridUtils {
        
        public class Direction {
            readonly int dir;
            
            public Direction(int i) {
                dir = Mod(i, 4);
            }
            public Direction(string s) {
                dir = s switch {
                    "R" or ">" or "E" or "0" => 0,
                    "U" or "^" or "N" or "1" => 1,
                    "L" or "<" or "W" or "2" => 2,
                    "D" or "v" or "S" or "3" => 3,
                    _ => throw new NotSupportedException()
                };
            }

            public Direction Rotate(int n) {
                return new Direction(dir + n);
            }
            public Direction Rotate(string s) {
                return this.Rotate(
                    s switch {
                        "L" => 1,
                        "R" => 3,
                        _ => throw new NotSupportedException()
                    }
                );
            }

            public int ToInt(){
                return dir;
            }

            public override string ToString(){
                return dir switch {
                    0 => "→",
                    1 => "↑",
                    2 => "←",
                    3 => "↓",
                    _ => throw new NotSupportedException()
                };
            }

            public override bool Equals(object? obj) {
                if (obj is null || !(this.GetType().Equals(obj.GetType()))) {
                    return false;
                } else {
                    Direction other = (Direction)obj;
                    return this == other;
                }
            }

            public override int GetHashCode() {
                return ToString().GetHashCode();
            }

            public static bool operator == (Direction left, Direction right) {
                return left.dir == right.dir;
            }
            public static bool operator != (Direction left, Direction right) {
                return !(left == right);
            }
            public static Direction operator - (Direction self) {
                return self.Rotate(2);
            }
        }

        public class Generic2D {

            protected int _X;
            protected int _Y;

            public int X {
                get { return _X; }
            }
            public int Y {
                get { return _Y; }
            }

            public Generic2D(int x, int y) {
                _X = x;
                _Y = y;
            }

            public (int, int) ToTuple() {
                return (X, Y);
            }

            public override string ToString() {
                return $"({X}, {Y})";
            }

            public string ToTypeString(){
                return $"{this.GetType()}({X}, {Y})";
            }

            public int CompareTo(Generic2D other) {
                return Sign(Y - other.Y) * 2 + Sign(X - other.X);
            }

            public override bool Equals(object? obj)
            {
                if (obj is null || !(this.GetType().Equals(obj.GetType()))) {
                    return false;
                } else {
                    Generic2D other = (Generic2D)obj;
                    return this.CompareTo(other) == 0;
                }
            }

            public override int GetHashCode() =>
                this.ToTypeString().GetHashCode();

        }

        public class Vector : Generic2D {

            public Vector(int x, int y) : base(x, y)
            {}
            public Vector() : this(0, 0)
            {}
            

            public int CompareTo(Vector other) {
                return Sign(Y - other.Y) * 2 + Sign(X - other.X);
            }

            public int Distance() {
                return Abs(X) + Abs(Y);
            }

            public double Length() {
                return Sqrt(Pow(X, 2) + Pow(Y, 2));
            }

            public Vector Normalized() {
                if (this.ToTuple() == (0, 0)) {
                    return this;
                }
                int d = Gcd(X, Y);
                return new Vector(X / d, Y / d);
            }

            public Vector Rotate(int n) {
                return Mod(n, 4) switch {
                    0 => this,
                    1 => new Vector(-Y, X),
                    2 => new Vector(-X, -Y),
                    3 => new Vector(Y, -X),
                    _ => throw new NotSupportedException()
                };
            }
            public Vector Rotate(string s) {
                return this.Rotate(
                    s switch {
                        "L" => 1,
                        "R" => 3,
                        _ => throw new NotSupportedException()
                    }
                );
            }

            public override bool Equals(object? obj) => base.Equals(obj);
            public override int GetHashCode() => base.GetHashCode();

            public static Vector FromDirection(Direction d) {
                return d.ToInt() switch {
                    0 => new Vector(1, 0),
                    1 => new Vector(0, 1),
                    2 => new Vector(-1, 0),
                    3 => new Vector(0, -1),
                    _ => throw new NotSupportedException()
                };
            }

            public static Vector operator * (int left, Vector right) {
                return new Vector(left * right.X, left * right.Y);
            }
            public static Vector operator * (Vector left, int right) {
                return right * left;
            }
            public static Vector operator + (Vector left, Vector right) {
                return new Vector(left.X + right.X, left.Y + right.Y);
            }
            public static Vector operator - (Vector v) {
                return new Vector(-v.X, -v.Y);
            }
            public static Vector operator - (Vector left, Vector right) {
                return left + (-right);
            }
            public static bool operator == (Vector left, Vector right) {
                return left.Equals(right);
            }
            public static bool operator != (Vector left, Vector right) {
                return !(left == right);
            }
        }

        public class Point : Generic2D {

            public Point(int x, int y) : base(x, y)
            {}
            public Point() : this(0, 0)
            {}

            public int Distance(Point other) {
                return (this - other).Distance();
            }
            public int Distance(){
                return this.Distance(new Point(0,0));
            }

            public double Length(Point other) {
                return (this - other).Length();
            }
            public double Length(){
                return this.Length(new Point(0,0));
            }

            public Point ToPoint(){
                return new Point(X, Y);
            }

            public virtual IEnumerable<Point> Adjacent(bool corners = false) {
                for (int i = 0; i < 4; i++) {
                    yield return this + Vector.FromDirection(new Direction(i)); 
                }
                if (corners) {
                    for (int i = 0; i < 4; i++) {
                        yield return this + Vector.FromDirection(new Direction(i)) +
                            Vector.FromDirection(new Direction(i+1));
                    }
                }
            }

            public override bool Equals(object? obj) => base.Equals(obj);

            public override int GetHashCode() => base.GetHashCode();

            public static Vector operator - (Point left, Point right) {
                return new Vector(left.X - right.X, left.Y - right.Y);
            }
            public static Point operator + (Point left, Vector right) {
                return new Point(left.X - right.X, left.Y - right.Y);
            }
            public static Point operator + (Vector left, Point right) {
                return right + left;
            }
            public static bool operator == (Point left, Point right) {
                return left.Equals(right);
            }
            public static bool operator != (Point left, Point right) {
                return !(left == right);
            }
            public static bool operator < (Point left, Point right) {
                return left.CompareTo(right) < 0;
            }
            public static bool operator > (Point left, Point right) {
                return right < left;
            }
            public static bool operator <= (Point left, Point right) {
                return left.CompareTo(right) <= 0;
            }
            public static bool operator >= (Point left, Point right) {
                return right <= left;
            }
        }

        public class MapPoint : Point {
            protected static List<List<char>>? Frame = null;
            protected static int[] xLimits = new int[2];
            protected static int[] yLimits = new int[2];
            protected static Func<Point, bool> _IsOccupied = (p => false);

            public static void SetFrame(IEnumerable<IEnumerable<char>> frame) {
                Frame = frame.Select(iter => iter.ToList()).ToList();
                xLimits[0] = 0;
                xLimits[1] = Frame.Count() - 1;
                yLimits[0] = 0;
                yLimits[1] = Frame[0].Count() - 1;
            }
            public static void SetFrame(int xUpper, int yUpper, int xLower = 0, int yLower = 0){
                xLimits[0] = xLower;
                xLimits[1] = xUpper;
                yLimits[0] = yLower;
                yLimits[1] = yUpper;
            }

            public static void SetOccupied(Func<Point, bool> isOccupied) {
                _IsOccupied = (p => (
                    p.Y.IsInInterval(yLimits) &&
                    p.X.IsInInterval(xLimits) &&
                    isOccupied(p)
                ));
            }
            public static void SetOccupied() {
                SetOccupied(p => Frame?[p.Y]?[p.X] != Empty);
            }

            public MapPoint(int x, int y) : base(x, y) {}

            public bool Occupied {get { return _IsOccupied(this.ToPoint()); }}
            public bool Free {get { return !this.Occupied; }}

            public override IEnumerable<MapPoint> Adjacent(bool corners = false)
            {
                foreach (Point p in base.Adjacent(corners)) {
                    MapPoint mapP = new MapPoint(p.X, p.Y);
                    if (!mapP.Occupied) {
                        yield return mapP;
                    }
                }
            }
        }

        public class Movable : MapPoint {
            Direction dir;
            public Movable(int x, int y, Direction d) : base(x, y) {
                dir = d;
            }

            public override int GetHashCode()
            {
                throw new NotSupportedException("Cannot hash class Movable");
            }

            public void Turn(int i){
                dir = dir.Rotate(i);
            }
            public void Turn(string s){
                dir = dir.Rotate(s);
            }

            public virtual void MoveTo(Point p) {
                MapPoint pMap = new(p.X, p.Y);
                if (pMap.Free) {
                    _X = p.X;
                    _Y = p.Y;
                }
            }

            public virtual void Move(int steps, Direction direction) {
                dir = direction;

                if (steps < 0) {
                    steps = -steps;
                    direction = -direction;
                }

                for (int i = 0; i < steps; i++) {
                    Point before = this.ToPoint();
                    MoveTo(this + Vector.FromDirection(direction));
                    Point after = this.ToPoint();
                    if (before == after) {
                        break;
                    }
                }
            }
            public virtual void Move(int steps) {
                Move(steps, dir);
            }
        }
    
        public class Image {
            protected char[][] _image;
            public Image(IEnumerable<IEnumerable<char>> imTemplate){
                _image = imTemplate.Select(l => l.ToArray()).ToArray();
            }

            public (int, int) Shape() {
                return (_image[0].Length, _image.Length);
            }

            public Image Copy(){
                return new Image(_image);
            }

            public Image Rotate(int numTimes) {
                int n = Mod(numTimes, 4);
                (int xMax, int yMax) = Shape();
                if (n == 0) {
                    return Copy();
                }
                else if (n == 1) {
                    char[][] newImg = new char[xMax][];
                    for (int x = xMax - 1; x >= 0; x--) {
                        char[] newRow = new char[yMax];
                        for (int y = 0; y < yMax; y++) {
                            newRow[y] = (_image[y][x]);
                        }
                        newImg[x] = newRow;
                    }
                    return new Image(newImg);
                }
                else if (n == 2) {
                    return new Image(
                        _image.Select(l => l.Reverse()).Reverse()
                    );
                }
                else if (n == 3) {
                    return Rotate(2).Rotate(1);
                }
                else {
                    throw new Exception("This shouldn't happen");
                }
            }

            public Image Flip(bool upDown=false) {
                if (upDown) {
                    return new Image(_image.Reverse());
                }
                else {
                    return new Image(_image.Select(l => l.Reverse()));
                }
            }

            public IEnumerable<Image> Rotations() {
                for (int i = 0; i < 4; i++) {
                    yield return Rotate(i);
                }
            }

            public IEnumerable<Image> Variations() {
                Image flipped = Flip();
                foreach (Image r in Rotations()){
                    yield return r;
                }
                foreach (Image r in flipped.Rotations()){
                    yield return r;
                }
            }

            public override string ToString() {
                return _image.Select(l => l.JoinString()).JoinString("\n");
            }

            public override bool Equals(object? obj){
                if (obj is null || !(this.GetType().Equals(obj.GetType()))) {
                    return false;
                } else {
                    Image other = (Image)obj;
                    return this == other;
                }
            }

            public override int GetHashCode() {
                return ToString().GetHashCode();
            }

            public static bool operator == (Image left, Image right) {
                return left._image == right._image;
            }
            public static bool operator != (Image left, Image right) {
                return !(left._image == right._image);
            }
            public static Image operator + (Image left, Image right) {
                return new Image(
                    left._image.Select((c, i) => c.Concat(right._image[i]))
                );
            }
            public static Image operator & (Image left, Image right) {
                return new Image(
                    left._image.Concat(right._image)
                );
            }
        }
        
        public class HexDirection {
            string dir;

            public static string[] Directions = new string[6]{"N", "NW", "NE", "S", "SE", "SW"};
            
            public HexDirection(string d) {
                d = d.Replace('U', 'N').Replace('D', 'S').Replace('L', 'W').Replace('R', 'E');
                dir = d switch {
                    "N" or "NW" or "NE" or "S" or "SE" or "SW" => d,
                    _ => throw new ArgumentException("Wrong direction")
                };
            }

            public override string ToString() {
                return dir;
            }

            public override bool Equals(object? obj) {
                if (obj is null || !(this.GetType().Equals(obj.GetType()))) {
                    return false;
                } else {
                    HexDirection other = (HexDirection)obj;
                    return this == other;
                }
            }

            public override int GetHashCode() {
                return ToString().GetHashCode();
            }

            public static bool operator == (HexDirection left, HexDirection right) {
                return left.ToString() == right.ToString();
            }
            public static bool operator != (HexDirection left, HexDirection right) {
                return !(left == right);
            }
            public static HexDirection operator - (HexDirection self) {
                return new HexDirection(
                    self.dir switch {
                        "N" => "S",
                        "NW" => "SE",
                        "SW" => "NE",
                        "S" => "N",
                        "SE" => "NW",
                        "NE" => "SW",
                        _ => throw new ArgumentException("This cannot happen")
                    }
                );
            }
        }

        public class HexPoint : Generic2D {
            public HexPoint(int i, int j) : base(i, j) {}
            public HexPoint() : base(0, 0) {}

            public override string ToString() {
                return $"<{X}, {Y}>";
            }

            public HexPoint Move(int numTimes, HexDirection dir) {
                if (numTimes < 0) {
                    throw new ArgumentException("Cannot move a negative number of steps");
                }

                (int a, int b) = dir.ToString() switch {
                    "N" => (1, 1),
                    "NW" => (0, 1),
                    "SW" => (-1, 0),
                    "S" => (-1, -1),
                    "SE" => (0, -1),
                    "NE" => (1, 0),
                    _ => throw new ArgumentException("Wrong direction")
                };
                return new HexPoint(X + a * numTimes, Y + b * numTimes);
            }

            public IEnumerable<HexPoint> Adjacent() {
                foreach (string dirStr in HexDirection.Directions) {
                    yield return Move(1, new HexDirection(dirStr));
                }
            }

            public int Distance(HexPoint other) {
                int a = X - other.X;
                int b = Y - other.Y;
                if (a * b <= 0) {
                    return Abs(a) + Abs(b);
                }
                else {
                    return Max(Abs(a), Abs(b));
                }
            }
        }

        public static class MazeFunctions {

            public static int AStar(
                MapPoint start,
                MapPoint finish,
                Func<Point, Point, int>? distanceFunc = null,
                Func<Point, Point, int>? estimateFunc = null,
                bool corners = false
            ) {
                Func<Point, Point, int> distanceFuncNN = distanceFunc ?? ((p, q) => p.Distance(q));
                Func<Point, Point, int> estimateFuncNN = estimateFunc ?? ((p, q) => p.Distance(q));

                Func<Point, int> estimateFinish = (p => estimateFuncNN(p, finish));
                PriorityQueue<MapPoint, int> openSet = new();
                Dictionary<Point, int> distances = new(){{start, 0}};
                openSet.Enqueue(start, estimateFinish(start));

                while (openSet.Count > 0) {
                    MapPoint current = openSet.Dequeue();
                    if (current == finish) {
                        return distances[finish];
                    }
                    foreach (MapPoint newPoint in current.Adjacent(corners: corners)) {
                        int tentativeDistance = distances[current] +
                            distanceFuncNN(current, newPoint);
                        if (
                            !distances.ContainsKey(newPoint) ||
                            tentativeDistance < distances[newPoint]
                        ) {
                            distances[newPoint] = tentativeDistance;
                            openSet.Enqueue(newPoint, tentativeDistance + estimateFinish(newPoint));
                        }
                    }
                }

                return -1;
            }

            public static Dictionary<Point, int> Dijkstra(
                MapPoint start,
                Func<Point, Point, int>? distanceFunc = null
            ) {
                Func<Point, Point, int> distanceFuncNN = distanceFunc ?? ((p, q) => p.Distance(q));
                PriorityQueue<MapPoint, int> openSet = new();
                Dictionary<Point, int> distances = new(){{start, 0}};
                openSet.Enqueue(start, 0);
                
                while (openSet.Count > 0) {
                    MapPoint current = openSet.Dequeue();
                    foreach (MapPoint newPoint in current.Adjacent()) {
                        int tentativeDistance = distances[current] +
                            distanceFuncNN(current, newPoint);
                        if (
                            !distances.ContainsKey(newPoint) ||
                            distances[newPoint] > tentativeDistance
                        ) {
                            distances[newPoint] = tentativeDistance;
                            openSet.Enqueue(newPoint, tentativeDistance);
                        }
                    }
                    
                }
                return distances;
            }
        
        }

    }
}