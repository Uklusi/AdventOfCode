from AOCClasses import Position, SolidPosition
result = ""
maze = []

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip("\n")
        maze.append(line)

def isSolid(p):
    return maze[p.y][p.x] == " "

x = maze[0].index("|")

pos = SolidPosition(x, 0, orientation="N", solid=isSolid, frame=maze)

oldoldpos = Position(x, -1)
while True:
    oldpos = pos.copy()
    pos.move(1)
    c = maze[pos.y][pos.x]
    if c == "O":
        result += c
        break
    if pos == oldpos != oldoldpos:
        pos.turnRight()
        oldoldpos = oldpos.copy()
    elif pos == oldoldpos:
        pos.turnRight()
        pos.turnRight()
    else:
        if c.isalpha():
            result += c


with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

