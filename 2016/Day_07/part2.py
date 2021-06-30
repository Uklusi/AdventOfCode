import re

result = 0
ABA = r'([a-z])(?!\1)([a-z])\1'
BAB = r'\2\1\2'
ABA1 = r'([a-z])(?!\3)([a-z])\3'
BAB1 = r'\4\3\4'
startBracket = r'\[[^\]]*'
endBracket = r'[^\[]*\]'
outBracket = r'(?:[a-z]*|.*\][a-z]*)'
hasABA = re.compile( startBracket + ABA + endBracket + outBracket + BAB + \
                     r'|^' + outBracket + ABA1 + r'.*' + startBracket + BAB1 + endBracket)

ips = []
with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        ips.append(line)


for ip in ips:
    if hasABA.search(ip):
        result += 1



with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

