result = 0
conts = []
value = 150

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        conts.append(int(line))

conts.sort(reverse=True)

minConts = 999
possibNumbers = 0

def setNumber(numconts):
    global minConts
    global possibNumbers
    if numconts < minConts:
        minConts = numconts
        possibNumbers = 1
    elif numconts == minConts:
        possibNumbers += 1
    return

def countPossibilities(val, conts, numconts):
    if val == 0:
        setNumber(numconts)
    elif val < 0:
        pass
    elif len(conts) == 0:
        pass
    else:
        countPossibilities(val-conts[0], conts[1:], numconts+1)
        countPossibilities(val, conts[1:], numconts)
    return

countPossibilities(value, conts, 0)
result = possibNumbers

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

