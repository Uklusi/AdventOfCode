from AOCClasses import HexGrid

result = 0

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        instructions = line.split(",")

p = HexGrid(0,0)
for instruction in instructions:
    p.move(1, instruction)
    result = max(result, HexGrid(0,0) - p)
    
with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

