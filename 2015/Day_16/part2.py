result = 0
"""Sue 500: pomeranians: 10, cats: 3, vizslas: 5"""
sues = []

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip().replace(":", "").replace(",", "")
        data = line.split()
        keys = [s for (i, s) in enumerate(data) if i % 2 == 0]
        values = [int(s) for (i, s) in enumerate(data) if i % 2 == 1]
        sues.append({k: v for (k, v) in zip(keys, values)})

presentInfo = {
    "children": 3,
    "cats": 7,
    "samoyeds": 2,
    "pomeranians": 3,
    "akitas": 0,
    "vizslas": 0,
    "goldfish": 5,
    "trees": 3,
    "cars": 2,
    "perfumes": 1
}
gtarray = ["cats", "trees"]
ltarray = ["pomeranians", "goldfish"]

def checkSues():
    for sue in sues:
        flag = True
        for info in presentInfo:
            if info in sue:
                if info in gtarray and sue[info] <= presentInfo[info]:
                    flag = False
                elif info in ltarray and sue[info] >= presentInfo[info]:
                    flag = False
                elif info not in gtarray + ltarray and sue[info] != presentInfo[info]:
                    flag = False
        if flag == True:
            return sue["Sue"]

result = checkSues()

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

