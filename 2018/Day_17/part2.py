from AoCUtils import *
# from queue import SimpleQueue as Queue
"""
x=495, y=2..7
y=7, x=495..501
"""
result = 0

clay = set()

ymin = +inf
ymax = -inf

with open("input.txt", "r") as input:
    for line in input:
        (single, double) = line.strip().split(", ")
        single = single.split("=")
        double = double.split("=")
        single[1] = int(single[1])
        double[1] = [int(n) for n in double[1].split("..")]
        double[1] = range(double[1][0], double[1][1] + 1)
        if single[0] == "x":
            x = single[1]
            for y in double[1]:
                clay.add(Position(x,y))
                ymin = min(ymin, y)
                ymax = max(ymax, y)
        else:
            y = single[1]
            ymin = min(ymin, y)
            ymax = max(ymax, y)
            for x in double[1]:
                clay.add(Position(x,y))

water = set()
wet = set()

def travelDown(startPos):
    global clay
    global water
    global wet
    currentPos = startPos
    down = Vector(x=0, y=1)
    while True:
        nextPos = currentPos + down
        wet.add(currentPos)
        if nextPos in clay or nextPos in water:
            return currentPos
        if nextPos in wet or currentPos.y > ymax:
            return None
        currentPos = nextPos

def travelHoriz(startPos, direction):
    # startPos has a solid block (water or clay) underneath
    global clay
    global water
    global wet
    down = Vector(x=0, y=1)
    blocked = False
    currentPos = startPos
    count = 0
    while True:
        nextPos = currentPos + direction
        downPos = currentPos + down
        wet.add(currentPos)
        if downPos in clay or downPos in water:
            if nextPos in clay or nextPos in water:
                blocked = True
                break
            else:
                count += 1
                currentPos = nextPos
        else:
            blocked = False
            break
    return (currentPos, blocked, count)

def fillHoriz(startPos):
    global water
    up    = Vector(x= 0, y=-1)
    left  = Vector(x=-1, y= 0)
    right = Vector(x= 1, y= 0)

    (limitLeft,  blockedLeft,  countLeft ) = travelHoriz(startPos, left)
    (limitRight, blockedRight, countRight) = travelHoriz(startPos, right)
    if blockedLeft and blockedRight:
        for i in range(countLeft + countRight + 1):
            water.add(limitLeft + i * right)
        return ([startPos + up], "h")
    else:
        return (( [] if blockedLeft else [limitLeft] ) + ( [] if blockedRight else [limitRight] ), "d")

def fill(startPos):
    queue = []
    queue.append((startPos, "d"))
    while len(queue) > 0:
        (pos, direction) = queue.pop()
        if direction == "d":
            ret = travelDown(pos)
            if ret is not None:
                queue.append((ret, "h"))
        elif direction == "h":
            (ret, newDir) = fillHoriz(pos)
            for newPos in ret:
                queue.append((newPos, newDir))
        else:
            raise(Exception("I fucked up"))
        # printImage()

fill(Position(x=500, y=0))

result = len( {p for p in water if ymin <= p.y <= ymax} )

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

