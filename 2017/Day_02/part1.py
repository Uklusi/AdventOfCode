result = 0

with open("input.txt", "r") as input:
    for line in input:
        line = [int(n) for n in line.strip().split()]
        line.sort()
        result += line[-1] - line[0]

with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

