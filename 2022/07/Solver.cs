using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static System.Math;

using AoCUtils;
using AoCUtils.GridUtils;
using static AoCUtils.Constants;
using static AoCUtils.Functions;

namespace AoC {

    public static class Common {

    }

    public class TreeNode {
        public List<TreeNode> children;
        public string Name;
        public int DirSize;
        public int ActualSize;
        public TreeNode(string s){
            children = new();
            Name = s;
            DirSize = 0;
            ActualSize = 0;
        }

        public static TreeNode ReadTree(List<string> inputData) {
            TreeNode root = new TreeNode("/");
            TreeNode current = root;
            Stack<TreeNode> stack = new();

            foreach (var line in inputData) {
                if (line == "$ cd /") {
                    stack.Clear();
                    current = root;
                }
                else if (
                    line == "$ ls"
                    || line.StartsWith("dir")
                ) {
                    continue;
                }
                else if (line == "$ cd ..") {
                    current = stack.Pop();
                }
                else if (line.StartsWith("$ cd ")) {
                    stack.Push(current);
                    string nodeName = line[5..];
                    TreeNode newCurr;
                    try {
                        newCurr = current.children.Where(n => n.Name == nodeName).First();
                    }
                    catch {
                        newCurr = new(nodeName);
                        current.children.Add(newCurr);
                    }

                    current = newCurr;
                }
                else {
                    int size = line.Split(" ")[0].ToInt();
                    current.DirSize += size;
                }
            }

            root.SetActualSize();
            
            return root;
        }
        
        public int SetActualSize() {
            ActualSize = DirSize;
            foreach (TreeNode child in children) {
                ActualSize += child.SetActualSize();
            }
            return ActualSize;
        }
    }

    public static class Part1 {

        static Logger logger = new Logger("Part1", writeToFile: false);

        public static string solve(bool useExample) {
            
            var inputReader = new Input(useExample: useExample);

            string result = "";
            int resultInt = 0;

            var inputData = inputReader.ReadLines();

            TreeNode root = TreeNode.ReadTree(inputData);

            int GetResult(TreeNode node) {
                int TmpResult = 0;
                if (node.ActualSize <= 100000) {
                    TmpResult += node.ActualSize;
                }
                foreach (TreeNode child in node.children) {
                    TmpResult += GetResult(child);
                }
                return TmpResult;
            }

            resultInt = GetResult(root);

            
            


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            logger.Close();
            return result;
        }
    }



    public static class Part2 {

        static Logger logger = new Logger("Part2", writeToFile: false);

        public static string solve(bool useExample) {

            var inputReader = new Input(useExample: useExample);
            var inputData = inputReader.ReadLines();
            
            string result = "";
            int resultInt = 0;

            TreeNode root = TreeNode.ReadTree(inputData);
            int neededSize = 30_000_000 + root.ActualSize - 70_000_000;

            TreeNode? FindSmallestBiggest(TreeNode node, int neededSize){
                if (node.ActualSize < neededSize) {
                    return null;
                }
                else {
                    TreeNode candidate = node;
                    foreach (TreeNode child in node.children) {
                        TreeNode? ret = FindSmallestBiggest(child, neededSize);
                        if (candidate.ActualSize > (ret?.ActualSize ?? int.MaxValue)) {
                            candidate = ret ?? candidate;
                        }
                    }
                    return candidate;
                }
            }

            TreeNode? solution = FindSmallestBiggest(root, neededSize);
            resultInt = solution?.ActualSize ?? -1;



            


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            logger.Close();
            return result;
        }
    }

}