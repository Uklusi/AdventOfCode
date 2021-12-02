from AoCUtils import *
from collections import defaultdict


result = 0
partNumber = "2"

writeToLog = False
if writeToLog:
    logFile = open("log" + partNumber + ".txt", "w")
else:
    logFile = "stdout"
printLog = printLogFactory(logFile)

board = []
with open("input.txt", "r") as inputFile:
    lines = inputFile.read().strip().split("\n")
    for line in lines:
        line = line.strip()
        board.append(line)

class RecursivePosition(PositionNDim):
    def __init__(self, x, y, level):
        super().__init__(x, y, level)
        self.level = level

    def adjacent(self):
        if (self.x, self.y) == (2,2):
            return None
        positions = []
        match (self.x, self.y):
            case (0, _):
                positions.append(
                    RecursivePosition(1, 2, self.level - 1)
                )
            case (3, 2):
                positions.extend( [
                    RecursivePosition(4, 0, self.level + 1),
                    RecursivePosition(4, 1, self.level + 1),
                    RecursivePosition(4, 2, self.level + 1),
                    RecursivePosition(4, 3, self.level + 1),
                    RecursivePosition(4, 4, self.level + 1)
                ])
            case _:
                positions.append(
                    RecursivePosition(self.x - 1, self.y, self.level)
                )

        match (self.x, self.y):
            case (4, _):
                positions.append(
                    RecursivePosition(3, 2, self.level - 1)
                )
            case (1, 2):
                positions.extend( [
                    RecursivePosition(0, 0, self.level + 1),
                    RecursivePosition(0, 1, self.level + 1),
                    RecursivePosition(0, 2, self.level + 1),
                    RecursivePosition(0, 3, self.level + 1),
                    RecursivePosition(0, 4, self.level + 1)
                ])
            case _:
                positions.append(
                    RecursivePosition(self.x + 1, self.y, self.level)
                )

        match (self.x, self.y):
            case (_, 0):
                positions.append(
                    RecursivePosition(2, 1, self.level - 1)
                )
            case (2, 3):
                positions.extend( [
                    RecursivePosition(0, 4, self.level + 1),
                    RecursivePosition(1, 4, self.level + 1),
                    RecursivePosition(2, 4, self.level + 1),
                    RecursivePosition(3, 4, self.level + 1),
                    RecursivePosition(4, 4, self.level + 1)
                ])
            case _:
                positions.append(
                    RecursivePosition(self.x, self.y - 1, self.level)
                )

        match (self.x, self.y):
            case (_, 4):
                positions.append(
                    RecursivePosition(2, 3, self.level - 1)
                )
            case (2, 1):
                positions.extend( [
                    RecursivePosition(0, 0, self.level + 1),
                    RecursivePosition(1, 0, self.level + 1),
                    RecursivePosition(2, 0, self.level + 1),
                    RecursivePosition(3, 0, self.level + 1),
                    RecursivePosition(4, 0, self.level + 1)
                ])
            case _:
                positions.append(
                    RecursivePosition(self.x, self.y + 1, self.level)
                )
        
        return positions

state = defaultdict(lambda: [[0 for i in range(5)] for j in range(5)])
state[0] = [[0 if board[j][i] == "." else 1 for i in range(5)] for j in range(5)]
minLevel = 0
maxLevel = 0

def step() -> None:
    global state
    global minLevel
    global maxLevel
    n = 5
    m = 5
    newstate = deepcopy(state)
    for lev in range(minLevel - 1, maxLevel + 2):
        for i, j in product(range(n), range(m)):
            if (i, j) == (2, 2):
                continue
            onNeighs = 0
            for p in RecursivePosition(i, j, lev).adjacent():
                onNeighs += state[p.level][p.y][p.x]
            if state[lev][j][i] and onNeighs in [1]:
                newstate[lev][j][i] = 1
            elif not state[lev][j][i] and onNeighs in [1, 2]:
                newstate[lev][j][i] = 1
            else:
                newstate[lev][j][i] = 0
    if any([ any( [state[minLevel - 1][j][i] == 1 for j in range(m)] ) for i in range(n) ]):
        minLevel -= 1
    if any([ any( [state[maxLevel + 1][j][i] == 1 for j in range(m)] ) for i in range(n)]):
        maxLevel += 1
    state = newstate

for _ in range(200):
    step()


result = sum([sum(state[k][i]) for i in range(5) for k in range(minLevel, maxLevel + 1)])


with open("output" + partNumber + ".txt", "w") as outputFile:
    outputFile.write(str(result))
    print(str(result))

if writeToLog:
    cast(TextIOWrapper, logFile).close()

