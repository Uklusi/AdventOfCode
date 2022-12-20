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

    public class Board {
        public int piece;
        public int jetIndex;
        public bool[] jetRight;

        /// board[0] is the bottomest row, should be all true
        /// Highest row should be board[^1]
        public List<bool[]> board;
        public int maxH;
        public static int emptyLinesOnTop = 10;

        public Board(string jetSeq) {
            piece = 0;
            jetIndex = 0;
            jetRight = jetSeq.Select(c => (c == '>')).ToArray();
            board = new();
            maxH = 0;

            bool[] row = Repeat(true, 7).ToArray();
            board.Add(row.ToArray());
            AddEmptyRows(Board.emptyLinesOnTop);
        }
        public int FromTopRock(int i) {
            return board.Count - Board.emptyLinesOnTop + i;
        }

        public bool CheckCollision(int y, int x) {
            if (!x.IsInInterval(0, 6)) {
                return true;
            }


            return board[FromTopRock(y)][x];
        }

        public static Vector[][] pieces = 
        Arr(
            Arr(
                new Vector(0,0),
                new Vector(1,0),
                new Vector(2,0),
                new Vector(3,0)
            ),
            Arr(
                new Vector(0,1),
                new Vector(1,0),
                new Vector(1,1),
                new Vector(1,2),
                new Vector(2,1)
            ),
            Arr(
                new Vector(0,0),
                new Vector(1,0),
                new Vector(2,0),
                new Vector(2,1),
                new Vector(2,2)
            ),
            Arr(
                new Vector(0,0),
                new Vector(0,1),
                new Vector(0,2),
                new Vector(0,3)
            ),
            Arr(
                new Vector(0,0),
                new Vector(0,1),
                new Vector(1,0),
                new Vector(1,1)
            )
        );

        public static Vector[][][] piecesRelativeBorders = pieces.Select(
            vList => {
                var moved = Arr(
                    vList.Select(v => v + new Vector(-1, 0)),
                    vList.Select(v => v + new Vector(1, 0)),
                    vList.Select(v => v + new Vector(0, -1))
                );
                return moved.Select(wList => wList.Except(vList).ToArray()).ToArray();
            }
        ).ToArray();

        public void AddRow() => board.Add(Repeat(false, 7).ToArray());

        public bool IsMovePossible(IEnumerable<Vector> posList, Point bottomRight) {
            return posList.All(
                v => {
                    Point p = bottomRight + v;
                    return !CheckCollision(p.Y, p.X);
                }
            );
        }

        public void AdvancePiece() {
            piece = (piece + 1) % Board.pieces.Count();
        }
        public void AdvanceJet() {
            jetIndex = (jetIndex + 1) % jetRight.Count();
        }

        public void AddEmptyRows(int indexY) {
            foreach(int i in IntRange(indexY)) {
                AddRow();
            }
        }

        public void ResizeBoard(int rowsToAdd, HashSet<int> completedRows) {
            int c = board.Count();
            // if (! completedRows.Empty()) {
            //     List<bool[]> newBoard = new();
            //     foreach(int i in IntRange(completedRows.Max(), c) ) {
            //         newBoard.Add(board[i]);
            //     }
            //     board = newBoard;
            // }
            int d = c - 1000;
            if (c > 1000) {
                List<bool[]> newBoard = new();
                foreach(int i in IntRange(d, c) ) {
                    newBoard.Add(board[i]);
                }
                board.Clear(); // GC you should work without me telling you this WHYYYYY
                board = newBoard;
            }
            AddEmptyRows(rowsToAdd);
        }

        public void AddPiece() {
            Point PiecePos = new Point(2, 3);
            bool bonked = false;
            while (!bonked) {
                bool currJet = jetRight[jetIndex];
                AdvanceJet();
                if (IsMovePossible(
                    Board.piecesRelativeBorders[piece][currJet.ToInt()], PiecePos
                )) {
                    PiecePos += new Vector(currJet ? 1 : -1, 0);
                }
                bonked = !IsMovePossible(
                    Board.piecesRelativeBorders[piece][2], PiecePos
                );
                if (!bonked) {
                    PiecePos += new Vector(0, -1);
                }
            }

            IEnumerable<Point> piecePoints = Board.pieces[piece].Select(v => PiecePos + v);
            int numRowsToAdd = 0;
            HashSet<int> completedRows = new();
            foreach (var p in piecePoints) {
                numRowsToAdd = Max(numRowsToAdd, p.Y + 1);
                board[FromTopRock(p.Y)][p.X] = true;
                if (board[FromTopRock(p.Y)].All()) {
                    completedRows.Add(FromTopRock(p.Y));
                }
            }
            maxH += numRowsToAdd;
            ResizeBoard(numRowsToAdd, completedRows);
            AdvancePiece();
        }

        public Grid2D<char> ToGrid() {
            return Grid2D<bool>.FromOtherType<bool, char>(
                board.AsEnumerable().Reverse(),
                b => b ? FULL : PATH
            );
        }
    }

    public static class Part1 {
        public static string solve(bool useExample) {
            
            Input reader = new Input(useExample: useExample);

            string result = "0";
            int resultInt = 0;

            string input = reader.Read();

            Board board = new Board(input);

            foreach (int i in IntRange(2022)) {
                board.AddPiece();
            }

            resultInt = board.maxH;

            


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
            int resultInt = 0;

            long iHateMyLife = 1000000000000L;

            string input = reader.Read();

            Board board = new Board(input);
            long afterPeriod = 0;
            long maxH = 0;

            Dictionary<string, (long, long)> seen = new();
            for (long i = 0; ; i++) {
                if (i % 10000 == 0) {
                }
                
                if (i % 5 == 0) {
                    string check = board.jetIndex.ToString() + board.ToGrid().ToString();
                    if (seen.ContainsKey(check)) {
                        long start = seen[check].Item1;
                        long period = i - start;
                        long hGrowth = board.maxH - seen[check].Item2;
                        long coveredByCycles = iHateMyLife - start;
                        long numCycles = coveredByCycles / period;
                        maxH = seen[check].Item2 + numCycles * hGrowth;
                        afterPeriod = coveredByCycles % period;
                        break;

                    }
                    seen[check] = (i, board.maxH);
                }
                board.AddPiece();
                
            }
            board.maxH = 0;
            for (long i = 0; i < afterPeriod; i++) {
                board.AddPiece();
            }

            result = (board.maxH + maxH).ToString();
            


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }

}