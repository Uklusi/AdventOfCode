import hashlib

result = ""
input = "reyedfim"

index = 0

def md5(string):
    return hashlib.md5(string.encode()).hexdigest()

while len(result) < 8:
    hash = md5(input + str(index))
    if hash[0:5] == "00000":
        result += hash[5]
    index += 1

with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

