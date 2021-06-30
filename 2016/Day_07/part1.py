import re

result = 0
ABBA = r'([a-z])(?!\1)([a-z])\2\1'
outBracket = re.compile(ABBA) # can be made more precise
inBracket = re.compile(r'\[[^\]]*' + ABBA + r'[^\[]*\]')

ips = []
with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        ips.append(line)

for ip in ips:
    if not inBracket.search(ip) and outBracket.search(ip):
        result += 1


with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

