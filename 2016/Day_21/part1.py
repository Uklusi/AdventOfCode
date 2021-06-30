start = "abcdefgh"
result = start

def swapPos(s, x, y):
    l = list(s)
    l[x] = s[y]
    l[y] = s[x]
    return "".join(l)

def swapLetter(s, x, y):
    return s.replace(x, "#").replace(y, x).replace("#", y)

def rotateLeft(s, x):
    return s[x:] + s[:x]

def rotateRight(s, x):
    return s[-x:] + s[:-x]

def rotateLetter(s, x):
    i = s.index(x)
    i = i + 1 + (1 if i >= 4 else 0)
    i = i % len(s)
    return rotateRight(s, i)

def reverse(s):
    return "".join(reversed(s))

def reversePositions(s, x, y):
    return s[:x] + reverse(s[x:y+1]) + s[y+1:]

def move(s, x, y):
    c = s[x]
    if x < y:
        return s[:x] + s[x+1:y+1] + c + s[y+1:]
    else:
        return s[:y] + c + s[y:x] + s[x+1:]

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip().split()
        if line[0] == "swap":
            if line[1] == "position":
                result = swapPos(result, int(line[2]), int(line[5]))
            else:
                result = swapLetter(result, line[2], line[5])
        elif line[0] == "rotate":
            if line[1]== "left":
                result = rotateLeft(result, int(line[2]))
            elif line[1]== "right":
                result = rotateRight(result, int(line[2]))
            else:
                result = rotateLetter(result, line[6])
        elif line[0] == "reverse":
            result = reversePositions(result, int(line[2]), int(line[4]))
        else:
            result = move(result, int(line[2]), int(line[5]))

with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

