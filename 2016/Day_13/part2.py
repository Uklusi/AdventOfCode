from AOCClasses import solid, empty, path, Position
from functools import cache
result = 0
start = Position(1,1)

input = 1350

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

visited = {start}
currStep = {start}


for _ in range(50):
    nextStep = set()
    for p in currStep:
        nextStep |= set(q for q in p.gridAdj() if not isSolid(q))
    nextStep -= visited
    visited |= nextStep
    currStep = nextStep

result = len(visited)

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

