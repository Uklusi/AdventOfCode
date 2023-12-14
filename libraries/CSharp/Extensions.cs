using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using static System.Math;

using AoCUtils.GridUtils;

namespace AoCUtils {

    public static class IEnumerableExtensions {
        
        public static bool Empty<T>(this IEnumerable<T> self) {
            return self.Count() == 0;
        }
        public static bool Empty<T, U>(this PriorityQueue<T, U> self) {
            return self.Count == 0;
        }

        public static bool NotEmpty<T>(this IEnumerable<T> self) => !self.Empty();
        public static bool NotEmpty<T, U>(this PriorityQueue<T, U> self) => !self.Empty();

        public static bool All<T>(this IEnumerable<T> self) {
            return self.All(x => x.ToBool());
        }

        // Does not work since list.Any() is list.NotEmpty()
        // public static bool Any<T>(this IEnumerable<T> self) {
        //     return self.Any(x => x.ToBool());
        // }
        public static bool AnyBool<T>(this IEnumerable<T> self) {
            return self.Any(x => x.ToBool());
        }


        public static IEnumerable<(int index, T item)> Enumerate<T>(this IEnumerable<T> self) =>
            self.Select((item, index) => (index, item));

        public static IEnumerable<(Point p, T item)> Enumerate2D<T>(this IEnumerable<IEnumerable<T>> self) {
            return self.Select(
                (line, y) => line.Select(
                    (item, x) => (p: new Point(x, y), item)
                )
            ).Flatten();
        }

        public static IEnumerable<IEnumerable<U>> SelectInner<T, U>(
            this IEnumerable<IEnumerable<T>> self,
            Func<T, U> f
        ) {
            return self.Select(l => l.Select(f));
        }

        public static IEnumerable<R> SelectTuple<T1, T2, R>(
            this IEnumerable<(T1, T2)> self,
            Func<T1, T2, R> f
        ) {
            return self.Select(t => f(t.Item1, t.Item2));
        } 

        public static IEnumerable<R> ZipSelect<T1, T2, R>(
            this IEnumerable<T1> self,
            IEnumerable<T2> other,
            Func<T1, T2, R> f
        ) {
            return self
                .Zip(other)
                .SelectTuple(f);
        }

        public static bool ComponentEquals<T>(this IEnumerable<T> self, IEnumerable<T> other)
        where T : IEquatable<T> {
            if (self.Count() != other.Count()) {
                return false;
            }
            return self.Zip(other).SelectTuple((l, r) => l.Equals(r)).All();
        }


        public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> self) {
            return self.SelectMany(i => i);
        }

