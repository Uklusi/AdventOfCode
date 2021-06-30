result = 0

bridges = []

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        bridges.append([int(n) for n in line.split("/")])

def search(bridges, current, total):
    global result
    for bridge in bridges:
        if current in bridge:
            i = bridge.index(current)
            other = bridge[1 - i]
            bridgesNew = bridges.copy()
            bridgesNew.remove(bridge)
            totalNew = total + sum(bridge)
            search(bridgesNew, other, totalNew)
    result = max(result, total)

search(bridges, 0, 0)

with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

