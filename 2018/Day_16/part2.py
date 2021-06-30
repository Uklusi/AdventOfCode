from collections import defaultdict

result = 0

with open("input.txt", "r") as input:
    data = input.read().split("\n\n\n")
    opcodeTestsTxt = data[0].split("\n\n")
    program = [[int(n) for n in line.split()] for line in data[1].strip().split("\n")]

opcodeTests = []
for testTxt in opcodeTestsTxt:
    testTxt = testTxt.split("\n")
    before = eval(testTxt[0][8:])
    after = eval(testTxt[2][8:])
    code = [int(n) for n in testTxt[1].split()]
    opcodeTests.append((before, after, code))

opcodeList = [
    "addr", # c = a + b
    "addi", # c = a + vb
    "mulr", 
    "muli",
    "banr", # bitwise and
    "bani",
    "borr", # bitwise or
    "bori",
    "setr",
    "seti",
    "gtir",
    "gtri",
    "gtrr",
    "eqir",
    "eqri",
    "eqrr"
]

def opcodes(reg, code, params):
    reg = reg.copy()
    operation = code[0:2] if code[0:2] in ["gt", "eq"] else code[0:3]
    modeA = code[-2] if operation in ["gt", "eq"] else code[-1] if operation == "set" else "r"
    modeB = code[-1]

    (_, a, b, c) = params
    valA = reg[a] if modeA == "r" else a
    valB = reg[b] if modeB == "r" else b

    if operation == "add":
        reg[c] = valA + valB
    elif operation == "mul":
        reg[c] = valA * valB
    elif operation == "ban":
        reg[c] = valA & valB
    elif operation == "bor":
        reg[c] = valA | valB
    elif operation == "set":
        reg[c] = valA
    elif operation == "gt":
        reg[c] = int(valA > valB)
    elif operation == "eq":
        reg[c] = int(valA == valB)
    else:
        raise(Exception("No Idea"))
    return reg

opcodeNums = defaultdict(lambda: set(opcodeList))

for test in opcodeTests:
    (before, after, instr) = test
    possibles = set()
    for op in opcodeList:
        ret = opcodes(before, op, instr)
        if ret == after:
            possibles.add(op)
    opcodeNums[instr[0]].intersection_update(possibles)

opcodeNums = [(k, v) for (k,v) in opcodeNums.items()]
keyFun = lambda t: len(t[1])
opcodeNums.sort(key=keyFun)
while len(opcodeNums[-1][1]) > 1:
    for (i, (k, v)) in enumerate(opcodeNums):
        if len(v) == 1:
            for j in range(i+1, len(opcodeNums)):
                opcodeNums[j][1].difference_update(v)
    opcodeNums.sort(key=keyFun)
opcodeNums = {
    k: v.pop() for (k,v) in opcodeNums
}

def runInstruction(instr, regs):
    num = instr[0]
    opcode = opcodeNums[num]
    regs = opcodes(regs, opcode, instr)
    return regs

regs = [0,0,0,0]

for instr in program:
    regs = runInstruction(instr, regs)

result = regs[0]

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

