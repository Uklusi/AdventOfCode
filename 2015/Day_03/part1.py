result = 0

presents = {}

x = 0
y = 0

def iname(i, j):
    return str(i) + "-" + str(j)

presents[iname(x,y)] = 1

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        for char in line:
            if char == "^":
                y += 1
            elif char == "v":
                y += -1
            elif char == "<":
                x += 1
            elif char == ">":
                x += -1
            index = iname(x,y)
            if index not in presents:
                presents[index] = 1
            else:
                presents[index] += 1

result = len(presents.keys())
    
with open("output1.txt", "w") as output:
    output.write( str(result) )
    print(str(result))