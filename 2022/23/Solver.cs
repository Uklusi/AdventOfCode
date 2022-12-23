using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Immutable;
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
        static Logger logger = new Logger(writeToFile: true);
        public static void Log(params string[] args) => logger.Log(args);
        public static void Log(params object[] args) => logger.Log(args);
        public static void Log() => logger.Log("");
        public static void LogEnum<T>(IEnumerable<T> en) => logger.LogEnum(en);
    }

    public class ElvesMovement {
        HashSet<Point> Positions;
        Direction[] CheckOrder;

        public ElvesMovement(List<string> map) {
            CheckOrder = Arr(
                new Direction("N"),
                new Direction("S"),
                new Direction("W"),
                new Direction("E")
            );
            Positions = new();
            foreach ((int y, string line) in map.Enumerate()) {
                foreach ((int x, char c) in line.Enumerate()) {
                    if (c == '#') {
                        Positions.Add(new Point(x, y));
                    }
                }
            }
        }

        public void Step() {
            Dictionary<Point, Point> Proposals = new();
            HashSet<Point> CollidedOrIsolated = new();
            HashSet<Point> HasCollision = new();
            
            foreach (Point p in Positions) {
                ImmutableHashSet<Point> adj = p.Adjacent(corners: true).ToImmutableHashSet();
                ImmutableHashSet<Point> adjNeighs = adj.Intersect(Positions);
                if (adjNeighs.Empty()) {;
                    CollidedOrIsolated.Add(p);
                }
                else {
                    bool isAllOccupied = true;
                    foreach (Direction d in CheckOrder) {
                        Vector v0 = Vector.FromDirection(d, upIsNegative: true);
                        Vector w = v0.Rotate(1);
                        Vector v1 = v0 + w;
                        Vector v2 = v0 - w;
                        if (
                            Arr(v0, v1, v2)
                                .Select(v => v + p)
                                .Intersect(adjNeighs)
                                .Empty()
                        ) {
                            isAllOccupied = false;
                            Point proposed = v0 + p;
                            if (HasCollision.Contains(proposed)) {
                                CollidedOrIsolated.Add(p);
                            }
                            else if (Proposals.ContainsKey(proposed)) {
                                CollidedOrIsolated.Add(Proposals[proposed]);
                                CollidedOrIsolated.Add(p);
                                HasCollision.Add(proposed);
                                Proposals.Remove(proposed);
                            }
                            else {
                                Proposals[proposed] = p;
                            }
                            break;
                        }
                    }
                    if (isAllOccupied) {
                        CollidedOrIsolated.Add(p);
                    }
                }
            }

            Positions.Clear();
            foreach (var proposition in Proposals) {
                Positions.Add(proposition.Key);
            }
            foreach (Point stopped in CollidedOrIsolated) {
                Positions.Add(stopped);
            }
            CollidedOrIsolated.Clear();
            HasCollision.Clear();
            Proposals.Clear();

            CheckOrder = CheckOrder[1..^0].Append(CheckOrder[0]).ToArray();
        }

        public (int MinX, int MaxX, int MinY, int MaxY) GetSize() {
            IEnumerable<int> CoordsX = Positions.Select(p => p.X);
            IEnumerable<int> CoordsY = Positions.Select(p => p.Y);
            return (CoordsX.Min(), CoordsX.Max() + 1, CoordsY.Min(), CoordsY.Max() + 1);
        }

        public Grid2D<char> PrintMap() {
            List<string> lines = new();
            var sizes = GetSize();
            foreach (int y in IntRange(sizes.MinY, sizes.MaxY)) {
                lines.Add(
                    IntRange(sizes.MinX, sizes.MaxX)
                        .Select(x => Positions.Contains(new Point(x, y)))
                        .Select(b => b ? FULL : PATH)
                        .JoinString()
                );
            }
            return new Grid2D<char>(lines);
        }

        public int Run(int steps) {
            // Log(PrintMap());
            // Log("");
            foreach (int i in IntRange(steps)) {
                Step();
                // Log(PrintMap());
                // Log("");
            }

            IEnumerable<int> CoordsX = Positions.Select(p => p.X);
            IEnumerable<int> CoordsY = Positions.Select(p => p.Y);
            int rectCount = (CoordsX.Max() - CoordsX.Min() + 1) * (CoordsY.Max() - CoordsY.Min() + 1);
            return rectCount - Positions.Count;
            
        }

        public int StepsUntilNoMove() {
            var OldPositions = Positions.ToImmutableHashSet();
            for (int i = 1;;i++) {
                Step();
                if (OldPositions.SetEquals(Positions)) {
                    return i;
                }
                OldPositions = Positions.ToImmutableHashSet();
            }
        }
    }

    public static class Part1 {
        public static string solve(bool useExample) {
            
            Input reader = new Input(useExample: useExample);

            string result = "0";
            long resultInt = 0;

            var map = reader.ReadLines();

            ElvesMovement solver = new(map);

            resultInt = solver.Run(10);
            



            


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
            
            var map = reader.ReadLines();

            ElvesMovement solver = new(map);

            resultInt = solver.StepsUntilNoMove();

            


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }

}