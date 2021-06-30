result = 0

with open("input.txt", "r") as input:
    data = input.read().split("\n\n\n")
    opcodeTestsTxt = data[0].split("\n\n")

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

    (a, b, c) = params
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

for test in opcodeTests:
    (before, after, code) = test
    count = 0
    for op in opcodeList:
        ret = opcodes(before, op, code[1:])
        if ret == after:
            count += 1
    if count >= 3:
        result += 1





with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

