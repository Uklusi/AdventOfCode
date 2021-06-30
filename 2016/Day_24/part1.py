from queue import PriorityQueue
from AOCClasses import Position, gridDistance
from functools import partial
from itertools import permutations
result = 999999

def aStar(start, goal, func):
    global maze
    estimate = partial(func, goal)
    openSet = PriorityQueue()
    distance = {start: 0}
    counter = 0
    openSet.put((estimate(start) + distance[start], counter, start))
    counter += 1

    while not openSet.empty():
        if goal in distance:
            return distance[goal]
        (_, _, current) = openSet.get()
        for p in current.gridAdj():
            if maze[p] and p not in distance:
                distance[p] = distance[current] + 1
                openSet.put((estimate(p) + distance[p], counter, p))
                counter += 1
    
    return -1

row = -1
positions = {}
maze = {}
with open("input.txt", "r") as input:
    for line in input:
        row += 1
        line = line.strip()
        for (column, c) in enumerate(line):
            p = Position(row, column)
            if c == "#":
                maze[p] = False
            else:
                maze[p] = True
                if c != ".":
                    positions[int(c)] = p

distances = {}
for n in positions:
    distances[n] = {}
    for m in positions:
        if m != n:
            if m in distances:
                distances[n][m] = distances[m][n]
            else:
                distances[n][m] = aStar(positions[n], positions[m], gridDistance)
        
m = max(positions.keys()) + 1

for perm in permutations(range(1, m)):
    total = 0
    for i in range(len(perm)):
        n = perm[i]
        m = 0 if i == 0 else perm[i-1]
        total += distances[m][n]
    result = min(result, total) 


with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

