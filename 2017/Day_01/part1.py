result = 0

s = ""
with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        s = line

for (c, d) in zip(s, s[1:]+s[0]):
    if c == d:
        result += int(c)

with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

