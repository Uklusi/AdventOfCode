import hashlib
import re
from functools import cache
from AOCClasses import Position

result = ""

@cache
def md5(string):
    return hashlib.md5(string.encode()).hexdigest()

input = "rrrbmfta"

def directions(pos, path):
    dirs = md5(input + path)[:4]
    ret = []
    for i in range(4):
        if dirs[i] in "bcdef":
            ret.append(["U", "D", "L", "R"][i])
    if pos.x == 0 and "L" in ret:
        ret.remove("L")
    if pos.x == 3 and "R" in ret:
        ret.remove("R")
    if pos.y == 0 and "U" in ret:
        ret.remove("U")
    if pos.y == -3 and "D" in ret:
        ret.remove("D")
    return ret

start = Position(0,0)
target = Position(3, -3)

check = [(start, "")]

while result == "":
    new = []
    for (pos, path) in check:
        for d in directions(pos, path):
            newPos = pos.copy()
            newPos.move(1, d)
            newPath = path + d
            if newPos == target:
                result = newPath
                break
            else:
                new.append((newPos,newPath))
        if result != "":
            break
    check = new
    new = []



with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

