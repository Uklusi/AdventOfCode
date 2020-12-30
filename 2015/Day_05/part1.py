import re

result = 0

double = re.compile( r"([a-z])\1" )

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        prop1 = sum( map( line.count, ["a", "e", "i", "o", "u"] ) ) >= 3
        prop2 = double.search(line) is not None
        prop3 =  not any( map( line.__contains__, ["ab", "cd", "pq", "xy"] ) )
        if prop1 and prop2 and prop3:
            result += 1
    
with open("output1.txt", "w") as output:
    output.write( str(result) )
    print(str(result))