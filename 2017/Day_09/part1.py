import re

result = 0

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        stream = line

stream = re.sub(r"!.", "", stream)
stream = re.sub(r"<[^>]*>", "", stream)
stream = re.sub(r"[^{}]", "", stream)

value = 0
for c in stream:
    if c == "{":
        value += 1
        result += value
    elif c == "}":
        value -= 1

with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

