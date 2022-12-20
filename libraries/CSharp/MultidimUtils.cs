using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using static System.Math;

namespace AoCUtils {
    using static Functions;

    namespace MultidimUtils {

        public class VectorMultiDim {
            int[] _comps;
            
            public VectorMultiDim(IEnumerable<int> components) {
                _comps = components.ToArray();
            }
            public VectorMultiDim(params int[] components){
                _comps = components;
            }

            public int Dimension {get => _comps.Length;}
            public int[] Components {get => (int[])_comps.Clone();}

            public override string ToString() {
                return "<" + _comps.JoinString(", ") + ">";
            }

            public string ToTypeString(){
                return $"{GetType()}{ToString()}";
            }

            public override bool Equals(object? obj)
            {
                if (obj is null || !(GetType().Equals(obj.GetType()))) {
                    return false;
                } else {
                    VectorMultiDim other = (VectorMultiDim)obj;
                    return _comps == other.Components;
                }
            }

            public override int GetHashCode() => ToTypeString().GetHashCode();
            
            public VectorMultiDim Normalized() {
                if (_comps.All(i => i == 0)) {
                    return this;
                }
                int d = _comps.Aggregate(Gcd);
                return new VectorMultiDim(_comps.Select(i => i / d));
            }

            public int Length() {
                return _comps.Select(Abs).Aggregate((i, j) => i + j);
            }
            
            public static VectorMultiDim operator * (int left, VectorMultiDim right) {
                return new VectorMultiDim(right.Components.Select(i => left * i));
            }
            public static VectorMultiDim operator * (VectorMultiDim left, int right) {
                return right * left;
            }
            public static VectorMultiDim operator + (VectorMultiDim left, VectorMultiDim right) {
                if (
                    left.Dimension != right.Dimension
                ) {
                    throw new ArgumentException("Cannot sum Vectors with different dimensions");
                }

                return new VectorMultiDim(
                    left.Components.ZipApply(
                        right.Components,
                        (l, r) => l + r
                    )
                );
            }
            public static VectorMultiDim operator - (VectorMultiDim v) {
                return (-1 * v);
            }
            public static VectorMultiDim operator - (VectorMultiDim left, VectorMultiDim right) {
                return left + (-right);
            }
            
            public static bool operator == (VectorMultiDim left, VectorMultiDim right) {
                if (
                    left.Dimension != right.Dimension
                ) {
                    throw new ArgumentException("Cannot sum Vectors with different dimensions");
                }

                return left.Equals(right);
            }
            public static bool operator != (VectorMultiDim left, VectorMultiDim right) {
                return !(left == right);
            }

            public static int operator / (VectorMultiDim left, VectorMultiDim right) {
                if (left.Dimension != right.Dimension) {
                    throw new ArgumentException();
                }

                var lc = left.Components;
                var rc = right.Components;

                if (lc.Any(i => i < 0) || rc.Any(i => i < 0)) {
                    throw new NotImplementedException("I don't wanna think about it");
                }

                if (rc.All(i => i==0)) {
                    throw new ArgumentException("Cannot divide by 0");
                }
                return lc.ZipApply(rc, (i, j) => j != 0 ? i / j : int.MaxValue).Min();
            }
        }

        public class PointMultiDim {
            int[] _coords;
            
            public PointMultiDim(IEnumerable<int> coords) {
                _coords = coords.ToArray();
            }
            public PointMultiDim(params int[] coords){
                _coords = coords;
            }

            public int Dimension {get => _coords.Length;}
            public int[] Coordinates {get => (int[])_coords.Clone();}

            public override string ToString() {
                return "(" + _coords.JoinString(", ") + ")";
            }

            public string ToTypeString(){
                return $"{GetType()}{ToString()}";
            }

            public override bool Equals(object? obj)
            {
                if (obj is null || !(GetType().Equals(obj.GetType()))) {
                    return false;
                } else {
                    PointMultiDim other = (PointMultiDim)obj;
                    return _coords == other._coords;
                }
            }

            public override int GetHashCode() =>
                this.ToTypeString().GetHashCode();

            public int Distance(PointMultiDim other) {
                return (this - other).Length();
            }
            public int Distance(){
                return this.Distance(new PointMultiDim(_coords.Select(i => 0)));
            }

            public virtual IEnumerable<PointMultiDim> Adjacent(bool corners = false) {
                if (corners) {
                    IEnumerable<int[]> offsets = new int[][] {new int[Dimension]};
                    int[][] Split3(int[] list, int coord) {
                        int[] l1 = (int[])list.Clone();
                        int[] l3 = (int[])list.Clone();
                        l1[coord]--;
                        l3[coord]++;
                        return Arr(l1, (int[])list.Clone(), l3);
                    }
                    foreach (int coord in IntRange(Dimension)) {
                        offsets = offsets.SelectMany(Split3);
                    }
                    foreach (int[] offset in offsets) {
                        if (!offset.All(i => i == 0)) {
                            yield return new PointMultiDim(
                                _coords.ZipApply(offset, (i, j) => i + j)
                            );
                        }
                    }

                }
                else {
                    foreach (int coord in IntRange(_coords.Length)) {
                        yield return new PointMultiDim(
                            _coords.Select((val, index) => val + (index == coord).ToInt())
                        );
                        yield return new PointMultiDim(
                            _coords.Select((val, index) => val - (index == coord).ToInt())
                        );
                    }
                }
            }

            public static VectorMultiDim operator - (PointMultiDim left, PointMultiDim right) {
                if (
                    left.Dimension != right.Dimension
                ) {
                    throw new ArgumentException("Cannot sum Vectors with different dimensions");
                }

                return new VectorMultiDim(
                    left.Coordinates.ZipApply(
                        right.Coordinates,
                        (l, r) => l - r
                    )
                );
            }
            public static PointMultiDim operator + (PointMultiDim left, VectorMultiDim right) {
                if (
                    left.Dimension != right.Dimension
                ) {
                    throw new ArgumentException("Cannot sum Vectors with different dimensions");
                }

                return new PointMultiDim(
                    left.Coordinates.ZipApply(
                        right.Components,
                        (l, r) => l + r
                    )
                );
            }
            public static PointMultiDim operator - (PointMultiDim left, VectorMultiDim right) {
                return left + (-right);
            }
            public static PointMultiDim operator + (VectorMultiDim left, PointMultiDim right) {
                return right + left;
            }
            public static bool operator == (PointMultiDim left, PointMultiDim right) {
                if (
                    left.Dimension != right.Dimension
                ) {
                    throw new ArgumentException("Cannot sum Vectors with different dimensions");
                }

                return left.Equals(right);
            }
            public static bool operator != (PointMultiDim left, PointMultiDim right) {
                return !(left == right);
            }
        }

    }

}