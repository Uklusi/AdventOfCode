result = 0

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        n = int(line)
        result += n

with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

