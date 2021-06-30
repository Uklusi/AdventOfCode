"""
uwzmqi (57)
emlzcpy (106) -> pwmoihf, sdwnkb
"""
from functools import cache

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
        tower[name]["under"] = None
        if len(line) > 1:
            over = line[1].split(", ")
            tower[name]["over"] = over

for name in tower.keys():
    for n in tower[name]["over"]:
        tower[n]["under"] = name

for name in tower:
    if tower[name]["under"] is None:
        bottom = name
        break

@cache
def totalWeight(name):
    tot = tower[name]["weight"]
    for n in tower[name]["over"]:
        tot += totalWeight(n)
    return tot

for name in tower:
    tower[name]["totalWeight"] = totalWeight(name)

def solve(name):
    balances = [tower[n]["totalWeight"] for n in tower[name]["over"]]
    if min(balances) == max(balances):
        return name
    n = min(balances)
    if balances.count(n) > 1:
        n = max(balances)
    i = balances.index(n)
    return solve(tower[name]["over"][i])

problem = solve(bottom)

under = tower[problem]["under"]

problemWeight = tower[problem]["totalWeight"]
otherWeight = list({tower[n]["totalWeight"] for n in tower[under]["over"]} - {problemWeight})[0]
correctW = otherWeight - (problemWeight - tower[problem]["weight"])
result = correctW

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

