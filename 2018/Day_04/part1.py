from collections import defaultdict

result = 0

with open("input.txt", "r") as input:
    lines = input.read().strip().split("\n")
    lines.sort()

guard = 0

guardData = defaultdict(lambda: defaultdict(lambda: 0))
for line in lines:
    ts = line[:18]
    other = line[19:].split()
    if other[0] == "Guard":
        guard = int(other[1][1:])
    elif other[0] == "falls":
        asleep = int(ts[-3:-1])
    else:
        awake = int(ts[-3:-1])
        for i in range(asleep, awake):
            guardData[guard][i] += 1
        guardData[guard]["total"] += awake - asleep

mostSleeping = max(guardData.keys(), key=lambda g: guardData[g]["total"])

minuteSleeping = max(range(60), key=lambda m: guardData[mostSleeping][m])
    
result = mostSleeping * minuteSleeping

with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

