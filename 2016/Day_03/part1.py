result = 0
triangles = []

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        nums = [int(n) for n in line.split()]
        nums.sort()
        triangles.append(nums)

for triangle in triangles:
    if triangle[0] + triangle[1] > triangle[2]:
        result += 1

with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

