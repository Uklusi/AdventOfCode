import re
import json

result = 0
data = ""

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        data += line


jsdata = json.loads(data)
# print(jsdata[0])
def calcsum(jsonObj):
    if isinstance(jsonObj, list):
        return sum([calcsum(o) for o in jsonObj])
    elif isinstance(jsonObj, dict):
        if "red" in jsonObj.values():
            return 0
        else:
            return sum([calcsum(o) for o in jsonObj.values()])
    elif isinstance(jsonObj, int):
        return jsonObj
    else:
        return 0

result = calcsum(jsdata)

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))



