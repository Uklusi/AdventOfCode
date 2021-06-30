from AOCClasses import *
from queue import SimpleQueue as Queue
from copy import deepcopy

log = open("logp2.txt", "w")
image = open("imagep2.txt", "w")

def printLog(txt="", other=""):
    txt = str(txt)
    other = str(other)
    log.write(txt + other + "\n")

def normal(val):
    if isinstance(val, tuple):
        return f"({val[0]:>2d}, {val[1]:>2d})"
    return normal(val.coords(inverted=True))

powAndNumHits = [
    ( 3, 67), # 10
    ( 4, 50), # 10
    ( 5, 40), # 10
    ( 6, 34), # 10
    ( 7, 29), # 10
    ( 8, 25), #  7
    ( 9, 23), #  7
    (10, 20), #  5
    (11, 19), #  5
    (12, 17), #  5
    (13, 16), #  4
    (14, 15), #  3
    (15, 14), #  1
    (16, 13), #  3
    (17, 12), #  0
    (19, 11), #  2
    (20, 10), #  0
    (23,  9), #  0
    (25,  8), #  0
    (29,  7), #  1
    (34,  6)  #  0  
]
POWER = 17

def stepOrientation(f, t):
    q = t - f
    if q.distance() != 1:
        raise(Exception("Not a step"))
    elif q.y == -1:
        return "U"
    elif q.y == 1:
        return "D"
    elif q.x == -1:
        return "L"
    elif q.x == 1:
        return "R"
    else:
        raise(Exception("Not a step"))

class Unit(SolidPosition):
    def __init__(self, x,y,frame=None, solid=None, unitType=None):
        super().__init__(x,y, reverseY=True, frame=frame, solid=solid)
        # self.hp = 67 if unitType == "E" else int(200/POWER -0.1)+1
        # self.atk = 1
        self.hp = 200
        self.atk = 3 if unitType == "G" else POWER
        self.type = unitType
        self.dead = False

    def moveTo(self, pos):
        if self == pos:
            return
        self.move(n=1, direction=stepOrientation(self, pos))

    def attack(self, enemy):
        enemy.hp -= self.atk
        if enemy.hp <= 0:
            enemy.dead = True

    def gridAdj(self, include=None):
        ret = super().gridAdj()
        if include is not None and self.distance(include) == 1:
            ret.append(include)
        return ret

    def __str__(self):
        at = "X" if self.dead else "@"
        return f"<{self.type} {at} {normal(self)} - HP: {self.hp:>3d}>"

    def __repr__(self):
        return str(self)

result = 0

units = []
enemies = {"G":[], "E":[]}

frame = []
with open("input.txt", "r") as input:
    for line in input:
        line = line.strip().split()[0]
        frame.append(line)

def isSolid(p):
    return frame[p.y][p.x] == "#" or (p in units and not all([q.dead for q in units if q == p]))

def listStr(l, indent = 0):
    return "\n".join([" " * indent + str(e) for e in l])

for (y, line) in enumerate(frame):
    for (x, c) in enumerate(line):
        if c == "G":
            g = Unit(x, y, frame=frame, solid=isSolid, unitType="G")
            units.append(g)
            enemies["E"].append(g)
        elif c == "E":
            e = Unit(x, y, frame=frame, solid=isSolid, unitType="E")
            units.append(e)
            enemies["G"].append(e)

def distanceAndStep(start, end, maxd):
    #start is a unit, end is a target position (nonsolid or start)
    # if start == Position(5,1) and end == Position(1,2):
    #     breakpoint()
    if start == end:
        return (0, start)
    visited = [start]
    current = Queue()
    current.put((start, 0, end))
    while not current.empty():
        (currentPos, steps, firstStep) = current.get()
        neighs = currentPos.gridAdj()
        if end in currentPos.gridAdj():
            return (steps + 1, firstStep)
        if steps >= maxd:
            return (None, None)
        else:
            neighs.sort()
            for neigh in neighs:
                if neigh not in visited:
                    if steps == 0:
                        firstStep = neigh
                    current.put((neigh, steps + 1, firstStep))
                    visited.append(neigh)
    return (None, None)

