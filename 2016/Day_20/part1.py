result = 0
blockedIps = []

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        limits = [int(n) for n in line.split("-")]
        blockedIps.append({"min": limits[0], "max": limits[1]})

blockedIps.sort(key=lambda x: x["min"])
found = False
i = 0
while not found and i < len(blockedIps):
    ipRange = blockedIps[i]
    if result < ipRange["min"]:
        found = True
    else:
        result = max(result, ipRange["max"] + 1)
        i += 1

with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

