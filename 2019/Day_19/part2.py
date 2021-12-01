from AoCUtils import *
from intcode import *


result = 0
partNumber = "2"

writeToLog = False
if writeToLog:
    logFile = open("log" + partNumber + ".txt", "w")
else:
    logFile = "stdout"
printLog = printLogFactory(logFile)

tape = readTape()

stop = False
y = 5

class MinMax(NamedTuple):
    min: int
    max: int

# From previous solution
minMax = [MinMax(6, 7)]
while not stop:
    y += 1
    xMin = minMax[-1].min - 1
    xMax = minMax[-1].max - 1

    minFound = False
    while not minFound:
        xMin += 1
        interpreter = Interpreter(tape, inputs=[xMin, y])
        minFound = interpreter.runOutput()[0] == 1
    
    maxFound = False
    while not maxFound:
        xMax += 1
        interpreter = Interpreter(tape, inputs=[xMax, y])
        maxFound = interpreter.runOutput()[0] == 0
    
    minMax.append(MinMax(xMin, xMax))

    if len(minMax) >= 100:
        if minMax[-100].max - minMax[-1].min >= 100:
            stop = True
            result = minMax[-1].min * 10000 + y - 99




with open("output" + partNumber + ".txt", "w") as outputFile:
    outputFile.write(str(result))
    print(str(result))

if writeToLog:
    cast(TextIOWrapper, logFile).close()

