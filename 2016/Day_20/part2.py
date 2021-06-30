result = 0
minIp = 0
blockedIps = []

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        limits = [int(n) for n in line.split("-")]
        blockedIps.append({"min": limits[0], "max": limits[1]})

blockedIps.sort(key=lambda x: x["min"])
for ipRange in blockedIps:
    if minIp < ipRange["min"]:
        result += ipRange["min"] - minIp
    minIp = max(minIp, ipRange["max"] + 1)

result += 2**32 - minIp

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

