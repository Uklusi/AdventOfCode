result = 0

nums = []

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        nums.append(int(line))

found = {}
i = 0
while result not in found:
    found[result] = 1
    result += nums[i]
    i = (i+1) % len(nums)

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

