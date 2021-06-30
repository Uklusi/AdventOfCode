"Disc 1 has 7 positions; at time 0 it is at position 0"
from math import prod
result = 0

discList = []

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip().replace(",", "").replace("#", "") \
        .replace(".", "").replace("=", " ").split()
        disc = {
            "offset": int(line[1]),
            "npos": int(line[3]),
            "stime": int(line[7]),
            "spos": int(line[12])
        }
        discList.append(disc)

modulo = prod([d["npos"] for d in discList])

for d in discList:
    npos = d["npos"]
    m = modulo // npos
    m1 = pow(m, -1, npos)
    result += (d["stime"] - d["offset"] - d["spos"]) * m * m1

result %= modulo

with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

