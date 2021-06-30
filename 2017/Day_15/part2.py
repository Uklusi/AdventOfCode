result = 0
genAStart = 116
genBStart = 299
# genAStart = 65
# genBStart = 8921

factorA = 16807
factorB = 48271

reminder = 2**31 - 1

A = genAStart
B = genBStart

for _ in range(5 * 10**6):
    while True:
        A = (A * factorA) % reminder
        if A % 4 == 0:
            break
    while True:
        B = (B * factorB) % reminder
        if B % 8 == 0:
            break
    if (A % 2**16) == (B % 2**16):
        result += 1

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

