result = 0

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        l = len(line)
        m = line.count("\\")
        m1 = line.count('"')
        result += m + m1 + 2

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

