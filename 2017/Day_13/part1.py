result = 0

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        (d, r) = [int(n) for n in line.split(": ")]
        if d % (2 * r - 2) == 0:
            result += d * r

with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

