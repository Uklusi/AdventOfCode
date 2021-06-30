from AOCClasses import solid, empty, path, Position
from functools import cache
result = 0
target = Position(31,39)
# target= Position(7,4)
start = Position(1,1)

input = 1350
# input = 10

def f(pos):
    x = pos.x
    y = pos.y
    return x*x + 3*x + 2*x*y + y + y*y

@cache
def isSolid(pos):
    x = pos.x
    y = pos.y
    if x < 0 or y < 0:
        return True
    n = f(pos) + input
    count = 0
    while n > 0:
        if n % 2:
            count += 1
        n //= 2
    if count % 2:
        return True
    else:
        return False

toRun = [([], start)]
toRunNext = []

def runMaze(singleRun):
    visited = singleRun[0]
    current = singleRun[1]
    if current in visited:
        return False
    if current == target:
        return True
    global toRunNext
    toRunNext += [(visited + [current], neigh) for neigh in current.gridAdj() if not isSolid(neigh)]
    return False

stop = False
while not stop:
    while len(toRun) > 0:
        singleRun = toRun.pop()
        toTarget = runMaze(singleRun)
        if toTarget:
            stop = True
    if stop:
        break
    result += 1
    toRun = toRunNext
    toRunNext = []



# string = "\n".join(
#     ["".join(
#         [solid if isSolid(Position(i, j)) else empty for i in range(10)]
#     ) for j in range(7)]
# )

# print(string)

with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

