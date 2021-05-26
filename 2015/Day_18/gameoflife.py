from copy import deepcopy

class GameOfLife():
    def __init__(self, data, on="#", off="."):
        self.on = on
        self.off = off
        self.state = [[1 if c is on else 0 for c in s] for s in data]

    def __repr__(self):
        return "\n".join(["".join([self.on if bit else self.off for bit in s]) for s in self.state])

    def __str__(self):
        return self.__repr__()

    def _neighs(self, point):
        x = point[0]
        y = point[1]
        n = len(self.state) - 1
        m = len(self.state[0]) - 1
        xlow = x - 1 if x > 0 else 0
        xhigh = x + 1 if x < m else m
        ylow = y - 1 if y > 0 else 0
        yhigh = y + 1 if y < n else n
        return [(a,b) for a in range(xlow, xhigh + 1) for b in range(ylow, yhigh + 1) if (a,b) != point]
    
    def step(self):
        n = len(self.state)
        m = len(self.state[0])
        newstate = deepcopy(self.state)
        for i in range(n):
            for j in range(m):
                onNeighs = 0
                for (x,y) in self._neighs((i,j)):
                    onNeighs += self.state[x][y]
                if self.state[i][j] and onNeighs in [2,3]:
                    newstate[i][j] = 1
                elif not self.state[i][j] and onNeighs == 3:
                    newstate[i][j] = 1
                else:
                    newstate[i][j] = 0
        self.state = newstate