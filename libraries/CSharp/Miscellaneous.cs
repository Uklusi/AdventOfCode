using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using static System.Math;

namespace AoCUtils {
    using static Functions;

    public static class Constants {
        public const char SOLID = '\u2588';
        public const char FULL = SOLID;
        public const char EMPTY = ' ';
        public const char PATH = '·';
    }

    public static class Functions {
        public static Exception InPanic() {
            return new Exception("AAAA");
        }

        public static (int min, int max) MinMax(int a, int b) {
            return (Min(a, b), Max(a, b));
        }
        public static (long min, long max) MinMax(long a, long b) {
            return (Min(a, b), Max(a, b));
        }

        public static int Mod(int a, int m) {
            return ((a % m) + m) % m;
        }
        public static long Mod(long a, long m) {
            return ((a % m) + m) % m;
        }
        public static int Mod(long a, int m) {
            return (int)(((a % m) + m) % m);
        }

        public static int Gcd(int a, int b) {
            if (a == 0 && b == 0){
                throw new ArgumentException("Cannot calculate gcd between the provided numbers");
            }

            (a, b) = (Abs(a), Abs(b));
            (a, b) = (Min(a, b), Max(a, b));

            // It is guaranteed that a1, b1 are positive and a1 <= b1
            int GcdNoCheck(int a1, int b1) {
                if (a1 == 0) {
                    return b1;
                }
                return GcdNoCheck(b1 % a1, a1);
            }
            
            return GcdNoCheck(a, b);
        }
        public static long Gcd(long a, long b) {
            if (a == 0 && b == 0){
                throw new ArgumentException("Cannot calculate gcd between the provided numbers");
            }

            (a, b) = (Abs(a), Abs(b));
            (a, b) = (Min(a, b), Max(a, b));

            // It is guaranteed that a1, b1 are positive and a1 <= b1
            long GcdNoCheck(long a1, long b1) {
                if (a1 == 0) {
                    return b1;
                }
                return GcdNoCheck(b1 % a1, a1);
            }
            
            return GcdNoCheck(a, b);
        }

        public static T[] Arr<T>(params T[] args) => args;

        public static IEnumerable<T> Repeat<T>(T value, int numTimes){
            if (numTimes <= 0) {
                throw new ArgumentOutOfRangeException(
                    nameof(numTimes),
                    "Number of repeats must be greater than 0"
                );
            }
            return IntRange(numTimes).Select(_ => value);
        }
        public static IEnumerable<T> RepeatFlat<T>(IEnumerable<T> value, int numTimes){
            if (numTimes <= 0) {
                throw new ArgumentOutOfRangeException(
                    nameof(numTimes),
                    "Number of repeats must be greater than 0"
                );
            }
            return IntRange(numTimes).Select(_ => value).Flatten();
        }

        public static IEnumerable<(T1, T2)> Zip<T1, T2>(IEnumerable<T1> a, IEnumerable<T2> b) {
            return a.Zip(b);
        }

        public static IEnumerable<int> IntRange(int start, int end, int step) {
            if (step == 0 || Sign(end - start) != Sign(step)) {
                yield break;
            }
            for(int i = start; i < end; i+= step) {
                yield return i;
            }
        }
        public static IEnumerable<int> IntRange(int start, int end) => IntRange(start, end, 1);
        public static IEnumerable<int> IntRange(int end) => IntRange(0, end, 1);
    }

    public class DefaultDictionary<TKey, TValue> : Dictionary<TKey, TValue> where TKey : notnull {
        private readonly TValue _defaultValue;
        public DefaultDictionary(TValue defaultValue) {
            _defaultValue = defaultValue;
        }
        public DefaultDictionary() {
            _defaultValue = default(TValue) ?? throw new Exception("Value type has no default value");
        }
        
        new public TValue this[TKey key] {
            get {
                return TryGetValue(key, out TValue? value) ? value : _defaultValue;
            }
            set {
                base[key] = value;
            }
        }
    }
    
    public class Counter<T> : Dictionary<T, int> where T : notnull {
        private readonly int _defaultValue;
        public Counter(int defaultValue) {
            _defaultValue = defaultValue;
        }
        public Counter() : this(0) {}

        new public int this[T key] {
            get {
                return TryGetValue(key, out int value) ? value : _defaultValue;
            }
            set {
                base[key] = value;
            }
        }

        public void SetMax(T key, int n) {
            this[key] = Max(this[key], n);
        }
        public void SetMin(T key, int n) {
            this[key] = Min(this[key], n);
        }
    }

    public class OrderedDictionary<TKey, TValue> : 
        IDictionary<TKey, TValue>
    {
        public struct Enumerator : IEnumerator, IEnumerator<KeyValuePair<TKey, TValue>> {
            public IDictionaryEnumerator _e;
            public bool MoveNext() {
                return _e.MoveNext();
            }
            public void Reset() {
                _e.Reset();
            }
            public KeyValuePair<TKey, TValue> Current {
                get {
                    return new((TKey)_e.Entry.Key, (TValue)_e.Entry.Value!);
                }
            }
            object IEnumerator.Current {
                get {
                    return new KeyValuePair<TKey, TValue>((TKey)_e.Entry.Key, (TValue)_e.Entry.Value!);
                }
            }
            public void Dispose() {}
        }
        private OrderedDictionary _d = new();
        public OrderedDictionary() : base() {}

        public TValue this[TKey key] {
            get {
                return (TValue)(
                    _d[key!] ?? throw new Exception("I hate you stop putting null as a value")
                );
            }
            set {
                TKey k = key ?? throw new Exception("Key cannot be null");
                _d[k] = value ?? throw new Exception("I hate you stop putting null as a value");
            }
        }
        public int Count {
            get {return _d.Count;}
        }
        public bool IsReadOnly {
            get {return _d.IsReadOnly;}
        }
        public ICollection<TKey> Keys {
            get {return _d.Keys.Cast<TKey>().ToList();}
        }
        public ICollection<TValue> Values {
            get {return _d.Values.Cast<TValue>().ToList();}
        }

        public void Add(KeyValuePair<TKey, TValue> kv) {
            this[kv.Key] = kv.Value;
        }
        public void Add(TKey key, TValue value) {
            this[key] = value;
        }

        public void Clear() {
            _d.Clear();
        }
        public bool ContainsKey(TKey key) {
            if (key is null) {
                throw new ArgumentNullException(nameof(key));
            }
            return _d.Contains(key!);
        }
        public bool Contains(KeyValuePair<TKey, TValue> kv) {

            if (!this.ContainsKey(kv.Key)) {
                return false;
            }
            if (kv.Value is not null) {
                return kv.Value.Equals(this[kv.Key]);
            }
            return this[kv.Key] is null;
        }
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {
            _d.CopyTo(array, arrayIndex);
        }
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
            return new OrderedDictionary<TKey, TValue>.Enumerator{_e = _d.GetEnumerator()};
        }
        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }
        public bool Remove(TKey key) {
            bool success = false;
            if (this.ContainsKey(key!)) {
                success = true;
                _d.Remove(key!);
            }
            return success;
        }
        public bool Remove(KeyValuePair<TKey, TValue> kv) {
            bool success = false;
            if (this.ContainsKey(kv.Key!)) {
                success = true;
                _d.Remove(kv.Key!);
            }
            return success;
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) {
            if (this.ContainsKey(key)) {
                value = this[key];
                return true;
            }
            value = default;
            return false;
        }

    }
    
}