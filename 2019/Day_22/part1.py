from AoCUtils import *


result = 0
partNumber = "1"

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

N = 10007
k = 2019

for instruction in instructions:
    match instruction:
        case ["deal", "into", *_]:
            k = N - k - 1
        case ["deal", "with", "increment", num]:
            num = int(num)
            k = (k * num) % N
        case ["cut", num]:
            num = int(num)
            k = (k - num) % N

result = k




with open("output" + partNumber + ".txt", "w") as outputFile:
    outputFile.write(str(result))
    print(str(result))

if writeToLog:
    cast(TextIOWrapper, logFile).close()

