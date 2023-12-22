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

    public static class Common {
        static Logger logger = new Logger(writeToFile: false);
        public static void Log(params string[] args) => logger.Log(args);
        public static void Log(params object[] args) => logger.Log(args);
        public static void LogEnum<T>(IEnumerable<T> en) => logger.LogEnum(en);
        
        

    }

    public static class Part1 {
        public static string solve(bool useExample) {
            
            Input reader = new Input(useExample: useExample);

            string result = "0";
            long resultInt = 0;

            var data = reader.ReadLines();
            Grid2D<char> grid = new(data);
            MapPoint.SetFrame(data);
            MapPoint.SetOccupied(p => grid[p] == '#');
            Point startP = data.Enumerate2D().Where(t => t.item == 'S').First().p;
            MapPoint start = new(startP.X, startP.Y);

            var distances = MazeFunctions.Dijkstra(start);

            int steps = useExample ? 6 : 64;

            resultInt = distances.Values.Where(i => i <= steps).Where(i => i % 2 == 0).Count();


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }



    public static class Part2 {
        public static string solve(bool useExample) {

            Input reader = new Input(useExample: useExample);
            
            string result = "0";
            long resultInt = 0;

            // I'm using crazy assumptions that are not true AT ALL for the example
            var data = reader.ReadLines();
            Grid2D<char> grid = new(data);
            MapPoint.SetFrame(data);
            MapPoint.SetOccupied(p => grid[p] == '#');
            Point startP = data.Enumerate2D().Where(t => t.item == 'S').First().p;
            MapPoint start = new(startP.X, startP.Y);
            // Assumptions number 1 and 2: grid is square NxN, and N = 2m + 1
            int N = data.Count;
            int m = N / 2;
            if (data[0].Length != N || N != 2 * m + 1) {
                throw InPanic();
            }
            // Log($"m: {m}, N: {N}");
            // Assumption: the outer border, the middle row and column are all empty
            foreach (int i in IntRange(N)) {
                if (Arr(
                    new Point(i, 0),
                    new Point(i, m),
                    new Point(i, N - 1),
                    new Point(0, i),
                    new Point(m, i),
                    new Point(N - 1, i)
                ).Any(q => grid[q] == '#')) {
                    throw InPanic();
                }
            }

            MapPoint ul = new(0,0);
            MapPoint uu = new(m, 0);
            MapPoint ur = new(N - 1, 0);
            MapPoint ll = new(0, m);
            MapPoint rr = new(N - 1, m);
            MapPoint dl = new(0, N - 1);
            MapPoint dd = new(m, N - 1);
            MapPoint dr = new(N - 1, N - 1);

            Dictionary<MapPoint, Dictionary<Point, int>> distances = new();
            foreach (var p in Arr(start, ul, uu, ur, ll, rr, dl, dd, dr)) {
                distances[p] = MazeFunctions.Dijkstra(p);
            }
            long numReachable = distances[start].Values.Count;

            int CountParityAndCondition(MapPoint p, long parity, Func<int, bool> condition) {
                return distances[p].Values.Where(i => i % 2 == Mod(parity, 2)).Where(condition).Count();
            }
            int CountParity(MapPoint p, long parity) {
                return distances[p].Values.Where(i => i % 2 == Mod(parity, 2)).Count();
            }

            long numSameStartParity = CountParity(start, 0);
            long numOtherStartParity = CountParity(start, 1);
            // Log(sameParity, otherParity);

            // Assumption: steps >= 4m + 3 (this will be useful later)
            long steps = useExample ? 27 : 26501365;
            long stepsParity = steps % 2;

            // Log(ort, kort, mort);
            // Log(diag, kdiag, mdiag);

            // this is only here for debug, so I can log the change value
            long changeValue = 0;

            // Central tile section
            // Assumption: steps is odd
            resultInt += numOtherStartParity;
            // Log("centralChange", numOtherStartParity);

            // Orthogonal tiles section
            // In order to move orthogonally from start to (e.g.) the tile on the right's ll
            // we need m + 1 steps
            long ort = steps - (m + 1);
            // kort is the number of tiles we can move on the right starting by tile (1,0) ll
            long kOrt = ort / N;
            long remOrt = ort % N;

            // Assumption: m is odd.
            // Since orthogonal positions (ll, rr, uu, dd) dist m from start,
            // their parity is the opposite from start parity.
            long numSameOrtParity = numOtherStartParity;
            long numOtherOrtParity = numSameStartParity;

            // Since tile (1,0) ll is m+1 steps from start, it has the same parity of start
            // So for tile (1,0) we need to take the positions with different parity than ll.
            // Moving right one tile is N steps, so ll parity changes.
            // This means that we have floor(k/2) = (int)k/2 tiles where we take same parity than ll
            // and ceil(k/2) tiles where we take different parity than ll
            long a = kOrt / 2;
            changeValue = numSameOrtParity * a + numOtherOrtParity * (kOrt - a);
            // Log("hchange", changeValue);
            // The 4* is due to the fact that it is symmetrical wrt rotations
            resultInt += 4 * changeValue;

            // In the end, we need to account for the last tile (and the second to last)
            // We need to include the positions we can reach from the last ll
            // They need to be at max remOrt from the last ll
            // but we also need to remove the positions that are not reachable from the second to last ll
            // (they dist more than N + remOrt from that ll)
            // Here the assumption about the number of steps is useful:
            // in this case the second to last tile is at least (1,0), and cannot be (0,0)
            // and we are not removing too much
            foreach (var p in Arr(ll, rr, uu, dd)) {
                // Remember that if k == 0 the last ll is the one at (1,0), so we need to take
                // the positions with different parity than ll.
                changeValue = CountParityAndCondition(p, kOrt + 1, i => i <= remOrt);
                // Log("additiveChange", changeValue);
                resultInt += changeValue;
                changeValue = CountParityAndCondition(p, kOrt, i => i > N + remOrt);
                // Log("negativeChange", changeValue);
                resultInt -= changeValue;
            }
            // Log(resultInt);

            // Almost the same can be said for diagonal tiles.
            // In order to move in one of the 4 angle points of the next tile diagonally
            // (e.g. we move to (1,1)'s dl), we need 2 * (m + 1) steps
            long diag = steps - 2 * (m + 1);
            // kDiag is the number of tiles we can move on the right or up starting by tile (1,1) dl
            long kDiag = diag / N;
            long remDiag = diag % N;

            // Since diagonal positions (ul, ur, dl, dr) dist 2m from start,
            // their parity is the same than start parity.
            long numSameDiagParity = numSameStartParity;
            long numOtherDiagParity = numOtherStartParity;

            // Same idea of the orthogonal case, but the number of tiles change.
            // If b = ceil(k/2) and c = floor(k/2), we have that the number of tiles with same (1,1) dl parity
            // is 1 + 3 + ... + 2b-1 = b^2
            // (again, here we need to take positions with different parity than dl)
            // and the number of tiles with opposite (1,1) dl parity
            // is 2 + 4 + ... + 2c = 2 * (1+2+...+c) = 2*c(c+1)/2 = c(c+1)
            long b = (kDiag + 1) / 2;
            long c = kDiag / 2;
            changeValue = numOtherDiagParity * b * b + numSameDiagParity * c * (c + 1);
            // Log("fullDiagChange", changeValue);
            // Same reason for the 4*
            resultInt += 4 * changeValue;
            
            // Same reason for the correction term.
            // Here the hypotesis on the steps really is used at max, for the same reason:
            // we need 2m + 2 + N = 4m+3 steps to reach the second dl
            // Again we need to account for the fact that there are more terminal and almost-terminal tiles
            // (k+1 and k respectively) and for the fact that for the terminal we need to take
            // positions with parity opposite than k (k==0 means last is (1,1), where we need parity 1 from dl)
            foreach (var p in Arr(ul, ur, dl, dr)) {
                changeValue = CountParityAndCondition(p, kDiag + 1, i => i <= remDiag) * (kDiag + 1);
                resultInt += changeValue;
                // Log(changeValue);
                changeValue = CountParityAndCondition(p, kDiag, i => i > N + remDiag) * kDiag;
                resultInt -= changeValue;
                // Log(changeValue);
            }

            // My example is a toy case, with all positions empty.
            // In this case the result is the number of cells with odd parity with distance <= steps,
            // which are 4 * (1 + 3 + ... + steps) = 4 * s1 * s1
            if (useExample) {
                long s1 = steps / 2 + 1;
                Log(4 * s1 * s1);
            }



            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }

}