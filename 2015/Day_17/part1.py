result = 0
conts = []
value = 150

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        conts.append(int(line))

conts.sort(reverse=True)

def countPossibilities(val, conts):
    if val == 0:
        return 1
    elif val < 0:
        return 0
    elif len(conts) == 0:
        return 0
    else:
        return countPossibilities(val-conts[0], conts[1:]) + \
            countPossibilities(val, conts[1:])

result = countPossibilities(value, conts)

with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

