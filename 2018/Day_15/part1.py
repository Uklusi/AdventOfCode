from AOCClasses import *
from queue import SimpleQueue as Queue

log = open("logp1.txt", "w")
image = open("imagep1.txt", "w")

def printLog(txt="", other=""):
    txt = str(txt)
    other = str(other)
    log.write(txt + other + "\n")

def normal(val):
    if isinstance(val, tuple):
        return f"({val[0]: 2d}, {val[1]: 2d})"
    return normal(val.coords(inverted=True))


class Unit(SolidPosition):
    def __init__(self, x,y,frame=None, solid=None, unitType=None):
        super().__init__(x,y, reverseY=True, frame=frame, solid=solid)
        self.hp = 200
        self.atk = 3
        self.type = unitType
        self.dead = False

    def moveTo(self, pos):
        q = pos - self
        if q.distance() == 0:
            return
        elif q.distance() != 1:
            raise(Exception("Wrong moveTo"))
        elif q.y == -1:
            self.move(1, "U")
        elif q.y == 1:
            self.move(1, "D")
        elif q.x == -1:
            self.move(1, "L")
        elif q.x == 1:
            self.move(n=1, direction="R")
        else:
            raise(Exception("Wrong moveTo"))

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
        line = line.strip()
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
    # if start == Position(4,1) and end == {Position(5,5)}:
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

def createImage():
    imageList = []
    for y in range(len(frame)):
        imagerowList = []
        hpList = []
        for x in range(len(frame[0])):
            p = Position(x,y)
            if p in unitsNotDead:
                i = unitsNotDead.index(p)
                imagerowList.append(unitsNotDead[i].type)
                hpList.append(f"{unitsNotDead[i].hp:>3d}")
            elif frame[y][x] == "#":
                imagerowList.append(solid)
            else:
                imagerowList.append(empty)
        imagerowList.append(" " + " ".join(hpList))
        imageList.append("".join(imagerowList))
    return "\n".join(imageList)


rounds = 0
while not all([e.dead for e in enemies["E"]]) and not all([e.dead for e in enemies["G"]]):
# for _ in range(1):
    roundFinished = True
    units.sort()
    printLog(f"Units at round {rounds:03d}:\n", listStr([u for u in units if not u.dead], indent=2))
    printLog()
    
    image.write(f"ROUND {rounds:>2d}\n")
    unitsNotDead = [u for u in units if not u.dead]
    # if rounds >= 23:
    #     breakpoint()
    
    image.write(createImage())
    image.write("\n\n")

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
    printLog()
    if roundFinished:
        rounds += 1
    print(rounds)

image.write(createImage())
image.write("\n\n")

printLog(f"Units at round {rounds:03d}:\n", listStr([u for u in units if not u.dead], indent=2))

result = sum([u.hp for u in units if not u.dead])

result *= rounds

with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

