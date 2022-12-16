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
            ///Shouldn't matter to pass these immutable objects to the function
            /// C# Only copies the references
            Dictionary<string, Edges> allEdges,
            Dictionary<string, int> flows,
            ImmutableHashSet<string> visited,
            string[] targets,
            int[] timeTo,
            int timeToEnd = 26,
            int currScore = 0,
            int currFlow = 0,
            int maxReached = 0
        ) {
            /// If the time to the end is 0, we are at the end, so the score is the one we received
            if (timeToEnd == 0) {
                return currScore;
            }
            /// CurrTime is > 0, and we did not reach any of the targets
            if (timeTo[0] > 0 && timeTo[1] > 0) {
                /// We can go only for the minimum of these three values before hitting a roadblock
                int m = Arr(timeTo[0], timeTo[1], timeToEnd).Min();
                /// In m minutes, all times diminish by m
                /// and the score grows by flow * m
                /// Everything else dows not change
                return FindBestPathTwoAgents(
                    allEdges: allEdges,
                    flows: flows,
                    visited: visited,
                    targets: targets,
                    timeTo: Arr(timeTo[0] - m, timeTo[1] - m),
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
            /// + flow t0 * (time to end - time to t0)
            /// + flow t1 * (time to end - time to t1)
            /// + flow unexplored * (time to end - minimum time to reach unexplored)
            var unexploredFlows = flows.Where(kv => !kv.Key.IsIn(visited));

            var timesAvailableBy0 = allEdges[targets[0]].Where(
                kv => !kv.Key.IsIn(visited)
            ).Select(
                kv => (Key: kv.Key, Value: timeToEnd - timeTo[0] - kv.Value)
            ).Where(
                kv => kv.Value > 0
            );

            var timesAvailableBy1 = allEdges[targets[1]].Where(
                kv => !kv.Key.IsIn(visited)
            ).Select(
                kv => (Key: kv.Key, Value: timeToEnd - timeTo[1] - kv.Value)
            ).Where(
                kv => kv.Value > 0
            );

            /// Full join of the poors (if it works it works)
            Counter<string> maxTimesAvailable = new Counter<string>();
            foreach (var kv in timesAvailableBy0) {
                maxTimesAvailable[kv.Key] = kv.Value;
            }
            foreach (var kv in timesAvailableBy1) {
                maxTimesAvailable[kv.Key] = Max(maxTimesAvailable[kv.Key], kv.Value);
            }

            int sumUnexploredFlows = maxTimesAvailable.Join(
                unexploredFlows,
                kv => kv.Key,
                kv => kv.Key,
                (kv1, kv2) => kv1.Value * kv2.Value
            ).Sum();

            /// The Max is there to avoid situations in which TimeTo is bigger than TimeToEnd,
            /// Which will underestimate the final score
            int estimate = currScore + timeToEnd * currFlow
            + sumUnexploredFlows
            + Max(timeToEnd - timeTo[0], 0) * flows[targets[0]]
            + Max(timeToEnd - timeTo[1], 0) * flows[targets[1]];
            // Log(estimate);
            if (estimate < maxReached) {
                return 0;
            }


            List<int> possibilities = new();
            /// Case 1: Both 0 and 1 are activating a new flow in the same minute
            /// This is also the case for the first step of the algorithm,
            /// Where both start from AA
            if (timeTo[0] == 0 && timeTo[1] == 0) {
                /// First thing is adding the new flows
                currFlow += flows[targets[0]] + flows[targets[1]];
                foreach(var t in 
                /// I want all the couples of edges reaching two distinct nonvisited that can be reached before the end
                    from kv1 in allEdges[targets[0]]
                        .Where(kv => !visited.Contains(kv.Key))
                        .Where(kv => kv.Value < timeToEnd)
                    join kv2 in allEdges[targets[1]]
                        .Where(kv => !visited.Contains(kv.Key))
                        .Where(kv => kv.Value < timeToEnd)
                    on 1 equals 1
                    where kv1.Key != kv2.Key
                        /// This is a simple deduplication:
                        /// It is symmetric to consider (A, B) and (B, A) when starting from the same place
                        /// So we only choose one
                        && (flows[kv1.Key] >= flows[kv2.Key] || targets[0] != targets[1])
                    /// I'm using an eurystic: I want to first try the nodes will make me gain more,
                    /// and by going to A first I gain (timeToEnd - timeToA)*flowA
                    orderby
                        (timeToEnd - kv1.Value) * flows[kv1.Key]
                        + (timeToEnd - kv2.Value) * flows[kv2.Key]
                        descending
                    select (kv1, kv2)
                ) {
                    string t1 = t.kv1.Key;
                    string t2 = t.kv2.Key;
                    int n = FindBestPathTwoAgents(
                        allEdges: allEdges,
                        flows: flows,
                        visited: visited.Add(t1).Add(t2),
                        targets: Arr(t1, t2),
                        timeTo: Arr(t.kv1.Value + 1, t.kv2.Value + 1),
                        timeToEnd: timeToEnd,
                        currScore: currScore,
                        currFlow: currFlow,
                        maxReached: maxReached
                    );
                    
                    maxReached = Max(maxReached, n);
                    possibilities.Add(n);
                }
                /// possibilities is only empty when there are no two disting reachable nodes
                /// In this case, to consider the possibility that there is only one reachable node,
                /// we try both the cases
                /// Since we added both the flows, we need to remove the one that will be re-added in the next call 
                if (possibilities.Empty()) {
                    foreach (int i in Arr(0,1)) {
                        int[] timeToNew = (int[])timeTo.Clone();
                        timeToNew[i] = timeToEnd;

                        possibilities.Add(
                            FindBestPathTwoAgents(
                                allEdges: allEdges,
                                flows: flows,
                                visited: visited,
                                targets: targets,
                                timeTo: timeToNew,
                                timeToEnd: timeToEnd,
                                currScore: currScore,
                                currFlow: currFlow - flows[targets[i]],
                                maxReached: maxReached
                            )
                        );
                    }
                }
            }
            else {
                int index;
                if (timeTo[0] == 0) {
                    index = 0;
                }
                else {
                    index = 1;
                }
                currFlow += flows[targets[index]];
                foreach(
                    /// Same as before: I only want nonvisited nodes that I will reach before the end
                    var kv in allEdges[targets[index]]
                        .Where(kv => !visited.Contains(kv.Key))
                        .Where(kv => timeToEnd > kv.Value)
                        /// The eurystic is also the same
                        .OrderByDescending(kv => (timeToEnd - kv.Value) * flows[kv.Key])
                ) {
                    var newTargets = targets.ToArray();
                    var newTimes = timeTo.ToArray();
                    newTargets[index] = kv.Key;
                    newTimes[index] = kv.Value + 1;
                    int n = FindBestPathTwoAgents(
                        allEdges: allEdges,
                        flows: flows,
                        visited: visited.Add(kv.Key),
                        targets: newTargets,
                        timeTo: newTimes,
                        timeToEnd: timeToEnd,
                        currScore: currScore,
                        currFlow: currFlow,
                        maxReached: maxReached
                    );
                    maxReached = Max(maxReached, n);
                    possibilities.Add(n);
                }
                /// possibilities is empty if there are no new nodes to check
                /// in this case we let the simulation run for the max time
                /// or until the other node finishes, what comes first
                if (possibilities.Empty()) {
                    var newTimes = timeTo.ToArray();
                    newTimes[index] = timeToEnd;

                    possibilities.Add(
                        FindBestPathTwoAgents(
                            allEdges: allEdges,
                            flows: flows,
                            visited: visited,
                            targets: targets,
                            timeTo: newTimes,
                            timeToEnd: timeToEnd,
                            currScore: currScore,
                            currFlow: currFlow,
                            maxReached: maxReached
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
            var relevantNodes = flows.Where(kv => kv.Value > 0).Select(kv => kv.Key).ToImmutableHashSet();

            foreach(string node in relevantNodes.Add("AA")) {
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
            var relevantNodes = flows.Where(kv => kv.Value > 0).Select(kv => kv.Key).ToImmutableHashSet();

            foreach(string node in relevantNodes.Add("AA")) {
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
                Arr<string>().ToImmutableHashSet(),
                Arr("AA", "AA"),
                Arr(0, 0)
            );


            if (resultInt != 0) {
                result = resultInt.ToString();
            }
            return result;
        }
    }

}