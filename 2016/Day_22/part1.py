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

for p, pdata in grid.items():
    for (q, qdata) in grid.items():
        if p != q:
            if pdata["used"] and pdata["used"] <= qdata["avail"]:
                result += 1
        
with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

