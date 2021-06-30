from AoCUtils import *
from functools import cache
result = 0

depth = 5913
target = Position(8, 701)
# depth = 510
# target = Position(10,10)
erosionModule = 20183

@cache
def calcGeoIndex(pos):
    if pos.x == target.x and pos.y == target.y:
        ret = 0
    elif pos.x == 0:
        ret = pos.y * 48271
    elif pos.y == 0:
        ret = pos.x * 16807
    else:
        ret = calcErosionLevel(pos + Vector(-1, 0)) * calcErosionLevel(pos +  Vector(0, -1))
    return ret % erosionModule

@cache
def calcErosionLevel(pos):
    return (calcGeoIndex(pos) + depth) % erosionModule

@cache
def calcType(pos):
    return calcErosionLevel(pos) % 3

@cache
def nameType(pos):
    t = calcType(pos)
    return "R" if t == 0 else "W" if t == 1 else "N"
# 0 for rocky, 1 for wet, 2 for narrow

class CavePosition(MapPosition):
    def __init__(
        self,
        x = 0,
        y = 0,
        reverseY = True,
        frame = None,
        xmin = -inf,
        xmax = inf,
        ymin = -inf,
        ymax = inf,
        forbidden = lambda p: False,
        state = "T"
    ):
        super().__init__(
            x,
            y,
            reverseY = reverseY,
            frame = frame,
            xmin = xmin,
            xmax = xmax,
            ymin = ymin,
            ymax = ymax,
            occupied = forbidden
        )
        self.state = state

    def __add__(self, vector):
        return CavePosition(
            x = self.x + vector.vx,
            y = self.y + vector.vy,
            reverseY = self.reverseY,
            xmin = self.xmin,
            xmax = self.xmax,
            ymin = self.ymin,
            ymax = self.ymax,
            state = self.state,
            forbidden = forbidden
        )
    
    @property
    def ground(self):
        return nameType(self)
    
    def stdcoords(self, inverted=False):
        if inverted:
            return (self.y, self.x, self.state)
        return (self.x, self.y, self.state)

    def adjacent(self, includeCorners=False):
        c1 = self.copy()
        c2 = self.copy()
        c3 = self.copy()
        c1.state = "T"
        c2.state = "C"
        c3.state = "N"
        clist = [c1, c2, c3]
        ret = super().adjacent(includeCorners=includeCorners) + [c for c in clist if c != self and c.isEmpty()]
        return ret

    def position(self):
        return Position(self.x, self.y, reverseY=self.reverseY)

    def distance(self, other):
        return (self - other).distance() + (0 if self.state == other.state else 7)

def forbidden(cave):
    s = cave.state
    g = cave.ground
    return any([
        s == "T" and g == "W",
        s == "C" and g == "N",
        s == "N" and g == "R"
    ])

cave = CavePosition(0, 0, xmin=0, ymin=0, state="T", forbidden=forbidden)
target = CavePosition(target.x, target.y, xmin=0, ymin=0, state="T", forbidden=forbidden)

def newDistance(p, q):
    return p.distance(q) + (0 if p.state == q.state else 7)

result = aStar(start=cave, goal=target)

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

