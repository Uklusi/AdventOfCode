result = 0
row = []

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip().replace("^", "1").replace(".", "0")
        for c in line:
            row.append(int(c))

l = len(row)
h = 40

result += l - sum(row)
for _ in range(h - 1):
    newRow = []
    row = [0] + row + [0]
    for i in range(l):
        newRow.append(row[i] ^ row[i+2])
    result += l - sum(newRow)
    row = newRow


with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

