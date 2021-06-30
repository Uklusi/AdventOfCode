from copy import copy, deepcopy
import hashlib

# Position class
class Position():
    def __init__(self, x=0, y=0, orientation=0):
        self.x = x
        self.y = y
        self.orientation = orientation

    def __add__(self, other):
        return Position(self.x + other.x, self.y + other.y, self.orientation)
    
    def __rmul__(self, n):
        return Position(n*self.x, n*self.y) 
    
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
            raise(Exception("DirectionError: "+ direction))
    
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
            self.y += n
        elif direction == 1:
            self.x += n
        elif direction == 2:
            self.y -= n
        elif direction == 3:
            self.x -= n
        else:
            raise(Exception("DirectionError: "+ direction))

    def current(self):
        return (self.x, self.y)

    def copy(self):
        return copy(self)
    
    def adjacent(self):
        return [Position(self.x + i,self.y + j) for i in [-1,0,1] for j in [-1, 0, 1] if (i,j) != (0,0)]
    
    def gridAdj(self):
        return [self + Position(i, j) for (i,j) in [(-1, 0), (1,0), (0,1), (0,-1)] ]

    # Using - for grid distance calc
    def __sub__(self, other):
        return gridDistance(self, other)

def gridDistance(p, q):
    return abs(p.x - q.x) + abs(p.y - q.y)

def planeDistance(p, q):
    return ( (p.x - q.x) ** 2 + (p.y - q.y) ** 2 ) ** (1/2)

# LimitedPosition class
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

def _inbound(n, min, max):
    n = _minNone(n, max)
    n = _maxNone(n, min)
    return n

class LimitedPosition(Position):
    def __init__(self, x=0, y=0, orientation=0, xmin=None, xmax=None, ymin = None, ymax = None):
        super().__init__(x, y, orientation)
        self.xmin = xmin
        self.xmax = xmax
        self.ymin = ymin
        self.ymax = ymax

    def __add__(self, other):
        return LimitedPosition(self.x + other.x, self.y + other.y, self.orientation, self.xmin, self.xmax, self.ymin, self.ymax)
    
    def move(self, n, direction=None):
        super().move(n, direction)

        self.x = _inbound(self.x, self.xMin, self.xmax)
        self.y = _inbound(self.y, self.yMin, self.ymax)

    def isInLimits(self):
        return self.x == _inbound(self.x, self.xMin, self.xmax) and self.y == _inbound(self.y, self.yMin, self.ymax)
    
    def adjacent(self):
        ret = [self + Position(i, j) for i in [-1,0,1] for j in [-1, 0, 1] if \
            (i,j) != (0,0)]
        return [p for p in ret if p.isInLimits()]
    
    def gridAdj(self):
        ret = [self + Position(i, j) for (i,j) in [(-1, 0), (1,0), (0,1), (0,-1)] ]
        return [p for p in ret if p.isInLimits()]

# maze characters
solid = "\u2588"
empty = " "
path = "·"

class GameOfLife():
    def __init__(self, data, on="#", off="."):
        self.on = on
        self.off = off
        self.state = [[1 if c is on else 0 for c in s] for s in data]

    def __repr__(self):
        return "\n".join(["".join([solid if bit else empty for bit in s]) for s in self.state])

    def __str__(self):
        return self.__repr__()

    def _neighs(self, p):
        n = len(self.state) - 1
        m = len(self.state[0]) - 1
        q = LimitedPosition(p.x, p.y, xmin=0, xmax=m, ymin=0, ymax=n)
        return q.gridAdj()
    
    def step(self):
        n = len(self.state)
        m = len(self.state[0])
        newstate = deepcopy(self.state)
        for i in range(n):
            for j in range(m):
                onNeighs = 0
                for p in self._neighs(Position(i,j)):
                    onNeighs += self.state[p.x][p.y]
                if self.state[i][j] and onNeighs in [2,3]:
                    newstate[i][j] = 1
                elif not self.state[i][j] and onNeighs == 3:
                    newstate[i][j] = 1
                else:
                    newstate[i][j] = 0
        self.state = newstate



# Easier md5
def md5(string):
    return hashlib.md5(string.encode()).hexdigest()


class HexGrid():
    def __init__(self, x=0, y=0):
        self.x = x
        self.y = y

    def __add__(self, other):
        return HexGrid(self.x + other.x, self.y + other.y)
    
    def __rmul__(self, n):
        return Position(n*self.x, n*self.y) 
    
    def __hash__(self):
        return hash((self.x, self.y))
    
    def __eq__(self, other):
        return (self.x, self.y) == (other.x, other.y)

    def __str__(self):
        return "Hex" + str((self.x, self.y))

    def __repr__(self):
        return str(self)
        
    def move(self, n, direction=None):
        if direction is None:
            raise(Exception("DirectionError: None"))
        elif direction.lower() in ["n", "u"]:
            self.x += 1
            self.y += 1
        elif direction.lower() in ["ne", "ur"]:
            self.x += 1
        elif direction.lower() in ["nw", "ul"]:
            self.y += 1
        elif direction.lower() in ["s", "d"]:
            self.x += -1
            self.y += -1
        elif direction.lower() in ["se", "dr"]:
            self.y += -1
        elif direction.lower() in ["sw", "dl"]:
            self.x += -1
        else:
            raise(Exception("DirectionError: "+ direction))

    def current(self):
        return (self.x, self.y)

    def copy(self):
        return copy(self)
    
    def adjacent(self):
        return [self + HexGrid(i,j) for (i,j) in [(1,0), (0,1), (1,1), (-1,0), (0,-1),(-1,-1)]]
    
    # Using - for distance calc
    def __sub__(self, other):
        x = self.x - other.x
        y = self.y - other.y
        if x * y <= 0:
            return abs(x) + abs(y)
        else:
            return max(abs(x), abs(y))