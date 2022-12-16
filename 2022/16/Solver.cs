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
    using Mapping = Dictionary<string, List<string>>;
    using Edges = Dictionary<string, int>;

    public static class Common {
        static Logger logger = new Logger(writeToFile: false);
        public static void Log(params string[] args) => logger.Log(args);
        public static void Log(params object[] args) => logger.Log(args);
        public static void LogEnum<T>(IEnumerable<T> en) => logger.LogEnum(en);
        
        public static Edges Dijkstra_Graph(
            string start,
            Mapping adjacents
        ){
            Edges allNodes = new();
            PriorityQueue<string, int> toVisit = new();
            allNodes.Add(start, 0);
            toVisit.Enqueue(start, 0);

            while (!toVisit.Empty()) {
                string current = toVisit.Dequeue();
                int newd = allNodes[current] + 1;
                foreach( string adj in adjacents[current]) {
                    if (!allNodes.ContainsKey(adj) || allNodes[adj] > newd) {
                        allNodes[adj] = newd;
                        toVisit.Enqueue(adj, newd);
                    }
                }
            }
            return allNodes;
        }
        
        public static int FindBestPath(
            Dictionary<string, Edges> allEdges, 
            Dictionary<string, int> flows,
            IEnumerable<string> visited,
            string start = "AA",
            int currTime = 30,
            int currScore = 0,
            int currFlow = 0
        ) {
            Edges currEdges = allEdges[start];
            if (currTime <= 0) {
                return currScore + currFlow * currTime;
            }
            if (currEdges.Keys.Except(visited).Empty()) {
                return currScore + currFlow * currTime;
            }
            currFlow = currFlow +  flows[start];
            List<int> newScores = new(Arr(currScore));
            foreach (var kv in currEdges) {
                string current = kv.Key;
                int timeTo = kv.Value + 1;
                if (!current.IsIn(visited)) {
                    int flow = flows[start];
                    int newScore = FindBestPath(
                        allEdges,
                        flows,
                        visited.Append(current),
                        start: current,
                        currTime: currTime - timeTo,
                        currScore: currScore + timeTo * currFlow,
                        currFlow: currFlow
                    );
                    newScores.Add(newScore);
                }
            }
            return newScores.Max();
        }

        public static int FindBestPathTwoAgents(
            Dictionary<string, Edges> allEdges, 
            Dictionary<string, int> flows,
            IEnumerable<string> visited,
            // string start1 = "AA",
            // string start2 = "AA",
            string target1 = "AA",
            string target2 = "AA",
            int timeTo1 = 0,
            int timeTo2 = 0,
            int timeToEnd = 26,
            int currScore = 0,
            int currFlow = 0,
            int maxReached = 0
        ) {
            // Log(target1, target2, timeToTarget1, timeToTarget2);
            // Log("currtime:", currTime, "score:", currScore, "flow:", currFlow, "max:", maxReached);
            /// If the time to the end is 0, we are at the end, so the score is the one we received
            if (timeToEnd == 0) {
                return currScore;
            }
            /// CurrTime is > 0, and we did not reach any of the targets
            if (timeTo1 > 0 && timeTo2 > 0) {
                /// We can go only for the minimum of these three values before hitting a roadblock
                int m = Arr(timeTo1, timeTo2, timeToEnd).Min();
                /// In m minutes, all times diminish by m
                /// and the score grows by flow * m
                /// Everything else dows not change
                return FindBestPathTwoAgents(
                    allEdges: allEdges,
                    flows: flows,
                    visited: visited,
                    target1: target1,
                    target2: target2,
                    timeTo1: timeTo1 - m,
                    timeTo2: timeTo2 - m,
                    timeToEnd: timeToEnd - m,
                    currScore: currScore + currFlow * m,
                    currFlow: currFlow,
                    maxReached: maxReached
                );
            }

            /// Start of the extimate part
            /// The optimistic extimate is
            /// score
            /// + flow * time to end
            /// + flow t1 * (time to end - time to t1)
            /// + flow t2 * (time to end - time to t2)
            /// + flow unexplored * (time to end - minimum time to reach unexplored)
            var unexploredFlows = flows.Where(kv => !kv.Key.IsIn(visited));
            var unexploredEdgesBy1 = allEdges[target1].Where(
                kv => !kv.Key.IsIn(visited)
            );
            var timesAvailableBy1 = unexploredEdgesBy1.Select(
                kv => (Key: kv.Key, Value: timeToEnd - timeTo1 - kv.Value)
            ).Where(
                kv => kv.Value > 0
            );
            var unexploredEdgesBy2 = allEdges[target2].Where(
                kv => !kv.Key.IsIn(visited)
            );
            var timesAvailableBy2 = unexploredEdgesBy2.Select(
                kv => (Key: kv.Key, Value: timeToEnd - timeTo2 - kv.Value)
            ).Where(
                kv => kv.Value > 0
            );
            Counter<string> maxTimesAvailable = new Counter<string>();
            foreach (var kv in timesAvailableBy1) {
                maxTimesAvailable[kv.Key] = kv.Value;
            }
            foreach (var kv in timesAvailableBy2) {
                maxTimesAvailable[kv.Key] = Max(maxTimesAvailable[kv.Key], kv.Value);
            }

            int sumUnexploredFlows = maxTimesAvailable.Join(
                unexploredFlows,
                kv => kv.Key,
                kv => kv.Key,
                (kv1, kv2) => kv1.Value * kv2.Value
            ).Sum();

            int estimate = currScore + timeToEnd * currFlow
            + sumUnexploredFlows
            + (timeToEnd - timeTo1) * (timeToEnd >= timeTo1).ToInt() * flows[target1]
            + (timeToEnd - timeTo2) * (timeToEnd >= timeTo2).ToInt() * flows[target2];
            // Log(estimate);
            if (estimate < maxReached) {
                return 0;
            }


            List<int> possibilities = new();
            if (timeTo1 == 0 && timeTo2 == 0) {
                currFlow += flows[target1] + flows[target2];
                foreach(var t in 
                    from kv1 in allEdges[target1]
                    join kv2 in allEdges[target2]
                    on 1 equals 1
                    where !kv1.Key.IsIn(visited)
                        && !kv2.Key.IsIn(visited)
                        && kv1.Key != kv2.Key
                    orderby flows[kv1.Key] + flows[kv2.Key] descending
                    select (kv1, kv2)
                ) {
                    string t1 = t.kv1.Key;
                    string t2 = t.kv2.Key;
                    int n = FindBestPathTwoAgents(
                        allEdges, flows, visited.Append(t1).Append(t2),
                        t1, t2, t.kv1.Value + 1, t.kv2.Value + 1,
                        timeToEnd, currScore, currFlow, maxReached
                    );
                    maxReached = Max(maxReached, n);
                    possibilities.Add(n);
                }
                if (possibilities.Empty()) {
                    possibilities.Add(
                        FindBestPathTwoAgents(
                            allEdges, flows, visited, target1, target2,
                            timeTo1, 900,
                            timeToEnd, currScore, currFlow - flows[target1], maxReached
                        )
                    );
                }
            }
            else if (timeTo1 == 0) {
                currFlow += flows[target1];
                foreach(
                    var kv in allEdges[target1]
                        .Where(kv => !visited.Contains(kv.Key))
                        .OrderByDescending(kv => flows[kv.Key])
                ) {
                    int n = FindBestPathTwoAgents(
                        allEdges, flows, visited.Append(kv.Key),
                        kv.Key, target2, kv.Value + 1, timeTo2,
                        timeToEnd, currScore, currFlow, maxReached
                    );
                    maxReached = Max(maxReached, n);
                    possibilities.Add(n);
                }
                if (possibilities.Empty()) {
                    possibilities.Add(
                        FindBestPathTwoAgents(
                            allEdges, flows, visited, target1, target2,
                            900, timeTo2,
                            timeToEnd, currScore, currFlow, maxReached
                        )
                    );
                }
            }
            else if (timeTo2 == 0) {
                currFlow += flows[target2];
                foreach(
                    var kv in allEdges[target2]
                        .Where(kv => !visited.Contains(kv.Key))
                        .OrderByDescending(kv => flows[kv.Key])
                ) {
                    int n = FindBestPathTwoAgents(
                        allEdges, flows, visited.Append(kv.Key),
                        target1, kv.Key, timeTo1, kv.Value + 1,
                        timeToEnd, currScore, currFlow, maxReached
                    );
                    maxReached = Max(maxReached, n);
                    possibilities.Add(n);
                }
                if (possibilities.Empty()) {
                    possibilities.Add(
                        FindBestPathTwoAgents(
                            allEdges, flows, visited, target1, target2,
                            timeTo1, 900,
                            timeToEnd, currScore, currFlow, maxReached
                        )
                    );
                }
            }
            return possibilities.Max();
        }
    
    }

    public static class Part1 {
        public static string solve(bool useExample) {
            
            Input reader = new Input(useExample: useExample);

            string result = "";
            int resultInt = 0;

            var tokenList = reader.ReadTokens();
            var flowList = reader.ReadInts();

            Mapping mapping = new();
            Dictionary<string, int> flows = new();

            foreach (var t in tokenList.Zip(flowList)) {
                string current = t.First[1];
                int flow = t.Second[0];
                List<string> adjs = t.First.Skip(9).Select(s => s.Trim(',')).ToList();
                mapping[current] = adjs;
                flows[current] = flow;
            }

            Dictionary<string, Edges> RelevantEdges = new();
            Edges DijkstraEdges;
            string[] relevantNodes = flows.Where(kv => kv.Value > 0).Select(kv => kv.Key).ToArray();

            foreach(string node in relevantNodes.Append("AA")) {
                DijkstraEdges = Dijkstra_Graph(node, mapping);
                RelevantEdges[node] = new();
                foreach (var kv in DijkstraEdges) {
                    if (relevantNodes.Contains(kv.Key) && kv.Key != node) {
                        RelevantEdges[node].Add(kv.Key, kv.Value);
                    }
                }
            }

            resultInt = FindBestPath(RelevantEdges, flows, new List<string>());
            

            


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

            var tokenList = reader.ReadTokens();
            var flowList = reader.ReadInts();

            Mapping mapping = new();
            Dictionary<string, int> flows = new();

            foreach (var t in tokenList.Zip(flowList)) {
                string current = t.First[1];
                int flow = t.Second[0];
                List<string> adjs = t.First.Skip(9).Select(s => s.Trim(',')).ToList();
                mapping[current] = adjs;
                flows[current] = flow;
            }

            Dictionary<string, Edges> RelevantEdges = new();
            Edges DijkstraEdges;
            string[] relevantNodes = flows.Where(kv => kv.Value > 0).Select(kv => kv.Key).ToArray();

            foreach(string node in relevantNodes.Append("AA")) {
                DijkstraEdges = Dijkstra_Graph(node, mapping);
                RelevantEdges[node] = new();
                foreach (var kv in DijkstraEdges) {
                    if (relevantNodes.Contains(kv.Key) && kv.Key != node) {
                        RelevantEdges[node].Add(kv.Key, kv.Value);
                    }
                }
            }

            resultInt = FindBestPathTwoAgents(
                RelevantEdges,
                flows,
                new List<string>()
            );


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }

}