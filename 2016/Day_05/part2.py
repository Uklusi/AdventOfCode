import hashlib

result = ""
input = "reyedfim"

index = 0
components = {}

def md5(string):
    return hashlib.md5(string.encode()).hexdigest()

while len(components) < 8:
    hash = md5(input + str(index))
    if hash[0:5] == "00000":
        pos = hash[5]
        if "0" <= pos <= "7":
            pos = int(pos)
            if pos not in components:
                components[pos] = hash[6]
                print("".join([components[i] if i in components else "_" for i in range(8)]))
    index += 1

result = "".join([components[i] for i in range(8)])

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

