from AoCUtils import *
from collections import defaultdict
import re

result = 0
partNumber = "1"

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

def isPortalOuter(p):
    return p.x in [2, ncols - 3] or p.y in [2, nrows - 3]

class MapRecursivePosition(MapPosition):
    def __init__(
        self,
        x: int = 0,
        y: int = 0,
        level: int = 0,
        frame: Optional[Sequence[Sequence[Any]]] = None,
        occupied = lambda p: False
    ) -> None:
        super().__init__(x, y, frame = frame, occupied = occupied)
        self._frame = frame
        self.level = level
    
    def __eq__(self, other):
        return (
            self.x == other.x and
            self.y == other.y and
            self.level == other.level
        )
    
    def __hash__(self):
        return hash((self.x, self.y, self.level))
    
    def __str__(self):
        return f"({self.x}, {self.y}, {self.level})"

    def adjacent(self, includeCorners = False, include = []):
        global portals
        global portalPositions
        p = Position(self.x, self.y)
        if p in portalPositions:
            additional = list(set(portals[portalPositions[p]]) - {p})
        else:
            additional = []
            
        ret = [
            MapRecursivePosition(
                p.x, p.y, self.level, frame=self._frame, occupied=self._occupiedFunction
            )
            for p in super().adjacent()
        ] + [
            MapRecursivePosition(
                p.x, p.y, self.level + (-1 if isPortalOuter(self) else 1),
                frame=self._frame, occupied=self._occupiedFunction
            )
            for p in additional if self.level + (-1 if isPortalOuter(self) else 1) >= 0
        ]
        return ret
    
def distanceFunction(self: MapRecursivePosition, other: MapRecursivePosition | None = None) -> int:
    if other is None:
        other = Position(0,0)
    else:
        other = Position(other.x, other.y)
    other: Position
    p = Position(self.x, self.y)
    if p in portalPositions:
        if other in portals[portalPositions[p]]:
            return 1
    return (p - other).distance()

start = MapRecursivePosition(
    portals["AA"][0].x, portals["AA"][0].y, 0,
    frame = donutMap, occupied = lambda p: donutMap[p.y][p.x] != "."
)
end = MapRecursivePosition(
    portals["ZZ"][0].x, portals["ZZ"][0].y, 0,
    frame = donutMap, occupied = lambda p: donutMap[p.y][p.x] != "."
)

def dijkstra2(
    start: MapPosition,
    end,
    distanceFunction: Callable[[Position, Position], Numeric] = lambda p, q: p.distance(q),
) -> dict[Position, Numeric]:
    """
    Dijkstra graph exploration algorithm (on a grid).
    Returns a dictionary with, for each node, the min distance from start

    Usage: dijkstra(start, *distanceFunction)

    Assuming start instance of class Position
    or at least assuming that they are ordered, hashable and
    with a method called adjacent with parameter includeCorners.
    If called without specifiyng a distance fuction,
    it also assumes that there is a method called distance(otherPosition)
    """

    openSet: PriorityQueue[tuple[Numeric, Position]] = PriorityQueue()
    distance: dict[Position, Numeric] = {start: 0}
    openSet.put((distance[start], start))

    while not openSet.empty():
        (_, current) = openSet.get()
        if current == end:
            break
        for p in current.adjacent():
            tentativeDistance = distance[current] + distanceFunction(current, p)
            if p not in distance or distance[p] > tentativeDistance:
                distance[p] = tentativeDistance
                openSet.put((distance[p], p))
    return distance

# print(prettifyDict(portals))
# print(MapRecursivePosition(31, 8, 2, frame=donutMap, occupied = lambda p: donutMap[p.y][p.x] != ".").adjacent())
allNodes = dijkstra2(start = start, end = end, distanceFunction = distanceFunction)
# print(prettifyDict(allNodes))
result = allNodes[end]

with open("output" + partNumber + ".txt", "w") as outputFile:
    outputFile.write(str(result))
    print(str(result))

if writeToLog:
    cast(TextIOWrapper, logFile).close()

