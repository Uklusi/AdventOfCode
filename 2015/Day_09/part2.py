import networkx
import re
from itertools import permutations

G = networkx.Graph()

result = 0

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        (node1, node2, distance) = re.sub(r'( to | = )', " ", line).split(" ")
        distance = int(distance)
        G.add_nodes_from((node1, node2))
        G.add_edge(node1, node2, weight=distance)
    
    for p in permutations(G.nodes):
        cumul = 0
        for (a,b) in zip(p, p[1:]):
            cumul += G.edges[a,b]["weight"]
        if cumul > result:
            result = cumul



with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

