from AoCUtils import *


result = 0
partNumber = "2"

writeToLog = False
if writeToLog:
    logFile = open("log" + partNumber + ".txt", "w")
else:
    logFile = "stdout"
printLog = printLogFactory(logFile)

instructions = []

with open("input.txt", "r") as inputFile:
    lines = inputFile.read().strip().split("\n")
    for line in lines:
        line = line.strip().split()
        instructions.append(line)

N = 119315717514047
repetitions = 101741582076661
k = 2020

coeff = 1
shift = 0
for instruction in instructions:
    match instruction:
        case ["deal", "into", *_]:
            coeff = -coeff
            shift = -shift - 1
        case ["deal", "with", "increment", num]:
            num = int(num)
            coeff = coeff * num
            shift = shift * num
        case ["cut", num]:
            num = int(num)
            shift = shift - num

# repetition 1
# k -> c * k + s
# repetition 2
# k -> c * (c * k + s) + s = c ** 2 * k + c * s + s
# repetition r
# k -> c**r * k + s * (c**(r-1) + c**(r-2) + ... + c + 1)
# k -> c**r * k + s * (c**r - 1) / (c - 1)
# k <- (k + s * (c**r - 1) / (c - 1) ) / c**r

t = pow(coeff, repetitions, N)
result = (k - shift * (t - 1) * pow(coeff - 1, -1, N) ) * pow(t, -1, N) % N





with open("output" + partNumber + ".txt", "w") as outputFile:
    outputFile.write(str(result))
    print(str(result))

if writeToLog:
    cast(TextIOWrapper, logFile).close()

