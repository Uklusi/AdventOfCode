from collections import defaultdict
from AoCUtils import *


result = inf
partNumber = "2"

writeToLog = False
if writeToLog:
    logFile = open("log" + partNumber + ".txt", "w")
else:
    logFile = "stdout"
printLog = printLogFactory(logFile)

frame = []
doors = {}
keys = {}

with open("input.txt", "r") as inputFile:
    lines = inputFile.read().strip().split("\n")
    for (y, line) in enumerate(lines):
        line = line.strip()
        frame.append(list(line))
        for (x, c) in enumerate(line):
            match c:
                case "@":
                    start = Position(x,y)
                case c if c.isalpha() and c.isupper():
                    doors[Position(x,y)] = c.lower()
                case c if c.isalpha() and c.islower():
                    keys[Position(x,y)] = c

newStartsList: list[Position] = []
for p in start.adjacent(includeCorners=True, include=[start]):
    if start.distance(p) == 2:
        newStartsList.append(p)
        frame[p.y][p.x] = "@"
    else:
        frame[p.y][p.x] = "#"

newStarts = tuple(newStartsList)


def dijkstraTweaked(
    start: MapPosition,
    distanceFunction: Callable[[Position, Position], Numeric] = lambda p, q: p.distance(q),
    doors: dict[Position, str] = {},
) -> dict[Position, dict[frozenset, Numeric]]:

    openSet: PriorityQueue[tuple[Numeric, frozenset[str], Position]] = PriorityQueue()
    distance: defaultdict[Position, dict[frozenset[str], Numeric]] = defaultdict(lambda: {})
    distance[start] = {frozenset() : 0}
    openSet.put((0, frozenset(), start))

    while not openSet.empty():
        (d, currentDoors, current) = openSet.get()
        for p in current.adjacent():
            currentDoorsNew = currentDoors | ( {doors[p]} if p in doors else set())
            tentativeDistance = d + distanceFunction(current, p)
            if p not in distance or all(tentativeDistance < distance[p][k] for k in distance[p] if set(k) <= currentDoorsNew):
                distance[p][currentDoorsNew] = tentativeDistance
                openSet.put((tentativeDistance, currentDoorsNew, p))
    return distance

def occupied(pos):
    return frame[pos.y][pos.x] == "#"

dijkstraValues = {}

possibilities = set(keys.keys()) | set(newStarts)
for p in possibilities:
    d = dijkstraTweaked(MapPosition(p.x, p.y, frame=frame, occupied=occupied), doors=doors)
    dijkstraValues[p] = {k: v for (k, v) in d.items() if k != p and k in keys.keys()}



states: defaultdict[tuple[Position, ...], dict[frozenset[str], Numeric]] = defaultdict(lambda: {})
states[newStarts] = {frozenset(): 0}

def solve(positions: tuple[Position, ...], ownedKeys: frozenset[str]):
    global states
    global result
    possibleDistances = {pos: dijkstraValues[pos] for pos in positions}
    
    distance = states[positions][ownedKeys]

    for (targetPos, targetKey) in keys.items():
        if targetKey in ownedKeys:
            continue
        newOwnedKeys = ownedKeys | {targetKey}
        tentativeDistance = inf
        for (n, pos) in enumerate(positions):
            if targetPos not in possibleDistances[pos]:
                continue
            for (requiredKeys, partialDistance) in possibleDistances[pos][targetPos].items():
                if requiredKeys <= newOwnedKeys:
                    tentativeDistance = min(tentativeDistance, partialDistance)
            if tentativeDistance == inf:
                continue

            newDistance = distance + tentativeDistance
            if newDistance >= result:
                continue

            newPositions = positions[:n] + (targetPos,) + positions[n+1:]

            skip = False
            for pastGottenKeys in states[newPositions]:
                if pastGottenKeys >= newOwnedKeys and states[newPositions][pastGottenKeys] <= newDistance:
                    skip = True
                    break
            if skip:
                continue

            states[newPositions][newOwnedKeys] = newDistance
            # printLog(states)
            # printLog(newOwnedKeysStr + " " + str(newDistance) + ", " + str(result))
            # printLog("")
            if newOwnedKeys == set(keys.values()):
                result = min(result, newDistance)
            else:
                solve(newPositions, newOwnedKeys)

solve(newStarts, frozenset())

with open("output" + partNumber + ".txt", "w") as outputFile:
    outputFile.write(str(result))
    print(str(result))

if writeToLog:
    cast(TextIOWrapper, logFile).close()


