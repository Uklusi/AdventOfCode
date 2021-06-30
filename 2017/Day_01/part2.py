result = 0

s = ""
with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        s = line

l = len(s) // 2
for i in range(l):
    if s[i] == s[i+l]:
        result += 2* int(s[i])

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

