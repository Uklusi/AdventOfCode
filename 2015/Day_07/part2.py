MAXINT = 65536

result = 0

wireSignal = {"0": 0}

wireCalc = {}

funcDict = {
    "ASSIGN": lambda i, j: i,
    "AND": lambda i, j: i & j,
    "OR": lambda i, j: i | j,
    "NOT": lambda i, j: MAXINT - i - 1,
    "LSHIFT": lambda i, j: (i << j) % MAXINT,
    "RSHIFT": lambda i, j: i >> j
}

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        (calc, target) = line.split(" -> ")
        if calc.isnumeric():
            wireSignal[target] = int(calc)
        else:
            calcArray = calc.split()
            if len(calcArray) == 1:
                wireCalc[target] = {
                    "func": funcDict["ASSIGN"],
                    "val1": calcArray[0],
                    "val2": "0"
                }
            elif calcArray[0] == "NOT":
                if calcArray[1].isnumeric():
                    wireSignal[calcArray[1]] = int(calcArray[1]) 
                wireCalc[target] = {
                    "func": funcDict["NOT"],
                    "val1": calcArray[1],
                    "val2": "0"
                }
            else:
                if calcArray[0].isnumeric():
                    wireSignal[calcArray[0]] = int(calcArray[0]) 
                if calcArray[2].isnumeric():
                    wireSignal[calcArray[2]] = int(calcArray[2])
                
                wireCalc[target] = {
                    "func": funcDict[calcArray[1]],
                    "val1": calcArray[0],
                    "val2": calcArray[2],
                }
        
def recursiveCalcWire(wire):
    if wire in wireSignal:
        return wireSignal[wire]
    val1 = recursiveCalcWire( wireCalc[wire]["val1"] )
    val2 = recursiveCalcWire( wireCalc[wire]["val2"] )
    wireSignal[wire] = wireCalc[wire]["func"](val1, val2)
    return wireSignal[wire]

wireSignal["b"] = 46065

result = recursiveCalcWire("a")

with open("output2.txt", "w") as output:
    output.write( str(result) )
    print(str(result))