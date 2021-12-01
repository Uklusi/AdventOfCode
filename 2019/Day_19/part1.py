from AoCUtils import *
from intcode import *


result = 0
partNumber = "1"

writeToLog = False
if writeToLog:
    logFile = open("log" + partNumber + ".txt", "w")
else:
    logFile = "stdout"
printLog = printLogFactory(logFile)

tape = readTape()

image = [[empty for i in range(50)] for j in range(50)]

for (x, y) in product(range(50), range(50)):
    interpreter = Interpreter(tape, inputs=[x, y])
    result += interpreter.runOutput()[0]
    image[y][x] = solid if interpreter.runOutput()[0] else empty

print(Image(image))

with open("output" + partNumber + ".txt", "w") as outputFile:
    outputFile.write(str(result))
    print(str(result))

if writeToLog:
    cast(TextIOWrapper, logFile).close()

