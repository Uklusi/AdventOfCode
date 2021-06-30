import re

result = 9999999
o = ord("a")
O = ord("A")
lettersRe = re.compile("|".join(
    [chr(o+i) + chr(O+i) for i in range(26)] + [chr(O+i) + chr(o+i) for i in range(26)]
))

string = ""

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        string = line

for n in range(26):
    l = chr(o+n)
    L = chr(O+n)
    removed = string.replace(l, "").replace(L, "")
    while lettersRe.search(removed):
        removed = lettersRe.sub("", removed)

    result = min(result, len(removed))

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

