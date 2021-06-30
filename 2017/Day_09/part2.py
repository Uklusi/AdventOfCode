import re

result = 0

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        stream = line

stream = re.sub(r"!.", "", stream)
garbage = re.findall(r"<([^>]*)>", stream)

result = sum(map(len, garbage))

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

