from copy import copy, deepcopy

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
		
def _minNone(a, b):
    if a is None:
        return b
    if b is None:
        return a
    return min(a,b)

def _maxNone(a, b):
    if a is None:
        return b
    if b is None:
        return a
    return max(a,b)

class Position():
    def __init__(self, x=0, y=0, orientation=0, xmin=None, xmax=None, ymin = None, ymax = None):
        self.x = x
        self.y = y
        self.orientation = orientation
        self.xmin = xmin
        self.xmax = xmax
        self.ymin = ymin
        self.ymax = ymax

    def __add__(self, other):
        return Position(self.x + other.x, self.y + other.y, self.orientation, self.xmin, self.xmax, self.ymin, self.ymax)
    
    def __hash__(self):
        return hash((self.x, self.y))
    
    def __eq__(self, other):
        return (self.x, self.y) == (other.x, other.y)

    def __str__(self):
        return str((self.x, self.y))

    def __repr__(self):
        return str(self)
        
    def turnRight(self):
        self.orientation = (self.orientation + 1) % 4
    
    def turnLeft(self):
        self.orientation = (self.orientation - 1) % 4
    
    def turn(self, direction):
        if direction in ["R", "r", "1", 1]:
            self.turnRight()
        elif direction in ["L", "l", "-1", -1]:
            self.turnLeft()
        elif direction in ["0", 0]:
            pass
        else:
            raise("DirectionError")
    
    def move(self, n, direction=None):
        if direction is None:
            direction = self.orientation
        elif direction in ["N", "n", "U", "u"]:
            direction = 0
        elif direction in ["E", "e", "R", "r"]:
            direction = 1
        elif direction in ["S", "s", "D", "d"]:
            direction = 2
        elif direction in ["W", "w", "L", "l"]:
            direction = 3

        if direction == 0:
            self.y = _minNone(self.ymax, self.y + n)
        elif direction == 1:
            self.x = _minNone(self.xmax, self.x + n)
        elif direction == 2:
            self.y = _maxNone(self.ymin, self.y - n)
        elif direction == 3:
            self.x = _maxNone(self.xmin, self.x - n)
        else:
            raise("DirectionError")

    def current(self):
        return (self.x, self.y)

    def copy(self):
        return copy(self)
    
    def isInLimits(self):
        ret = True
        if self.xmin is not None:
            ret = ret and self.xmin <= self.x
        if self.xmax is not None:
            ret = ret and self.x <= self.xmax
        if self.ymin is not None:
            ret = ret and self.ymin <= self.y
        if self.ymax is not None:
            ret = ret and self.y <= self.ymax
    
    def adjacent(self):
        ret = [self + Position(i, j) for i in [-1,0,1] for j in [-1, 0, 1] if \
            (i,j) != (0,0)]
        return [p for p in ret if p.isInLimits()]
            
    
    def gridAdj(self):
        ret = [self + Position(i, j) for (i,j) in [(-1, 0), (1,0), (0,1), (0,-1)] ]
        return [p for p in ret if p.isInLimits()]

solid = "\u2588"
empty = " "
path = "·"