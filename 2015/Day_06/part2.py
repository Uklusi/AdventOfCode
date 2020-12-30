result = 0

lights = {}

def iname(i, j):
    return str(i) + "," + str(j)

def turnOn(startx, starty, endx, endy):
    for i in range(startx, endx + 1):
        for j in range(starty, endy + 1):
            name = iname(i,j)
            lights[name] = lights.get(name, 0) + 1
            

def turnOff(startx, starty, endx, endy):
    for i in range(startx, endx + 1):
        for j in range(starty, endy + 1):
            name = iname(i,j)
            lights[name] = max(lights.get(name, 0) - 1, 0)

def toggle(startx, starty, endx, endy):
    for i in range(startx, endx + 1):
        for j in range(starty, endy + 1):
            name = iname(i,j)
            lights[name] = lights.get(name, 0) + 2

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

result = sum(lights.values())

with open("output2.txt", "w") as output:
    output.write( str(result) )
    print(str(result))