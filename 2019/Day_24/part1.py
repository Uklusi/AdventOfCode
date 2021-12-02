from AoCUtils import *


result = 0
partNumber = "1"

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

class GameOfLife2(GameOfLife):
    def __init__(self, data):
        super().__init__(data)
    
    def _neighs(self, p: Position) -> list[MapPosition]:
        q = MapPosition(p.x, p.y, frame=self.state)
        return q.adjacent(includeCorners=False)

    def step(self) -> None:
        n = len(self.state)
        m = len(self.state[0])
        newstate = deepcopy(self.state)
        for i in range(n):
            for j in range(m):
                onNeighs = 0
                for p in self._neighs(Position(i,j)):
                    onNeighs += self.state[p.x][p.y]
                if self.state[i][j] and onNeighs in [1]:
                    newstate[i][j] = 1
                elif not self.state[i][j] and onNeighs in [1, 2]:
                    newstate[i][j] = 1
                else:
                    newstate[i][j] = 0
        self.state = newstate

game = GameOfLife2(data = board)
seen = {}
im = game.image()
while im not in seen:
    seen[im] = True
    game.step()
    im = game.image()

result = sum([(32 ** y * 2 ** x ) * game.state[y][x] for (x, y) in product(range(5), range(5))])

with open("output" + partNumber + ".txt", "w") as outputFile:
    outputFile.write(str(result))
    print(str(result))

if writeToLog:
    cast(TextIOWrapper, logFile).close()

