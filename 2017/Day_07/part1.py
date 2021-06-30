"""
uwzmqi (57)
emlzcpy (106) -> pwmoihf, sdwnkb
"""
result = 0
tower = {}

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip().split(" -> ")
        name, w = line[0].split()
        n = int(w[1:-1])
        tower[name] = {}
        tower[name]["weight"] = n
        tower[name]["over"] = []
        tower[name]["under"] = []
        if len(line) > 1:
            over = line[1].split(", ")
            tower[name]["over"] = over

for name in tower.keys():
    for n in tower[name]["over"]:
        tower[n]["under"].append(name)

for name in tower:
    if len(tower[name]["under"]) == 0:
        result = name
        break


with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

