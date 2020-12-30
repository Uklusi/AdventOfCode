import networkx
import re

G = networkx.Graph()

result = 0

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        (node1, node2, distance) = re.sub(r'( to | = )', " ", line).split(" ")
        distance = int(distance)
        G.add_nodes_from((node1, node2))
        G.add_edge(node1, node2, weight=distance)

 


with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

