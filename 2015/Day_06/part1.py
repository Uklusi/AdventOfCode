result = 0

lights = set()

def iname(i, j):
    return str(i) + "," + str(j)

def turnOn(startx, starty, endx, endy):
    for i in range(startx, endx + 1):
        lights.update(
            { iname(i, j) for j in range(starty, endy + 1) }
        )

def turnOff(startx, starty, endx, endy):
    for i in range(startx, endx + 1):
        lights.difference_update(
            { iname(i, j) for j in range(starty, endy + 1) }
        )

def toggle(startx, starty, endx, endy):
    for i in range(startx, endx + 1):
        lights.symmetric_difference_update(
            { iname(i, j) for j in range(starty, endy + 1) }
        )
        # print({ iname(i, j) for j in range(starty, endy + 1) })

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        words = line.split()
        if words[0] == "turn":
            words.remove("turn")
        (startx, starty) = [ int(n) for n in words[1].split(",") ]
        (endx, endy) = [ int(n) for n in words[3].split(",") ]
        if words[0] == "on":
            turnOn(startx, starty, endx, endy)
        elif words[0] == "off":
            turnOff(startx, starty, endx, endy)
        elif words[0] == "toggle":
            toggle(startx, starty, endx, endy)
        print(len(lights))

result = len(lights)

with open("output1.txt", "w") as output:
    output.write( str(result) )
    print(str(result))