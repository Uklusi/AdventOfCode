result = 0

table = []

time = 2503

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        words = line.split(" ")
        name = words[0]
        reindeer = {"name": name}
        reindeer["speed"] = int(words[3])
        reindeer["duration"] = int(words[6])
        reindeer["rest"] = int(words[13])
        reindeer["cycle"] = reindeer["duration"] + reindeer["rest"]
        reindeer["distcycle"] = reindeer["duration"] * reindeer["speed"]
        table.append(reindeer)

dist = 0
for reindeer in table:
    ncycles = time // reindeer["cycle"]
    residtime = time % reindeer["cycle"]
    dist = ncycles * reindeer["distcycle"] + min(residtime, reindeer["duration"]) * reindeer["speed"]
    result = max(result, dist) 

with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

