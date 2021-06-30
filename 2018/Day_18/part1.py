from AoCUtils import *

result = 0

frame = []
with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        frame.append(line)

class Landscape():
    def __init__(self, data):
        self.state = [[c for c in s] for s in data]

    def __repr__(self):
        return "\n".join(["".join([c for c in s]) for s in self.state])

    def __str__(self):
        return self.__repr__()

    def _neighs(self, p):
        q = MapPosition(p.x, p.y, frame=self.state)
        return q.adjacent(includeCorners=True)

    def getState(self, p):
        if isinstance(p, tuple):
            (x, y) = p
        else:
            (y, x) = p.coords(inverted=True)
        return self.state[y][x]

    def step(self):
        n = len(self.state)
        m = len(self.state[0])
        newstate = deepcopy(self.state)
        for j in range(n):
            for i in range(m):
                tree = "|"
                lumber = "#"
                open = "."
                num = {tree:0, lumber:0, open:0}
                pos = Position(i, j)
                currState = self.getState(pos)
                for neigh in self._neighs(pos):
                    s = self.getState(neigh)
                    num[s] += 1
                
                nextState = currState

                if currState == open and num[tree] >= 3:
                    nextState = tree
                elif currState == tree and num[lumber] >= 3:
                    nextState = lumber
                elif currState == lumber and (num[lumber] == 0 or num[tree] == 0):
                    nextState = open
                newstate[j][i] = nextState
        self.state = newstate

landscape = Landscape(frame)

for _ in range(10):
    landscape.step()
    # print(Image(landscape.state))
    # print()

final = Image(landscape.state).image()

result = final.count("#") * final.count("|")

with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

