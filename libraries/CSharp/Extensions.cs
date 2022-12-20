using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using static System.Math;

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

        public static bool Any<T>(this IEnumerable<T> self) {
            return self.Any(x => x.ToBool());
        }
        public static bool AnyBool<T>(this IEnumerable<T> self) {
            return self.Any(x => x.ToBool());
        }

        public static IEnumerable<(int index, T item)> Enumerate<T>(this IEnumerable<T> self) =>
            self.Select((item, index) => (index, item));

        public static IEnumerable<R> ZipApply<T1, T2, R>(
            this IEnumerable<T1> self,
            IEnumerable<T2> other,
            Func<T1, T2, R> f
        ) {
            return self.Zip(other).Select(t => f(t.First, t.Second));
        }


        public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> self) {
            return self.SelectMany(i => i);
        }

        public static IEnumerable<IEnumerable<U>> SelectInner<T, U>(
            this IEnumerable<IEnumerable<T>> self,
            Func<T, U> f
        ) {
            return self.Select(l => l.Select(f));
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
            var s = self.Select(l => l.ToArray()).ToArray();
            int rows = s.Count();
            int cols = s[0].Count();
            T[,] ret = new T[rows, cols];
            for (int r = 0; r < rows; r++) {
                for (int c = 0; c < cols; c++) {
                    ret[r,c] = s[r][c];
                }
            }
            return ret;
        }


        
        public static string JoinString<T>(this IEnumerable<T> self, char c) {
            return String.Join(c, self);
        }
        public static string JoinString<T>(this IEnumerable<T> self, string s) {
            return String.Join(s, self);
        }
        public static string JoinString<T>(this IEnumerable<T> self) {
            return self.JoinString("");
        }



        public static string[] ToStringArray<T>(this IEnumerable<T> self) =>
            self
                .Select(e => e?.ToString() ?? "")
                .Where(s => s != "")
                .ToArray();

        public static string ToDictionaryString<K, V>(this Dictionary<K,V> self) where K : notnull =>
            "{\n"
            + self
                .Select(kv => $"{kv.Key}: {kv.Value}")
                .JoinString("\n")
            + "\n]";
        
        public static string ToContentString<T>(this IEnumerable<T> self, string sep) =>
            "["
            + self
                .Select(e => e?.ToString() ?? "")
                .Where(s => s != "")
                .JoinString(sep)
            + "]";
        public static string ToContentString<T>(this IEnumerable<T> self) =>
            self.ToContentString(", ");


        public static string ToFormattedString<T>(this IEnumerable<IEnumerable<T>> self) =>
            "["
            + self
                .Select(l => l.ToContentString())
                .JoinString("\n")
            + "]";
    }

    public static class OtherExtensions {
        
        public static int ToInt(this string s) => int.Parse(s);
        public static int ToInt(this char c) => int.Parse(c.ToString());
        public static int ToInt(this bool b) => b ? 1 : 0;
        public static long ToLong(this string s) => long.Parse(s);
        public static bool ToBool<T>(this T? self) =>
            self is null || self.Equals(default(T));


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
        public static bool IsIn<T>(this T value, IEnumerable<T> options) {
            return options.Contains(value);
        }


        public static T[] Copy<T>(this T[] self) {
            return self.ToArray();
        }
        public static List<T> Copy<T>(this List<T> self) {
            return self.ToList();
        }

    }
}