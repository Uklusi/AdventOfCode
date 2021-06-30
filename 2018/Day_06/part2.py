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
    sumd = 0
    for (n, q) in enumerate(positions):
        sumd += p.distance(q)
    if sumd < 10000:
        result += 1

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

