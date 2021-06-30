result = 0
"""value 61 goes to bot 209
bot 200 gives low to bot 40 and high to bot 141"""

starting = {}
distribution = {}
inventory = {}

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip().split()
        if line[0] == "value":
            starting[int(line[1])] = int(line[5])
            inventory[int(line[5])] = []
        else:
            botlow = int(line[6]) + (0 if line[5] == "bot" else 1000)
            bothigh = int(line[11]) + (0 if line[10] == "bot" else 1000)
            distribution[int(line[1])] = {
                "low": botlow,
                "high": bothigh 
            }
            inventory[botlow] = []
            inventory[bothigh] = []

for (v, b) in starting.items():
    inventory[b].append(v)

fullBot = max(inventory.keys(), key=lambda x: len(inventory[x]))

while len(inventory[fullBot]) > 1:
    inventory[fullBot].sort()
    inventory[distribution[fullBot]["low"]].append(inventory[fullBot][0])
    inventory[distribution[fullBot]["high"]].append(inventory[fullBot][1])
    inventory[fullBot] = []
    fullBot = max(inventory.keys(), key=lambda x: len(inventory[x]))

result = inventory[1000][0]*inventory[1001][0]*inventory[1002][0]

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

