result = 0

jumps = []
with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        jumps.append(int(line))

pos = 0

while pos >= 0 and pos < len(jumps):
    n = jumps[pos]
    jumps[pos] += 1
    pos += n
    result += 1

with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

