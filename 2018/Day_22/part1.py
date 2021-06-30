from AoCUtils import *
from functools import cache

result = 0

depth = 5913
target = Position(8, 701)
# depth = 510
# target = Position(10,10)
erosionModule = 20183

@cache
def calcGeoIndex(pos):
    if pos == Position(0,0) or pos == target:
        ret = 0
    elif pos.x == 0:
        ret = pos.y * 48271
    elif pos.y == 0:
        ret = pos.x * 16807
    else:
        ret = calcErosionLevel(pos + Vector(-1, 0)) * calcErosionLevel(pos +  Vector(0, -1))
    return ret % erosionModule

@cache
def calcErosionLevel(pos):
    return (calcGeoIndex(pos) + depth) % erosionModule

@cache
def calcType(pos):
    return calcErosionLevel(pos) % 3

for x in range(target.x + 1):
    for y in range(target.y + 1):
        result += calcType(Position(x,y))

def visual(p):
    if p == Position(0,0):
        return "M"
    elif p == target:
        return "T"
    t = calcType(p)
    if t == 0:
        return "."
    elif t== 1:
        return "="
    else:
        return "|"

m = Map(visual=visual, xmax = 10, ymax = 10)
print(m)

with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

