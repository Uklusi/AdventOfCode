result = 0

presents = {}

x = [0,0]
y = [0,0]

turn = 0

def iname(i, j):
    return str(i) + "-" + str(j)

presents[iname(0,0)] = 1

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        for char in line:
            if char == "^":
                y[turn] += 1
            elif char == "v":
                y[turn] += -1
            elif char == "<":
                x[turn] += 1
            elif char == ">":
                x[turn] += -1
            index = iname(x[turn],y[turn])
            if index not in presents:
                presents[index] = 1
            else:
                presents[index] += 1
            turn = 1 - turn

result = len(presents.keys())
    
with open("output1.txt", "w") as output:
    output.write( str(result) )
    print(str(result))