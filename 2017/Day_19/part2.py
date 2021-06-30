from AOCClasses import Position, SolidPosition
result = 0
maze = []

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip("\n")
        maze.append(line)

def isSolid(p):
    return maze[p.y][p.x] == " "

x = maze[0].index("|")

pos = SolidPosition(x, 0, orientation="N", solid=isSolid, frame=maze)

result = 1
oldoldpos = Position(x, -1)
while True:
    oldpos = pos.copy()
    pos.move(1)
    c = maze[pos.y][pos.x]
    if c == "O":
        result += 1
        break
    if pos == oldpos != oldoldpos:
        pos.turnRight()
        oldoldpos = oldpos.copy()
    elif pos == oldoldpos:
        pos.turnRight()
        pos.turnRight()
    else:
        result += 1

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

