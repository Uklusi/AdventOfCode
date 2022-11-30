from AoCUtils import *

writeToLog = False
# writeToLog = True

useExample = False
# useExample = True


result = 0
partNumber = "1"

if writeToLog:
    logFile = open("log" + partNumber + ".txt", "w")
else:
    logFile = "stdout"
printLog = printLogFactory(logFile)

timer_start(partNumber)

inputFileName = ("example.txt" if useExample else "input.txt")
with open(inputFileName, "r") as inputFile:
    lines = inputFile.read().strip().split("\n")
    for line in lines:
        line = line.strip()





timer_stop(partNumber)

with open("output" + partNumber + ".txt", "w") as outputFile:
    outputFile.write(str(result))
    print(str(result))

if writeToLog:
    cast(TextIOWrapper, logFile).close()
