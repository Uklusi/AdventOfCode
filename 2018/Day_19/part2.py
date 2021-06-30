result = 0

instructions = []
with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        if line[0] == "#":
            pointerReg = int(line.split()[1])
        else:
            instructions.append([line.split()[0]] + [int(n) for n in line.split()[1:]])

def runInstruction(reg, params):
    reg = reg.copy()
    (code, a, b, c) = params

    operation = code[0:2] if code[0:2] in ["gt", "eq"] else code[0:3]
    modeA = code[-2] if operation in ["gt", "eq"] else code[-1] if operation == "set" else "r"
    modeB = code[-1]

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

i = 0
registers = [1,0,0,0,0,0]

# program is intractable, disassembled code
n = 10551418
result = sum([i for i in range(1, n+1) if n % i == 0])

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