        public static IEnumerable<(T Left, U Right)> Product<T, U>(this IEnumerable<T> self, IEnumerable<U> other) {
            return
                from t in self
                join u in other
                on 1 equals 1
                select (t, u);
        }
    }

    public static class DoubleArrayExtensions {
        public static IEnumerable<IEnumerable<T>> GetRows<T>(this T[,] self) {
            int rownum = self.GetLength(0);
            int colnum = self.GetLength(1);
            IEnumerable<T1> GetRow<T1>(int row, T1[,] self){
                for (int c = 0; c < colnum; c++) {
                    yield return self[row, c];
                }
            }
            for (int r = 0; r < rownum; r++) {
                yield return GetRow(r, self);
            }
        }
        public static IEnumerable<IEnumerable<T>> GetCols<T>(this T[,] self) {
            int rownum = self.GetLength(0);
            int colnum = self.GetLength(1);
            IEnumerable<T1> GetCol<T1>(int col, T1[,] self){
                for (int r = 0; r < rownum; r++) {
                    yield return self[r, col];
                }
            }
            for (int c = 0; c < colnum; c++) {
                yield return GetCol(c, self);
            }
        }
    }

    public static class EnumerableConversionExtensions {

        public static T[][] ToJaggedArray<T>(this IEnumerable<IEnumerable<T>> self) {
            return self.Select(l => l.ToArray()).ToArray();
        }

        public static List<List<T>> ToNestedList<T>(this IEnumerable<IEnumerable<T>> self) {
            return self.Select(l => l.ToList()).ToList();
        }

        public static T[,] ToDoubleArray<T>(this IEnumerable<IEnumerable<T>> self) {
            var s = self.ToJaggedArray();
            int rows = s.Length;
            int cols = s[0].Length;
            T[,] ret = new T[rows, cols];
            for (int r = 0; r < rows; r++) {
                for (int c = 0; c < cols; c++) {
                    ret[r,c] = s[r][c];
                }
            }
            return ret;
        }

        public static T[,] ToDoubleArrayFromCols<T>(this IEnumerable<IEnumerable<T>> self) {
            var s = self.ToJaggedArray();
            int rows = s[0].Length;
            int cols = s.Length;
            T[,] ret = new T[rows, cols];
            for (int r = 0; r < rows; r++) {
                for (int c = 0; c < cols; c++) {
                    ret[r,c] = s[c][r];
                }
            }
            return ret;
        }

        
        public static string JoinString<T>(this IEnumerable<T> self, char c) {
            return string.Join(c, self);
        }
        public static string JoinString<T>(this IEnumerable<T> self, string s) {
            return string.Join(s, self);
        }
        public static string JoinString<T>(this IEnumerable<T> self) {
            return self.JoinString("");
        }

        public static IEnumerable<string> ToStringEnum<T>(this IEnumerable<T> self) =>
            self
                .Select(e => e?.ToString() ?? "")
                .Where(s => s != "");

        public static string[] ToStringArray<T>(this IEnumerable<T> self) =>
            self
                .ToStringEnum()
                .ToArray();

        

        public static string ReprList<T>(this IEnumerable<T> self, string sep) =>
            "["
            + self
                .ToStringEnum()
                .JoinString(sep)
            + "]";
        public static string ReprList<T>(this IEnumerable<T> self) =>
            self.ReprList(", ");
        public static string ReprNestedList<T>(this IEnumerable<IEnumerable<T>> self) =>
            "["
            + self
                .Select(l => l.ReprList())
                .JoinString("\n")
            + "]";
        public static string ReprDict<K, V>(this Dictionary<K,V> self) where K : notnull =>
            "{\n"
            + self
                .Select(kv => $"{kv.Key}: {kv.Value}")
                .JoinString("\n")
            + "\n}";

        public static string ReprNestedListInline<T>(this IEnumerable<IEnumerable<T>> self) =>
            "["
            + self
                .Select(l => l.ReprList())
                .JoinString(" - ")
            + "]";
        public static string ReprDictInline<K, V>(this Dictionary<K,V> self) where K : notnull =>
            "{ "
            + self
                .Select(kv => $"{kv.Key}: {kv.Value}")
                .JoinString(" - ")
            + " ]";
    }

    public static class OtherExtensions {
        
        public static int ToInt(this string s) => int.Parse(s);
        public static int ToInt(this char c) => int.Parse(c.ToString());
        public static int ToInt(this bool b) => b ? 1 : 0;
        public static long ToLong(this string s) => long.Parse(s);
        public static bool ToBool<T>(this T? self) =>
            self is not null && !self.Equals(default(T));


        public static bool IsIn<T>(this T value, IEnumerable<T> options) {
            return options.Contains(value);
        }
        public static bool IsInInterval(this int testing, int lowerLimit, int upperLimit) {
            return lowerLimit <= testing && testing <= upperLimit;
        }
        public static bool IsInInterval(this int testing, (int, int) limits) {
            return testing.IsInInterval(limits.Item1, limits.Item2);
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


        public static T[] Copy<T>(this T[] self) {
            return self.ToArray();
        }
        public static List<T> Copy<T>(this List<T> self) {
            return self.ToList();
        }

        // Removed since it's the same as UnionWith
        // public static void AddRange<T>(this HashSet<T> self, IEnumerable<T> range) {
        //     self.EnsureCapacity(self.Count + range.Count());
        //     foreach (T el in range) {
        //         self.Add(el);
        //     }
        // }

    }
}