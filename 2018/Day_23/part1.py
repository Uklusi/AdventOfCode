from AoCUtils import *
result = 0
"pos=<-61729268,-4319210,111177698>, r=52443495"

bots = []
with open("input.txt", "r") as input:
    for line in input:
        line = line.strip().split(">, r=")
        posStr = line[0][5:]
        pos = Position3D(*[int(n) for n in posStr.split(",")])
        rad = int(line[1])
        bots.append([rad, pos])

bots.sort(reverse=True)
current = bots[0]
currentRad = current[0]
currentPos = current[1]

for (_, p) in bots:
    if currentPos.distance(p) <= currentRad:
        result += 1


with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

