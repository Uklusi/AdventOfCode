using System.Collections;
using System.ComponentModel;
using System.Reflection.PortableExecutable;
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

    public class ElfComparer : Comparer<string> {
        public override int Compare(string? leftPar, string? rightPar) {
            string left = leftPar ?? "";
            string right = rightPar ?? "";
            int lpos = 0;
            int rpos = 0;
            Log(left, right);

            while (lpos != left.Length && rpos != right. Length) {
                char l = left[lpos];
                char r = right[rpos];
                // Log(l, r);

                if (l == ']' || r == ']') {
                    if (l == ']' && r == ']') {
                        lpos ++;
                        rpos++;
                        Log("Both lists closed, continuing");
                    }
                    else if (l == ']'){
                        Log($"Left closed when r continued, left < right");
                        return -1;
                    }
                    else {
                        Log($"R closed when l continued, left > right");
                        return 1;

                    }
                }
                else if (l == ',' && r == ',') {
                    lpos++;
                    rpos++;
                }
                else if (l == '[') {
                    if ( r == '[' ) {
                        lpos++;
                        rpos++;
                        Log("Both lists found");
                    }
                    else if (re.IsMatch(r.ToString())) {
                        (string n, int len) = ReadNum(right, rpos);
                        right = right.ReplaceAt(rpos, len, $"[{n}]");
                        Log($"Substituting {n} with [{n}] at right {rpos}");
                    }
                    else {
                        throw new Exception("This shouldn't happen");
                    }
                }
                else if (r == '[') {
                    if (re.IsMatch(l.ToString())) {
                        (string n, int len) = ReadNum(left, lpos);
                        left = left.ReplaceAt(lpos, len, $"[{n}]");
                        Log($"Substituting {n} with [{n}] at left {lpos}");
                    }
                    else {
                        throw new Exception("This shouldn't happen");
                    }
                }
                else {
                    //this should only be numeric
                    if (!re.IsMatch(l.ToString()) || !re.IsMatch(r.ToString())) {
                        throw new Exception("This shouldn't happen");
                    }
                    else {
                        (string lnum, int llen) = ReadNum(left, lpos);
                        (string rnum, int rlen) = ReadNum(right, rpos);
                        
                        Log($"Confronting {lnum} with {rnum}");
                        int ln = lnum.ToInt();
                        int rn = rnum.ToInt();
                        if (ln < rn) {
                            Log("left < right");
                            return -1;
                        }
                        else if (ln > rn) {
                            Log("left > right");
                            return 1;
                        }
                        else {
                            Log("left == right, continuing");
                            lpos += llen;
                            rpos += rlen;
                        }
                    }
                }

            }
            return 0;
        }
    }
    

    public static class Part1 {
        public static string solve(bool useExample) {
            
            Input reader = new Input(useExample: useExample);

            string result = "";
            int resultInt = 0;

            var PacketList = reader.ReadParagraphs();

            Log(PacketList.Count);
            
            var elfComparer = new ElfComparer();

            foreach ((int i, var packets) in PacketList.Enumerate()) {
                string left = packets[0];
                string right = packets[1];

                if (elfComparer.Compare(left, right) < 0){
                    resultInt += i+1;
                }

                Log(resultInt);
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

            List<string> data = reader.ReadParagraphs().Flatten().ToList();
            string s1 = "[[2]]";
            string s2 = "[[6]]";

            data.Add(s1);
            data.Add(s2);

            data.Sort(new ElfComparer());

            resultInt = (data.IndexOf(s1) + 1) * (data.IndexOf(s2) + 1);


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }

}