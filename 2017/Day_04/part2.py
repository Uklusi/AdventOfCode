result = 0

passphrases = []

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip().split()
        passphrases.append(line)

def sortString(s):
    l = list(s)
    l.sort()
    return "".join(l)

for phrase in passphrases:
    phrase = [sortString(w) for w in phrase]
    result += len(phrase) == len(set(phrase))

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

