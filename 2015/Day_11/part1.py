result = 0

input = "cqjxjnds"
l = len(input) - 1

# Rules: three consecutive letters (abc, bcd, ..., xyz)
# no i, o, l
# two different non-overlapping pairs (aa bb, ..)

longLetterGroups = ["abcdefgh", "pqrstuvwxyz"]
okLetters = "abcdefghjkmnpqrstuvwxyz"
okTriplets = [''.join(z) for group in longLetterGroups for z in zip(group, group[1:], group[2:]) ] 

def isOk(password):
    if all([trip not in password for trip in okTriplets]):
        return False
    if sum([c+c in password for c in okLetters]) < 2:
        return False
    return True

def letterReplace(word, index, letter):
    return word[:index] + letter + word[index+1:]

def nextInSequence(password):
    i = l
    flag = False
    while (not flag):
        if password[i] == "z":
            password = letterReplace(password, i, "a")
            i = i-1
        else:
            password = letterReplace(password, i, okLetters[okLetters.find(password[i]) + 1] )
            flag = True
        if i < 0:
            flag = True
    return password

result = input
while not isOk(result):
    result = nextInSequence(result)

with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

