result = 0

boxIds = []
with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        boxIds.append(line)

for (i, id1) in enumerate(boxIds):
    for j in range(i+1, len(boxIds)):
        id2 = boxIds[j]
        errors = 0
        for k in range(len(id1)):
            if id1[k] != id2[k]:
                errors += 1
                errnum = k
        if errors == 1:
            result = id1[:errnum] + id1[errnum+1:]
            break

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

