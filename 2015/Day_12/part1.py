import re

result = 0
data = ""

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        data += line

regex = re.compile(r'[^0-9-]+')
data = regex.sub(",", data)
result = sum([int(x) if x != "" else 0 for x in data.split(",")])


with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