def distanceAndStepList(start, end):
    #start is a unit, end is a list of target positions (nonsolid or start)
    # if start == Position(5,1) and end == Position(1,2):
    #     breakpoint()
    if start in end:
        return [(0, start, start)]
    visited = [start]
    current = Queue()
    current.put((start, 0, start))
    returnValues = []
    returnDistance = 99999
    while not current.empty():
        (currentPos, steps, firstStep) = current.get()
        if returnDistance < steps + 1:
            return returnValues
        
        neighs = currentPos.gridAdj()
        neighs.sort()
        for neigh in neighs:
            if steps == 0:
                firstStep = neigh
            if neigh in end:
                returnValues.append((steps + 1, neigh, firstStep))
                returnDistance = steps + 1
            elif neigh not in visited:
                current.put((neigh, steps + 1, firstStep))
                visited.append(neigh)
    return returnValues

# solid = "#"
empty = path


def createImage(oldUnits):
    positions = [a for (a, b) in oldUnits if not b.dead]
    units = [b for (_, b) in oldUnits if not b.dead]

    deadPositions = [a for (a, b) in oldUnits if b.dead]
    deadUnits = [b for (_, b) in oldUnits if b.dead]
    
    imageList = ["   " + "".join([str(x % 10) for x in range(len(frame[0]))])]
    for y in range(len(frame)):
        imagerowList = [f"{y:>2d} "]
        hpList = []
        for x in range(len(frame[0])):
            p = Position(x,y)
            if p in units:
                i = units.index(p)
                unit = units[i]
                imagerowList.append(unit.type)
                hpList.append(f"{unit.hp:3d}")
            elif p in deadUnits:
                imagerowList.append("X")
            elif p in positions:
                i = positions.index(p)
                t = units[i]
                arrow = dirToArrow(stepOrientation(p, t))
                imagerowList.append(arrow)
            elif frame[y][x] == "#":
                imagerowList.append(solid)
            else:
                imagerowList.append(empty)
        imagerowList.append(" " + " ".join(hpList))
        imageList.append("".join(imagerowList))
    return "\n".join(imageList)

oldUnits = [(deepcopy(u), u) for u in units if not u.dead]

rounds = 0
while not all([e.dead for e in enemies["E"]]) and not all([e.dead for e in enemies["G"]]):
# for _ in range(1):
    roundFinished = True
    units.sort()
    printLog(f"Units at round {rounds:>2d}:\n", listStr([u for u in units if not u.dead], indent=2))
    printLog()

    image.write(f"ROUND {rounds:>2d}\n")
    image.write(createImage(oldUnits))
    image.write("\n\n")
    oldUnits = [(deepcopy(u), u) for u in units if not u.dead]


    for unit in units:
        if unit.dead:
            continue
        printLog("Current Unit: ", unit)
        enemyList = [e for e in enemies[unit.type] if not e.dead]
        enemyList.sort(key=lambda e: unit.distance(e))
        # printLog("Enemy List for current unit:\n", listStr(enemyList, indent=2))

        if len(enemyList) == 0:
            printLog("No enemies, ending")
            printLog()
            roundFinished = False
            break

        targets = set()
        for e in enemyList:
            targets.update(e.gridAdj(include=unit))
        distances = distanceAndStepList(unit, targets)
        if len(distances) == 0:
            printLog("No reachable enemies")
            printLog("Unit did not move")
            printLog()
            continue

        distances.sort()
        printLog("Distances array (distance, targetTile, firstStep):\n", listStr([(d, normal(t), normal(f)) for (d,t,f) in distances], indent=2))
        (distance, _, firstStep) = distances[0]
        unit.moveTo(firstStep)
        if distance == 0:
            printLog("Unit did not move")
        else:
            printLog("Unit moved to ", normal(unit))

        targettableEnemies = [(e.hp, e) for e in enemyList if unit.distance(e) == 1]
        targettableEnemies.sort()
        if len(targettableEnemies) > 0:
            printLog("Targettable Enemies:\n", listStr(targettableEnemies, indent=2) )
            (_, e) = targettableEnemies[0]
            printLog("Attacking unit ", e)
            unit.attack(e)
            printLog("Result: ", e)
        printLog()
        # image.write(createImage(oldUnits))
        # image.write("\n\n")
    printLog()
    if roundFinished:
        rounds += 1
        if rounds % 10 == 0:
            print(rounds)

image.write(createImage(oldUnits))
image.write("\n\n")

printLog(f"Units at round {rounds:03d}:\n", listStr([u for u in units if not u.dead], indent=2))

E = "E"
print(f"Elves dead with power {POWER}: {len([u for u in units if u.dead and u.type == E])}")
result = sum([u.hp for u in units if not u.dead])

result *= rounds

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

