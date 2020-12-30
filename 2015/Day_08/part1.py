result = 0

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        line2 = eval(line)
        result += len(line) - len(line2)

with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

