result = 0

bridges = []

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        bridges.append([int(n) for n in line.split("/")])

maxlen = 0
def search(bridges, current, total, totlen):
    global result
    global maxlen
    for bridge in bridges:
        if current in bridge:
            i = bridge.index(current)
            other = bridge[1 - i]
            bridgesNew = bridges.copy()
            bridgesNew.remove(bridge)
            totalNew = total + sum(bridge)
            search(bridgesNew, other, totalNew, totlen + 1)
    if totlen > maxlen:
        maxlen = totlen
        result = total
    elif totlen == maxlen:
        result = max(result, total)

search(bridges, 0, 0, 0)

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

