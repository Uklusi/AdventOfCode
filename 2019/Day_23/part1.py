from AoCUtils import *
from intcode import *
from functools import partial
import time


result = 0
partNumber = "1"

writeToLog = True
if writeToLog:
    logFile = open("log" + partNumber + ".txt", "w")
else:
    logFile = "stdout"
printLog = printLogFactory(logFile)

intcodeTape = readTape()
outputQueue = defaultdict(lambda: [])
inputQueue = defaultdict(lambda: [])

def outputCallback(i, output):
    printLog(f"output from {i}: {output}")
    global outputQueue
    global inputQueue
    outputQueue[i].append(output)
    if len(outputQueue[i]) == 3:
        target = outputQueue[i][0]
        inputQueue[target].extend(outputQueue[i][1:])
        printLog(f"package from {i} to {target}: {outputQueue[i][1]}, {outputQueue[i][2]}")
        outputQueue[i] = []

def inputCallback(i):
    global inputQueue
    global network
    if len(inputQueue[i]) == 0:
        # printLog(f"input to {i}: -1")
        network[i].addInput(-1)
        time.sleep(0.01)
    else:
        k = inputQueue[i].pop(0)
        printLog(f"input to {i}: {k}")
        network[i].addInput(k)

network = []
for i in range(50):
    network.append(Interpreter(intcodeTape))
    inputQueue[i] = [i]
    network[i].setInputCallback(partial(inputCallback, i))
    network[i].setOutputCallback(partial(outputCallback, i))
    network[i].start()

while len(inputQueue[255]) == 0:
    pass

for i in range(50):
    network[i].stop()

result = inputQueue[255][1]






with open("output" + partNumber + ".txt", "w") as outputFile:
    outputFile.write(str(result))
    print(str(result))

if writeToLog:
    cast(TextIOWrapper, logFile).close()

