import re

result = 0
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

while lettersRe.search(string):
    string = lettersRe.sub("", string)

result = len(string)

with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

