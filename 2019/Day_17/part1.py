from AoCUtils import *
from intcode import Interpreter, readTape

result = 0
partNumber = "1"

writeToLog = True
if writeToLog:
    logFile = open("log" + partNumber + ".txt", "w")
else:
    logFile = "stdout"
printLog = printLogFactory(logFile)


intCodeTape = readTape("input.txt")

interpreter = Interpreter(intCodeTape=intCodeTape)

outputChars = []

def outputCallback(output: int) -> None:
    global outputChars
    outputChars.append(chr(output))

interpreter.setOutputCallback(outputCallback)

interpreter.run()

frame = join(outputChars).split()

printLog(prettify(frame))

def isSolid(p: Position) -> bool:
    if frame[p.y][p.x] in [".", "X"]:
        return True
    return False

for i in range(len(frame)):
    for j in range(len(frame[0])):
        p = MapPosition(j, i, frame=frame, occupied=isSolid)
        if p.isEmpty() and len(p.adjacent()) == 4:
            result += i * j

with open("output" + partNumber + ".txt", "w") as outputFile:
    outputFile.write(str(result))
    print(str(result))

if writeToLog:
    cast(TextIOWrapper, logFile).close()

