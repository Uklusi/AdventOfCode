from copy import copy, deepcopy
import hashlib
from itertools import product, permutations
import numpy as np

# Position class
class Position():
    def __init__(self, x=0, y=0, orientation=0):
        self.x = x
        self.y = y
        if orientation in ["N", "n", "U", "u", 0]:
            self.orientation = 0
        elif orientation in ["E", "e", "R", "r", 1]:
            self.orientation = 1
        elif orientation in ["S", "s", "D", "d", 2]:
            self.orientation = 2
        elif orientation in ["W", "w", "L", "l", 3]:
            self.orientation = 3
        else:
            raise(Exception(f"DirectionError: {orientation}"))

    def __add__(self, other):
        return Position(
            self.x + other.x,
            self.y + other.y,
            orientation=self.orientation
        )
    
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
        elif direction in ["0", 0, None]:
            pass
        else:
            raise(Exception(f"DirectionError: {direction}"))
    
    def move(self, n, direction=None):
        if direction is None:
            direction = self.orientation
        elif direction in ["N", "n", "U", "u", 0]:
            direction = 0
        elif direction in ["E", "e", "R", "r", 1]:
            direction = 1
        elif direction in ["S", "s", "D", "d", 2]:
            direction = 2
        elif direction in ["W", "w", "L", "l", 3]:
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
            raise(Exception(f"DirectionError {direction}"))

    def current(self):
        return (self.x, self.y)

    def copy(self):
        return copy(self)
    
    def adjacent(self):
        return [self + Position(i,j) for (i,j) in product([-1,0,1], repeat=2) if (i,j) != (0,0)]
    
    def gridAdj(self):
        return [self + Position(i, j) for (i,j) in [(-1, 0), (1,0), (0,1), (0,-1)] ]

    def __sub__(self, other):
        return Position(
            self.x - other.x,
            self.y - other.y,
            orientation=self.orientation
        )

    def distance(self, other=None):
        if other is None:
            other = Position(0,0)
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
    def __init__(self, x=0, y=0, orientation=0, frame=None, xmin=None, xmax=None, ymin=None, ymax=None):
        super().__init__(x=x, y=y, orientation=orientation)
        if frame is not None:
            self.xmin = 0
            self.xmax = len(frame[0]) - 1
            self.ymin = 0
            self.ymax = len(frame) - 1 
        else:
            self.xmin = xmin
            self.xmax = xmax
            self.ymin = ymin
            self.ymax = ymax

    def __add__(self, other):
        return LimitedPosition(
            self.x + other.x,
            self.y + other.y,
            orientation=self.orientation,
            xmin=self.xmin,
            xmax=self.xmax,
            ymin=self.ymin,
            ymax=self.ymax
        )
    
    def move(self, n, direction=None):
        super().move(n, direction)

        self.x = _inbound(self.x, self.xmin, self.xmax)
        self.y = _inbound(self.y, self.ymin, self.ymax)

    def isInLimits(self):
        return (
            self.x == _inbound(self.x, self.xmin, self.xmax) and
            self.y == _inbound(self.y, self.ymin, self.ymax)
        )
    
    def adjacent(self):
        ret = super().adjacent()
        return [p for p in ret if p.isInLimits()]
    
    def gridAdj(self):
        ret = super().gridAdj()
        return [p for p in ret if p.isInLimits()]

# maze characters
solid = "\u2588"
empty = " "
path = "·"


class SolidPosition(LimitedPosition):
    def __init__(
        self,
        x=0,
        y=0,
        orientation=0,
        frame=None,
        xmin=None,
        xmax=None,
        ymin=None,
        ymax=None,
        solid=lambda p: False
    ):
        super().__init__(
            x,
            y,
            orientation=orientation,
            frame=frame,
            xmin=xmin,
            xmax=xmax,
            ymin=ymin,
            ymax=ymax
        )
        self._solidFunction = solid

    def isSolid(self):
        return self._solidFunction(self)
    
    def isEmpty(self):
        return not self.isSolid()

    def __add__(self, other):
        return SolidPosition(
            self.x + other.x,
            self.y + other.y,
            orientation=self.orientation,
            xmin=self.xmin,
            xmax=self.xmax,
            ymin=self.ymin,
            ymax=self.ymax,
            solid=self._solidFunction
        )
    
    def move(self, n, direction=None):
        if n != 1:
            for _ in range(n):
                self.move(1, direction)
                return
        cx = self.x
        cy = self.y
        super().move(1, direction)
        if self.isSolid():
            self.x = cx
            self.y = cy

    def adjacent(self):
        ret = super().adjacent()
        return [p for p in ret if p.isEmpty()]
    
    def gridAdj(self):
        ret = super().gridAdj()
        return [p for p in ret if p.isEmpty()]


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
        q = LimitedPosition(p.x, p.y, frame=self.state)
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
            raise(Exception(f"DirectionError: {direction}"))

    def current(self):
        return (self.x, self.y)

    def copy(self):
        return copy(self)
    
    def adjacent(self):
        return [self + HexGrid(i,j) for (i,j) in [(1,0), (0,1), (1,1), (-1,0), (0,-1),(-1,-1)]]
    
    def __sub__(self, other):
        return HexGrid(self.x - other.x, self.y - other.y)

    def distance(self, other=None):
        if other is None:
            other = HexGrid(0,0)
        x = self.x - other.x
        y = self.y - other.y
        if x * y <= 0:
            return abs(x) + abs(y)
        else:
            return max(abs(x), abs(y))

