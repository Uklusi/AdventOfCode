"""
The first floor contains a polonium generator, a thulium generator, a thulium-compatible microchip, a promethium generator, a ruthenium generator, a ruthenium-compatible microchip, a cobalt generator, and a cobalt-compatible microchip.
The second floor contains a polonium-compatible microchip and a promethium-compatible microchip.
The third floor contains nothing relevant.
The fourth floor contains nothing relevant.
"""
# Did I ever tell you that I hate parsing?
from copy import deepcopy
result = 0
minSteps = 999999999
absoluteMinSteps = 99999999

floors = []
tot = 0
with open("input.txt", "r") as input:
    for line in input:
        line = line.strip().strip(".").replace(",", "").replace(" and ", " ")
        currFloor = []
        if "nothing" not in line: # There is something to parse
            ind = line.index(" a ") + 3
            line = line[ind:].split(" a ")
            for el in line:
                el = el.split()
                if el[1] == "microchip":
                    el[0] = el[0].split("-")[0]
                currFloor.append(el)
        floors.append(currFloor)
        tot += len(currFloor)

floors[0] += [["elerium", "generator"], ["elerium", "microchip"], ["dilithium", "generator"], ["dilithium", "microchip"]]

def checkValidity(floors):
    for floor in floors:
        chips = [el[0] for el in floor if el[1] == "microchip"]
        gens  = [el[0] for el in floor if el[1] == "generator"]
        for chip in chips:
            if len(gens) > 0 and chip not in gens:
                return False
    return True

def solve(floors, floorNumber, steps):
    floors = deepcopy(floors)
    global minSteps
    global tot
    global absoluteMinSteps
    if len(floors[-1]) == tot:
        minSteps = min(minSteps, steps)
        return #(True, True, steps)
    n = len(floors) - 1
    missing = 0
    missingSteps = 0
    wrongPosition = False
    for i in range(n):
        floorMissing = len(floors[i])
        if floorNumber > i and not wrongPosition:
            missingSteps += floorNumber - i
            wrongPosition = True
            floorMissing += 1
        elif floorNumber == i and wrongPosition:
            floorMissing -= 1
        missing += floorMissing
        missingSteps += 2*missing - 3
    absoluteMinSteps = min(absoluteMinSteps, steps + missingSteps)
    if minSteps < steps + missingSteps:
        return 
    # Still missing the actual solving part
    
    

solve(floors, 0, 0)

result = absoluteMinSteps          

# result = absoluteMinSteps for some reason :/
# I am absolutely perplexed, the chip-generator constraint is useless
# I mean, it is not in the example, so why is it in the PROBLEM?

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

