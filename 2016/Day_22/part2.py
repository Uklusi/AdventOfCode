from AOCClasses import Position
result = 0
grid = {}

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        if line[0] != "/":
            continue
        line = line.split()
        label = line[0].split("-")
        pos = Position(x=int(label[1][1:]), y=int(label[2][1:]), xmin=0, ymin=0, xmax=30, ymax=30)
        used = int(line[2][:-1])
        avail = int(line[3][:-1])
        grid[pos] = {"used": used, "avail": avail}

s = ""
for p, pdata in grid.items():
    if p.y == 0:
        s += "\n"
    u = pdata["used"]
    if u == 0:
        s += "_"
    elif u > 92:
        s += "#"
    else:
        s += "."

print(s)

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

