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



intCodeTape = readTape("input.txt")

inputInstructions = [
    "NOT A J",
    "NOT B T",
    "OR T J",
    "NOT C T",
    "OR T J",
    "AND D J",
    "NOT E T",
    "NOT T T",
    "OR H T",
    "AND T J",
    "RUN"
]
inputs = prepareAsciiInput(inputInstructions)
interpreter = Interpreter(intCodeTape=intCodeTape, inputs=inputs)

outputChars = []

def outputCallback(output: int) -> None:
    global outputChars
    if output < 256:
        outputChars.append(chr(output))

interpreter.setOutputCallback(outputCallback)

interpreter.run()

frame = join(outputChars).split()

print(prettify(frame))

result = interpreter.outputs[-1]



with open("output" + partNumber + ".txt", "w") as outputFile:
    outputFile.write(str(result))
    print(str(result))

if writeToLog:
    cast(TextIOWrapper, logFile).close()

