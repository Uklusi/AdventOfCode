result = 0

lines = []
with open("input.txt", "r") as input:
    for line in input:
        line = [int(n) for n in line.strip().split()]
        line.sort()
        lines.append(line)

for line in lines:
    for i in range(len(line)):
        m = line[i]
        for n in line[i+1:]:
            if n % m == 0:
                result += n // m

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

