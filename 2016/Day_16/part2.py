result = 0

def reverse(string):
    return "".join(reversed(string))

def invert(string):
    return "".join(["1" if c == "0" else "0" for c in string])

def step(string):
    return string + "0" + invert(reverse(string))

def checksum(string):
    while len(string) % 2 == 0:
        s1 = ""
        l1 = len(string) // 2
        for i in range(l1):
            s1 += "1" if string[2*i] == string[2*i + 1] else "0"
        string = s1
    return string

input = "10111100110001111"
toFill = 35651584

string = input
while len(string) < toFill:
    string = step(string)

string = string[:toFill]

result = checksum(string)

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