class Position3D():
    def __init__(self, x=0, y=0, z=0):
        self.x = x
        self.y = y
        self.z = z

    def __add__(self, other):
        return Position3D(
            self.x + other.x,
            self.y + other.y,
            self.z + other.z
        )
    
    def __rmul__(self, n):
        return Position3D(n*self.x, n*self.y, n*self.z) 
    
    def __hash__(self):
        return hash((self.x, self.y, self.z))
    
    def __eq__(self, other):
        return (self.x, self.y, self.z) == (other.x, other.y, other.z)

    def __str__(self):
        return str((self.x, self.y, self.z))

    def __repr__(self):
        return str(self)

    def current(self):
        return (self.x, self.y, self.z)

    def copy(self):
        return copy(self)
    
    def adjacent(self):
        return [self + Position3D(i,j,k) for (i,j,k) in product([-1,0,1], repeat=3) if (i,j,k) != (0,0,0)]
    
    def gridAdj(self):
        return [self + Position3D(i,j,k) for (i,j,k) in [(-1,0,0), (1,0,0), (0,1,0), (0,-1,0), (0,0,1), (0,0,-1)] ]

    def __sub__(self, other):
        return Position3D(
            self.x - other.x,
            self.y - other.y,
            self.z - other.z
        )

    def distance(self, other=None):
        if other is None:
            other = Position3D(0,0,0)
        s = self - other
        return sum(map(abs, [s.x, s.y, s.z]))
        
class Image():
    def __init__(self, image):
        self.pixels = np.array([list(r) for r in image])
    
    def copy(self):
        return Image(self.pixels)

    def image(self):
        return "\n".join(["".join([str(pixel) for pixel in row]) for row in self.pixels])
    
    def __str__(self):
        return self.image()

    def __repr__(self):
        return self.image()

    def rotate(self, n=1, clockwise=False, copy=False):
        if clockwise:
            k = -n
        else:
            k = n
        i = np.rot90(self.pixels, k)
        if copy:
            return Image(i)
        else:
            self.pixels = i
    
    def flip(self, ud=False, copy=False):
        if ud:
            i = np.flipud(self.pixels)
        else:
            i = np.fliplr(self.pixels)
        if copy:
            return Image(i)
        else:
            self.pixels = i

    def rotations(self):
        i1 = self.rotate(0, copy=True)
        i2 = self.rotate(1, copy=True)
        i3 = self.rotate(2, copy=True)
        i4 = self.rotate(3, copy=True)
        return [i1, i2, i3, i4]

    def variations(self):
        i1 = self.flip(copy=True)
        return self.rotations() + i1.rotations()

    def __hash__(self):
        return hash(self.image())

    @property
    def shape(self):
        return self.pixels.shape

    def __eq__(self, other):
        if self.shape != other.shape:
            return False
        return (self.pixels == other.pixels).all()

    def __add__(self, other):
        return Image(np.concatenate((self.pixels, other.pixels), axis=1))

    def __and__(self, other):
        return Image(np.concatenate((self.pixels, other.pixels), axis=0))
    
    def slice(self, y=(0, None), x=(0, None)):
        if isinstance(x, int):
            x = (x, x+1)
        if isinstance(y, int):
            y = (y, y+1)
        if x[1] is None:
            x = (x[0], self.shape[1])
        if y[1] is None:
            y = (y[0], self.shape[0])
        return Image(
            [[self.pixels[j][i] for i in range(*x)] for j in range(*y)]
        )




