from AOCClasses import Position
result = 0

infected = {}
weakened = {}
flagged = {}

with open("input.txt", "r") as input:
    lines = input.read().strip().split('\n')
    n = len(lines) // 2
    y = n
    for line in lines:
        line = line.strip()
        x = -n
        for c in line:
            if c == "#":
                infected[(x,y)] = 1
            x += 1
        y += -1

pos = Position(0,0,orientation="N")

for _ in range(10000000):
    p = pos.current()
    if p in weakened:
        weakened.pop(p)
        infected[p] = 1
        result += 1
    elif p in infected:
        pos.turnRight()
        infected.pop(p)
        flagged[p] = 1
    elif p in flagged:
        pos.turnReverse()
        flagged.pop(p)
    else:
        pos.turnLeft()
        weakened[p] = 1
    pos.move()

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

