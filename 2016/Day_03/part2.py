result = 0
triangles = []

with open("input.txt", "r") as input:
    i = 0
    rows = []
    for line in input:
        line = line.strip()
        nums = [int(n) for n in line.split()]
        rows.append(nums)
        i += 1
        if i == 3:
            triangles += [[rows[j][k] for j in range(3)] for k in range(3)]
            i = 0
            rows = []

for triangle in triangles:
    triangle.sort()
    if triangle[0] + triangle[1] > triangle[2]:
        result += 1

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

