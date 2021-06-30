from collections import defaultdict

result = 0

class Position4D():
    def __init__(self, l):
        self.coords = tuple(l)

    def __str__(self):
        return str(self.coords)

    def __hash__(self):
        return hash(self.coords)

    def distance(self, other=None):
        p1 = self.coords
        if other is None:
            p2 = (0,0,0,0)
        else:
            p2 = other.coords
        d = 0
        for i in range(4):
            d += abs(p1[i] - p2[i])
        return d


points = []
with open("input.txt", "r") as input:
    for line in input:
        line = line.strip().split(",")
        line = [int(n) for n in line]
        points.append(Position4D(line))

children = {p:[p] for p in points}
root = {}

for p in points:
    root[p] = p
    for q in root:
        if p.distance(q) <= 3 and q != p:
            r = root[q]
            if r not in children[p]:
                children[p].append(r)
                for s in children[r]:
                    children[p].append(s)
                    root[s] = p

result = len(set(root.values()))


with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

