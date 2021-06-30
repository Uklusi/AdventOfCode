import hashlib
import re
from functools import cache

result = 0
input = "ahsbgdzn"

@cache
def md5(string):
    return hashlib.md5(string.encode()).hexdigest()

threeChars = re.compile(r'(.)\1\1')

index = -1
counter = 0
while counter < 64:
    index += 1
    tempinput = input + str(index)
    match = threeChars.search(md5(tempinput))
    if match:
        c = match.group(1)
        searchKey = re.compile(c*5)
        for i in range(1000):
            keyinput = input +str(index + i + 1)
            if searchKey.search(md5(keyinput)):
                counter += 1
                break

result = index

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()

with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

