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
        
        public static void MoveNode(LinkedListNode<long> node) {
            LinkedList<long> list =
                node.List
                ?? throw new ArgumentException("node not in list");

            LinkedListNode<long> after =
                node.Previous
                ?? list.Last
                ?? throw new ArgumentException("list is empty");

            list.Remove(node);

            after = Scroll(after, node.Value);
            list.AddAfter(after, node);
        }

        public static void MoveNodes(IEnumerable<LinkedListNode<long>> seq) {
            foreach(var node in seq) {
                MoveNode(node);
            }
        }

        public static LinkedListNode<long> Scroll(
            LinkedListNode<long> node,
            long k
        ) {
            LinkedList<long> list =
                node.List
                ?? throw new ArgumentException("node not in list");

            int n = Mod(k, list.Count);
            foreach (int i in IntRange(n)) {
                node =
                    node.Next
                    ?? list.First
                    ?? throw new ArgumentException("list is empty");
            }
            return node;
        }

    }

    public static class Part1 {
        public static string solve(bool useExample) {
            
            Input reader = new Input(useExample: useExample);

            string result = "";
            long resultInt = 0;

            LinkedList<long> ElvesData = new();
            LinkedListNode<long> zeroNode = new(0);
            List<LinkedListNode<long>> nodes = new();
            var lines = reader.ReadLines();

            foreach (string l in lines) {
                long n = l.ToLong();
                var newNode = ElvesData.AddLast(n);
                nodes.Add(newNode);
                if (n == 0) {
                    zeroNode = newNode;
                }
            }

            MoveNodes(nodes);

            resultInt = (
                Scroll(zeroNode, 1000).Value
                + Scroll(zeroNode, 2000).Value
                + Scroll(zeroNode, 3000).Value
            );


            


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }



    public static class Part2 {
        public static string solve(bool useExample) {

            Input reader = new Input(useExample: useExample);
            
            string result = "";
            long resultInt = 0;

            LinkedList<long> ElvesData = new();
            LinkedListNode<long> zeroNode = new(0);
            List<LinkedListNode<long>> nodes = new();
            var lines = reader.ReadLines();

            long DecryptConstant = 811589153L;

            foreach (string l in lines) {
                long n = l.ToLong();
                var newNode = ElvesData.AddLast(n * DecryptConstant);
                nodes.Add(newNode);
                if (n == 0) {
                    zeroNode = newNode;
                }
            }

            foreach (int i in IntRange(10)) {
                MoveNodes(nodes);
            }

            resultInt = (
                Scroll(zeroNode, 1000).Value
                + Scroll(zeroNode, 2000).Value
                + Scroll(zeroNode, 3000).Value
            );


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }

}