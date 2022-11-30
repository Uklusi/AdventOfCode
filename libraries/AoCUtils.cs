using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AoCUtils {

    public class Reverse<T> : Comparer<T> where T : IComparable {
        public override int Compare(T? x, T? y){
            return y?.CompareTo(x) ?? -1;
        }
    }

    public static class Functions {
        
    }
}