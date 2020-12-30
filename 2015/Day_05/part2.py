import re

result = 0

double = re.compile( r"([a-z]{2}).*\1" )
repeat = re.compile( r"([a-z]).\1")

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        prop1 = double.search(line) is not None
        prop2 = repeat.search(line) is not None
        if prop1 and prop2:
            result += 1
    
with open("output2.txt", "w") as output:
    output.write( str(result) )
    print(str(result))