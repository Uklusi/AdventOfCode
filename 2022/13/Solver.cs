using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Diagnostics;
using static System.Math;

using AoCUtils;
using AoCUtils.GridUtils;
using static AoCUtils.Constants;
using static AoCUtils.Functions;

namespace AoC {
    using static Common;

    public static class Common {
        static Logger logger = new Logger(writeToFile: true);
        public static void Log(params string[] args) => logger.Log(args);
        public static void Log(params object[] args) => logger.Log(args);
        public static void LogEnum<T>(IEnumerable<T> en) => logger.LogEnum(en);

        public static string ReplaceAt(this string self, int start, int len, string replacement){
            return self.Remove(start, Min(len, self.Length - start)).Insert(start, replacement);
        }
        
        public static Regex re = new Regex(@"\d+");

        public static (string, int) ReadNum(string s, int n) {
            Match m = re.Match(s, n);
            return (m.Value, m.Length);
        }   

    }

    public class Packet {}
    
    public class PacketElem : Packet {
        public int val;
        public PacketElem(int n) {
            val = n;
        }
    }

    public class PacketList : Packet {
        public List<Packet> elems;

        public PacketList() {
            elems = new();
        }
        public PacketList(params Packet[] p) {
            elems = new();
            elems.AddRange(p);
        }

        public static PacketList Parse(string s) {
            Stack<PacketList> stack = new();
            Regex re = new Regex(@"\[|\]|,|\d+");
            List<string> tokens = re.Matches(s).Select(m => m.Value).ToList();
            PacketList lastPopped = new();
            foreach (string token in tokens) {
                switch (token) {
                    case "[":
                        stack.Push(new PacketList());
                        break;
                    case "]":
                        PacketList closed = stack.Pop();
                        if (stack.Count > 0) {
                            stack.Peek().elems.Add(closed);
                        }
                        else {
                            lastPopped = closed;
                        }
                        break;
                    case ",":
                        break;
                    default:
                        int n = token.ToInt();
                        stack.Peek().elems.Add(new PacketElem(n));
                        break;
                }
            }
            return lastPopped;
        }
    }

    public class PacketComparer : Comparer<Packet> {
        public override int Compare(Packet? x, Packet? y)
        {
            if (x is null || y is null) {
                return (y is null).ToInt() - (x is null).ToInt();
            }
            if (x is PacketList && y is PacketList) {
                PacketList xlist = x as PacketList ?? throw new UnreachableException();
                PacketList ylist = y as PacketList ?? throw new UnreachableException();
                foreach (var (a, b) in xlist.elems.Zip(ylist.elems)) {
                    int comparison = Compare(a, b);
                    if (comparison != 0) {
                        return comparison;
                    }
                }
                return xlist.elems.Count - ylist.elems.Count;
            }
            else if (x is PacketList && y is PacketElem) {
                PacketList xlist = x as PacketList ?? throw new UnreachableException();
                PacketElem yelem = y as PacketElem ?? throw new UnreachableException();
                return Compare(xlist, new PacketList(yelem));
            }
            else if (x is PacketElem && y is PacketList) {
                PacketElem xelem = x as PacketElem ?? throw new UnreachableException();
                PacketList ylist = y as PacketList ?? throw new UnreachableException();
                return Compare(new PacketList(xelem), ylist);
            }
            else if (x is PacketElem && y is PacketElem) {
                PacketElem xelem = x as PacketElem ?? throw new UnreachableException();
                PacketElem yelem = y as PacketElem ?? throw new UnreachableException();
                return xelem.val.CompareTo(yelem.val);
            }
            else {
                throw new UnreachableException();
            }
        }
    }

    public static class Part1 {
        public static string solve(bool useExample) {
            
            Input reader = new Input(useExample: useExample);

            string result = "";
            int resultInt = 0;

            var PacketsInput = reader.ReadParagraphs();
            
            var PacketComparer = new PacketComparer();

            foreach ((int i, var packets) in PacketsInput.Enumerate()) {
                PacketList x = PacketList.Parse(packets[0]);
                PacketList y = PacketList.Parse(packets[1]);

                if (PacketComparer.Compare(x, y) < 0){
                    resultInt += i+1;
                }
            }

            
            


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
            int resultInt = 0;

            List<PacketList> data = reader.ReadParagraphs().Flatten().Select(s => PacketList.Parse(s)).ToList();
            
            PacketList p1 = PacketList.Parse("[[2]]");
            PacketList p2 = PacketList.Parse("[[6]]");

            data.Add(p1);
            data.Add(p2);

            data.Sort(new PacketComparer());

            resultInt = (data.IndexOf(p1) + 1) * (data.IndexOf(p2) + 1);


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }

}