from AoCUtils import *
from intcode import *
from functools import partial
import time
from collections import deque


result = 0
partNumber = "2"

writeToLog = True
if writeToLog:
    logFile = open("log" + partNumber + ".txt", "w")
else:
    logFile = "stdout"
printLog = printLogFactory(logFile)

intcodeTape = readTape()
outputQueue = defaultdict(lambda: [])
inputQueue = defaultdict(lambda: [])
idleSignal = [deque([False]*10, 10) for _ in range(50)]

def outputCallback(i, output):
    # printLog(f"output from {i}: {output}")
    global outputQueue
    global inputQueue
    global idleSignal
    idleSignal[i].append(False)
    outputQueue[i].append(output)
    if len(outputQueue[i]) == 3:
        target = outputQueue[i][0]
        inputQueue[target].extend(outputQueue[i][1:])
        printLog(f"package from {i} to {target}: {outputQueue[i][1]}, {outputQueue[i][2]}")
        outputQueue[i] = []

def inputCallback(i):
    global inputQueue
    global network
    global idleSignal
    if len(inputQueue[i]) == 0:
        # printLog(f"input to {i}: -1")
        network[i].addInput(-1)
        idleSignal[i].append(True)
    else:
        k = inputQueue[i].pop(0)
        idleSignal[i].append(False)
        # printLog(f"input to {i}: {k}")
        network[i].addInput(k)

network = []
for i in range(50):
    network.append(Interpreter(intcodeTape))
    inputQueue[i] = [i]
    network[i].setInputCallback(partial(inputCallback, i))
    network[i].setOutputCallback(partial(outputCallback, i))

prevNat = [0, 0]
nat = [None, None]
while True:
    for i in range(50):
        network[i].step()
        while len(inputQueue[i]) > 0 or len(outputQueue[i]) > 0:
            network[i].step()

    if len(inputQueue[255]) > 0:
        nat = inputQueue[255]
        inputQueue[255] = []
    if all( [ all(idleSignal[i]) for i in range(50) ]) and all([len(inputQueue[i]) == 0 for i in range(50)]):
        inputQueue[0].extend(nat)
        printLog(f"NAT package from 255 to 0: {nat[0]}, {nat[1]}")
        if nat[1] == prevNat[1]:
            break
        prevNat = nat
        nat = [None, None]


result = prevNat[1]






with open("output" + partNumber + ".txt", "w") as outputFile:
    outputFile.write(str(result))
    print(str(result))

if writeToLog:
    cast(TextIOWrapper, logFile).close()

