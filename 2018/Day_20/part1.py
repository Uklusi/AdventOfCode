from AoCUtils import *
from collections import defaultdict
# import re

# Assumptions:
# NO LOOPS
# No things like "^N(E|W)N$" where we have two branching paths to control simultaneously
# The only things not excluded are "^N(EW|)N" where the bracked ends with | and the other option is nullpotent

result = 0

with open("input.txt", "r") as input:
    data = input.read().strip()

distances = defaultdict(lambda: +inf)
doors = set()
a = Agent(0,0)

distances[a.position()] = 0

i = 1
def iterTravel(a, steps):
    global i
    global distances
    currentAgent = a.copy()
    currentSteps = steps
    while data[i] not in ")$":
        char = data[i]
        i = i + 1
        if char == "(":
            iterTravel(currentAgent, currentSteps)
        elif char == "|":
            currentAgent = a.copy()
            currentSteps = steps
        else:
            currentAgent.move(direction=char)
            doors.add(currentAgent.position())
            currentAgent.move(direction=char)
            currentSteps += 1
            p = currentAgent.position()
            distances[p] = min(distances[p], currentSteps)
    i = i+1

iterTravel(a, 0)

xvals = list(map(lambda p: p.x, distances.keys()))
yvals = list(map(lambda p: p.y, distances.keys()))
frame = [0 for y in range(min(yvals)-1, max(yvals)+2)]
frame[0] = [0 for x in range(min(xvals)-1, max(xvals)+2)]

def visual(p):
    p = p + Vector(min(xvals) - 1, min(yvals) - 1)
    if p == Position(0,0):
        return "X"
    if p in distances or p in doors:
        return empty
    return solid

# castleMap = Map(frame=frame, visual=visual)

# print(castleMap)

result = max(distances.values())




with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

