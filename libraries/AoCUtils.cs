using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static System.Math;

namespace AoCUtils {
    using static Functions;

    public class Reverse<T> : Comparer<T> where T : IComparable {
        public override int Compare(T? x, T? y){
            return y?.CompareTo(x) ?? -1;
        }
    }

    public static class MyExtensions {
        public static IEnumerable<(int index, T item)> WithIndex<T>(this IEnumerable<T> self) =>
            self.Select((item, index) => (index, item));
    }

    public class Constants {
        public const string Solid = "\u2588";
        public const string Full = Solid;
        public const string Empty = " ";
        public const string Path = "·";
    }

    public class Generic2D {
        public readonly int X;
        public readonly int Y;

        public Generic2D(int x, int y) {
            X = x;
            Y = y;
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

        public override bool Equals(object? obj)
        {
            if (obj is null || !(this.GetType().Equals(obj.GetType()))) {
                return false;
            } else {
                Generic2D other = (Generic2D)obj;
                return this.ToTuple() == other.ToTuple();
            }
        }

        public override int GetHashCode() =>
            this.ToTypeString().GetHashCode();

    }

    public class Point2D : Generic2D {

        public Point2D(int x, int y) : base(x, y)
        {}
        public Point2D() : this(0, 0)
        {}

        public int CompareTo(Point2D other) {
            return Sign(Y - other.Y) * 2 + Sign(X - other.X);
        }

        public int Distance(Point2D other) {
            return (this - other).Distance();
        }
        public int Distance(){
            return this.Distance(new Point2D(0,0));
        }

        public double Length(Point2D other) {
            return (this - other).Length();
        }
        public double Length(){
            return this.Length(new Point2D(0,0));
        }

        public virtual IEnumerable<Point2D> Adjacent(bool corners = false) {
            for (int i = 0; i < 4; i++) {
                yield return this + Vector2D.FromDirection(i); 
            }
            if (corners) {
                for (int i = 0; i < 4; i++) {
                    yield return this + Vector2D.FromDirection(i) + Vector2D.FromDirection(i+1); 
                }
            }
        }


        public override bool Equals(object? obj) => base.Equals(obj);

        public override int GetHashCode() => base.GetHashCode();

        public static Vector2D operator - (Point2D left, Point2D right) {
            return new Vector2D(left.X - right.X, left.Y - right.Y);
        }
        public static Point2D operator + (Point2D left, Vector2D right) {
            return new Point2D(left.X - right.X, left.Y - right.Y);
        }
        public static Point2D operator + (Vector2D left, Point2D right) {
            return right + left;
        }
        public static bool operator == (Point2D left, Point2D right) {
            return left.Equals(right);
        }
        public static bool operator != (Point2D left, Point2D right) {
            return !(left == right);
        }
    }

    public class Vector2D : Generic2D {

        public Vector2D(int x, int y) : base(x, y)
        {}
        public Vector2D() : this(0, 0)
        {}
        

        public int CompareTo(Vector2D other) {
            return Sign(Y - other.Y) * 2 + Sign(X - other.X);
        }

        public int Distance() {
            return Abs(X) + Abs(Y);
        }

        public double Length() {
            return Sqrt(Pow(X, 2) + Pow(Y, 2));
        }

        public Vector2D Direction() {
            if (this.ToTuple() == (0, 0)) {
                return this;
            }
            int d = Gcd(X, Y);
            return new Vector2D(X / d, Y / d);
        }

        public Vector2D Rotate(int n) {
            return Mod(n, 4) switch {
                0 => this,
                1 => new Vector2D(-Y, X),
                2 => new Vector2D(-X, -Y),
                3 => new Vector2D(Y, -X),
                _ => throw new NotSupportedException()
            };
        }
        public Vector2D Rotate(string s) {
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

        public static Vector2D FromDirection(int n) {
            return Mod(n, 4) switch {
                0 => new Vector2D(1, 0),
                1 => new Vector2D(0, 1),
                2 => new Vector2D(-1, 0),
                3 => new Vector2D(0, -1),
                _ => throw new NotSupportedException()
            };
        }
        public static Vector2D FromDirection(string s) {
            return s switch {
                "R" or ">" or "E" or "0" => Vector2D.FromDirection(0),
                "U" or "^" or "N" or "1" => Vector2D.FromDirection(1),
                "L" or "<" or "W" or "2" => Vector2D.FromDirection(2),
                "D" or "v" or "S" or "3" => Vector2D.FromDirection(3),
                _ => throw new NotSupportedException()
            };
        }

        public static Vector2D operator * (int left, Vector2D right) {
            return new Vector2D(left * right.X, left * right.Y);
        }
        public static Vector2D operator * (Vector2D left, int right) {
            return right * left;
        }
        public static Vector2D operator + (Vector2D left, Vector2D right) {
            return new Vector2D(left.X + right.X, left.Y + right.Y);
        }
        public static Vector2D operator - (Vector2D v) {
            return new Vector2D(-v.X, -v.Y);
        }
        public static Vector2D operator - (Vector2D left, Vector2D right) {
            return left + (-right);
        }
        public static bool operator == (Vector2D left, Vector2D right) {
            return left.Equals(right);
        }
        public static bool operator != (Vector2D left, Vector2D right) {
            return !(left == right);
        }
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
    }
}