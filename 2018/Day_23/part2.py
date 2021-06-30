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
        dis = pos.distance()
        bots.append([pos, rad, dis])

smin = []
smax = []
for [pos, rad, dis] in bots:
    smin.append(dis-rad)
    smax.append(dis+rad)
smin.sort()
smax.sort()
i = 0
j = 0
ranges = [[0, -inf]]
while (i < len(smin)) and (j < len(smax)):
    a = smin[i] if i < len(smin) else inf
    b = smax[j] if j < len(smax) else inf
    prevR = ranges[-1]
    if a < b: # it always holds that a != b
        ranges.append([prevR[0] + 1, a])
        i += 1
    else:
        ranges.append([prevR[0] - 1, b + 1])
        j += 1

# I feel extremely dirty doing this, but it works lol
result = max(ranges, key=lambda l: l[0])[1]

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

