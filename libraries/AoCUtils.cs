using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static System.Math;

namespace AoCUtils {

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

    public class Point2D {
        public int X;
        public int Y;

        public Point2D() {
            X = 0;
            Y = 0;
        }
        public Point2D(int x, int y) {
            X = x;
            Y = y;
        }

        public int CompareTo(Point2D other) {
            return Sign(Y - other.Y) * 2 + Sign(X - other.X);
        }

        public (int, int) ToTuple() {
            return (X, Y);
        }

        public override string ToString() {
            return $"({X}, {Y})";
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

        public static Vector2D operator - (Point2D left, Point2D right) {
            return new Vector2D(left.X - right.X, left.Y - right.Y);
        }
        public static Point2D operator + (Point2D left, Vector2D right) {
            return new Point2D(left.X - right.X, left.Y - right.Y);
        }
        public static Point2D operator + (Vector2D left, Point2D right) {
            return right + left;
        }
    }

    public class Vector2D {
        public int X;
        public int Y;

        public Vector2D() {
            X = 0;
            Y = 0;
        }
        public Vector2D(int x, int y) {
            X = x;
            Y = y;
        }

        public int CompareTo(Vector2D other) {
            return Sign(Y - other.Y) * 2 + Sign(X - other.X);
        }

        public (int, int) ToTuple() {
            return (X, Y);
        }

        public override string ToString() {
            return $"({X}, {Y})";
        }

        public int Distance() {
            return Abs(X) + Abs(Y);
        }

        public double Length() {
            return Sqrt(Pow(X, 2) + Pow(Y, 2));
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

    }

    public static class Functions {
        public static int mod(int a, int m) {
            return ((a % m) + m) % m;
        }
    }
}