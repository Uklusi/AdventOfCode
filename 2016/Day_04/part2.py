result = 0
"bxaxipgn-vgpst-qphzti-rdcipxcbtci-635[ipctx]"
rooms = []
with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        check = line[-6:-1]
        id = int(line[-10:-7])
        origname = line[:-11]
        line = origname.replace("-", "")
        line = list(line)
        line.sort()
        rooms.append({
            "origname": origname,
            "name": line,
            "id": id,
            "check": check
        })


def calcCheckSum(name):
    frequences = {}
    for l in name:
        frequences[l] = frequences.get(l, 0) + 1
    letters = []
    for (l, v) in frequences.items():
        letters.append(f'{v:02d}' + str(999-ord(l)))
    letters.sort(reverse=True)
    retvalue = ""
    for i in range(5):
        n = int(letters[i][2:])
        letter = chr(999-n)
        retvalue += letter
    return retvalue

for room in rooms:
    if calcCheckSum(room["name"]) == room["check"]:
        shift = room["id"] % 26
        decryptedName = ""
        for l in room["origname"]:
            if l == "-":
                decryptedName += " "
            else:
                n = ord(l)
                n += shift
                if n > ord("z"):
                    n -= 26
                decryptedName += chr(n)
        if decryptedName == "northpole object storage":
            result = room["id"]



with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

