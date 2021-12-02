from AoCUtils import *
from intcode import *
from functools import partial


result = 0
partNumber = "2"

writeToLog = False
if writeToLog:
    logFile = open("log" + partNumber + ".txt", "w")
else:
    logFile = "stdout"
printLog = printLogFactory(logFile)


tape = readTape()

interpreter = Interpreter(tape)

def inputCallback(interpreter):
    s = input() 
    for c in prepareAsciiInput(s):
        interpreter.AddInput(c)

def outputCallback(output):
    print(asciiToChar(output), end="")

interpreter.setInputCallback(partial(inputCallback, interpreter))
interpreter.setOutputCallback(outputCallback)

interpreter.start()
interpreter.join()


with open("output" + partNumber + ".txt", "w") as outputFile:
    outputFile.write(str(result))
    print(str(result))

if writeToLog:
    cast(TextIOWrapper, logFile).close()

