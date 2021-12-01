from AoCUtils import *
from collections import defaultdict
import re

result = 0
partNumber = "2"

writeToLog = False
if writeToLog:
    logFile = open("log" + partNumber + ".txt", "w")
else:
    logFile = "stdout"
printLog = printLogFactory(logFile)

fullMap = []
donutMap = []

with open("input.txt", "r") as inputFile:
    lines = inputFile.read().split("\n")
    for n, line in enumerate(lines):
        fullMap.append(line)
        donutMap.append(re.sub("[A-Z]", " ", line))

nrows = len(fullMap)
ncols = len(fullMap[0])

innerXMin = ncols // 2
while donutMap[nrows // 2][innerXMin] == " ":
    innerXMin -= 1
innerXMax = ncols // 2
while donutMap[nrows // 2][innerXMax] == " ":
    innerXMax += 1
innerYMin = nrows // 2
while donutMap[innerYMin][ncols // 2] == " ":
    innerYMin -= 1
innerYMax = nrows // 2
while donutMap[innerYMax][ncols // 2] == " ":
    innerYMax += 1

portals: defaultdict[str, list[Position]] = defaultdict(lambda: [])

x = 2
for y in range(2, nrows - 2):
    if fullMap[y][x] == ".":
        portals[fullMap[y][x-2] + fullMap[y][x-1]].append(Position(x, y))

x = ncols - 3
for y in range(2, nrows - 2):
    if fullMap[y][x] == ".":
        portals[fullMap[y][x+1] + fullMap[y][x+2]].append(Position(x, y))

x = innerXMin
for y in range(innerYMin + 1, innerYMax):
    if fullMap[y][x] == ".":
        portals[fullMap[y][x+1] + fullMap[y][x+2]].append(Position(x, y))

x = innerXMax
for y in range(innerYMin + 1, innerYMax):
    if fullMap[y][x] == ".":
        portals[fullMap[y][x-2] + fullMap[y][x-1]].append(Position(x, y))

y = 2
for x in range(2, ncols - 2):
    if fullMap[y][x] == ".":
        portals[fullMap[y-2][x] + fullMap[y-1][x]].append(Position(x, y))

y = nrows - 3
for x in range(2, ncols - 2):
    if fullMap[y][x] == ".":
        portals[fullMap[y+1][x] + fullMap[y+2][x]].append(Position(x, y))

y = innerYMin
for x in range(innerXMin + 1, innerXMax):
    if fullMap[y][x] == ".":
        portals[fullMap[y+1][x] + fullMap[y+2][x]].append(Position(x, y))

y = innerYMax
for x in range(innerXMin + 1, innerXMax):
    if fullMap[y][x] == ".":
        portals[fullMap[y-2][x] + fullMap[y-1][x]].append(Position(x, y))

portalPositions = {pos: portal for portal, l in portals.items() for pos in l if len(l) == 2}

class MapPortalPosition(MapPosition):
    def __init__(
        self,
        x: int = 0,
        y: int = 0,
        frame: Optional[Sequence[Sequence[Any]]] = None,
        occupied = lambda p: False
    ) -> None:
        super().__init__(x, y, frame = frame, occupied = occupied)
        self._frame = frame
    
    def adjacent(self, includeCorners = False, include = []):
        global portals
        global portalPositions
        if self in portalPositions:
            additional = list(set(portals[portalPositions[self]]) - {self})
        else:
            additional = []
        ret = [
            MapPortalPosition(p.x, p.y, frame=self._frame, occupied=self._occupiedFunction)
            for p in super().adjacent() + additional
        ]
        return ret
    
def distanceFunction(p, other = None) -> int:
    if other is None:
        other = Position(0,0)
    if p in portalPositions:
        if other in portals[portalPositions[p]]:
            return 1
    return (p - other).distance()

start = MapPortalPosition(portals["AA"][0].x, portals["AA"][0].y, frame = donutMap, occupied = lambda p: donutMap[p.y][p.x] != ".")
end = MapPortalPosition(portals["ZZ"][0].x, portals["ZZ"][0].y, frame = donutMap, occupied = lambda p: donutMap[p.y][p.x] != ".")


allNodes = dijkstra(start = start, distanceFunction = distanceFunction)
result = allNodes[end]


with open("output" + partNumber + ".txt", "w") as outputFile:
    outputFile.write(str(result))
    print(str(result))

if writeToLog:
    cast(TextIOWrapper, logFile).close()

