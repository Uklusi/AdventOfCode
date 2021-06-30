result = 0

passphrases = []

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip().split()
        passphrases.append(line)

for phrase in passphrases:
    result += len(phrase) == len(set(phrase))

with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

