namespace AoCUtils {
    public class Reverse<T> : Comparer<T> where T : IComparable {
        public override int Compare(T? x, T? y){
            return y?.CompareTo(x) ?? -1;
        }
    }

    public class ReverseComparer<T> : Comparer<T> {
        private IComparer<T> _oldComparer;
        public ReverseComparer(IComparer<T> oldComparer) {
            _oldComparer = oldComparer;
        }

        public override int Compare(T? x, T? y) {
            return _oldComparer.Compare(y, x);
        }
    }

    public class ComparerFactory<T> {
        class InternalComparer : Comparer<T> {
            private Func<T, T, int> _comp;
            
            public InternalComparer(Func<T, T, int> comp) {
                _comp = comp;
            }

            public override int Compare(T? x, T? y) {
                if (x is null || y is null) {
                    return (x is null).ToInt() - (y is null).ToInt();
                }
                else {
                    return _comp(x, y);
                }
            }
        }
        

        public static IComparer<T> Create(Func<T, T, int> compare) {
            return new InternalComparer(compare);
        }
    }
}