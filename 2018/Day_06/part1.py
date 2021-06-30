from AOCClasses import Position
from itertools import product, chain
from collections import defaultdict

result = 0

positions = []
with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        positions.append(Position(*map(int, line.split(", "))))
    
xmin = min(map(lambda p: p.x, positions))
xmax = max(map(lambda p: p.x, positions))
ymin = min(map(lambda p: p.y, positions))
ymax = max(map(lambda p: p.y, positions))

posDistance = {}
for (i, j) in product(range(xmin, xmax + 1), range(ymin,  ymax + 1)):
    p = Position(i, j)
    mindist = 99999
    minQ = -1
    for (n, q) in enumerate(positions):
        d = p.distance(q)
        if d < mindist:
            minQ = n
            mindist = d
        elif d == mindist:
            minQ = -1
    posDistance[p] = minQ

infinite = set(map(lambda p: posDistance[p], chain(
    *[[Position(xmin, y), Position(xmax, y)] for y in range(ymin, ymax+1)],
    *[[Position(x, ymin), Position(x, ymax)] for x in range(xmin, xmax+1)]
)))

finite = set(range(len(positions))) - infinite

count = defaultdict(lambda: 0)

for (i, j) in product(range(xmin, xmax + 1), range(ymin,  ymax + 1)):
    p = Position(i, j)
    n = posDistance[p]
    if n in finite:
        count[n] += 1

maxi = max(finite, key=lambda n: count[n])

result = count[maxi]

with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

