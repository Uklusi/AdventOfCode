result = 0

def calcCheck(check, mistery):
    offset = mistery & 255
    check = check + offset
    check = check & 16777215
    check = check * 65899
    check = check & 16777215
    return check

def recalcMistery(mistery):
    return mistery // 256

check = 0
seen = set()
while True:
    mistery = check | 65536
    check = 8586263
    check = calcCheck(check, mistery)
    while (256 <= mistery):
        mistery = recalcMistery(mistery)
        check = calcCheck(check, mistery)
    if (check == 0):
        break
    elif check in seen:
        break
    else:
        seen.add(check)
        result = check


with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

