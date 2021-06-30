from AOCClasses import Position
from itertools import product
from collections import defaultdict
result = 0

claims = []
"#1 @ 551,185: 21x10"
with open("input.txt", "r") as input:
    for line in input:
        line = line.strip().split()
        startp = Position(*map(int, line[2].strip(":").split(",")))
        dims = [int(n) for n in line[3].split("x")]
        claims.append({"startp": startp, "dims": dims})

posClaimed = defaultdict(lambda: 0)
for (cId, claim) in enumerate(claims):
    startp = claim["startp"]
    dims = claim["dims"]
    for (i,j) in product(range(dims[0]), range(dims[1])):
        posClaimed[startp + Position(i,j)] += 1

for (cId, claim) in enumerate(claims):
    startp = claim["startp"]
    dims = claim["dims"]
    overlap = False
    for (i,j) in product(range(dims[0]), range(dims[1])):
        if posClaimed[startp + Position(i,j)] > 1:
            overlap = True
            break
    if not overlap:
        result = cId + 1
        break

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

