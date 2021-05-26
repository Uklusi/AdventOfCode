result = 0

table = []

time = 2503

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        words = line.split(" ")
        name = words[0]
        deer = {"name": name}
        deer["speed"] = int(words[3])
        deer["duration"] = int(words[6])
        deer["rest"] = int(words[13])
        deer["cycle"] = deer["duration"] + deer["rest"]
        deer["distcycle"] = deer["duration"] * deer["speed"]
        deer["currdist"] = 0
        deer["score"] = 0
        table.append(deer)

for i in range(time):
    for deer in table:
        if i % deer["cycle"] < deer["duration"]:
            deer["currdist"] += deer["speed"]
    maxdist = max([deer["currdist"] for deer in table])
    winners = [j for (j, deer) in enumerate(table) if deer["currdist"] == maxdist]
    for j in winners:
        table[j]["score"] += 1

maxscore = max([deer["score"] for deer in table])
result = maxscore
print([{"score": deer["score"], "dist": deer["currdist"]} for deer in table])

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

