from AOCClasses import *

result = ""

data = []
with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        p = Position(*map(int,line[10:24].split(",")))
        v = Position(*map(int,line[36:42].split(",")))
        data.append([p,v])

for i in range(10011, 10012):
    data2 = {p + i * v: True for (p,v) in data}

    px = list(map(lambda p: p.x, data2.keys()))
    py = list(map(lambda p: p.y, data2.keys()))
    minx = min(px)
    maxx = max(px) + 1
    miny = min(py)
    maxy = max(py) + 1

    ret = "\n".join(
        [ "".join(
            [solid if Position(i,j) in data2 else empty for i in range(minx, maxx)]
        ) for j in range(miny, maxy) ]
    )
result = ret

with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

