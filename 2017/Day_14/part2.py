from knotHash import knotHash
from AOCClasses import Position, SolidPosition, solid, empty
result = 0

input = "jzgqcdpd"
# input = "flqrgnkx"

input += "-"

hashlist = []

for i in range(128):
    hash_i = knotHash(input + str(i))
    n = int(hash_i, 16)
    bin_n = f"{n:0128b}"
    hashlist.append(bin_n)

def isSolid(p):
    x = p.x
    y = p.y
    return hashlist[y][x] == "0"

maze = SolidPosition(0,0, xmin=0, ymin=0, xmax=127, ymax=127, solid=isSolid)
visited = {}

for i in range(128):
    for j in range(128):
        p = Position(i, j)
        m = maze + p
        if m not in visited and not m.isSolid():
            result += 1
            queue = [m]
            while len(queue) > 0:
                n = queue.pop()
                visited[n] = 1
                for k in n.gridAdj():
                    if k not in visited:
                        queue.append(k)


with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

