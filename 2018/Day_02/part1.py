result = 0

boxIds = []
with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        boxIds.append(line)

count2 = 0
count3 = 0
for boxId in boxIds:
    flag2 = True
    flag3 = True
    for c in set(boxId):
        n = boxId.count(c)
        if n == 2 and flag2:
            count2 += 1
            flag2 = False
        elif n == 3 and flag3:
            count3 += 1
            flag3 = False

result = count2 * count3

with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

