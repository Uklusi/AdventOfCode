result = 0

instructions = []
with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        if line[0] == "#":
            pointerReg = int(line.split()[1])
        else:
            instructions.append([line.split()[0]] + [int(n) for n in line.split()[1:]])

def debug(reg, instruction):
    (code, a, b, c) = instruction

    operation = code[0:2] if code[0:2] in ["gt", "eq"] else code[0:3]
    modeA = code[-2] if operation in ["gt", "eq"] else code[-1] if operation == "set" else "r"
    modeB = code[-1]

    nameA = f"reg[{a}]" if modeA == "r" else f"{a}"
    nameB = f"reg[{b}]" if modeB == "r" else f"{b}"
    nameA = " point" if nameA == f"reg[{pointerReg}]" else nameA
    nameB = " point" if nameB == f"reg[{pointerReg}]" else nameB
    nameC = " point" if c == pointerReg else f"reg[{c}]"

    valA = reg[a] if modeA == "r" else a
    valB = reg[b] if modeB == "r" else b

    if operation == "set":
        rhs = f"{nameA:>12}"
        rhsVal = f"{valA:>12}"
    else:
        if operation == "add":
            operator = "+"
        elif operation == "mul":
            operator = "*"
        elif operation == "ban":
            operator = "&"
        elif operation == "bor":
            operator = "|"
        elif operation == "gt":
            operator = ">"
        elif operation == "eq":
            operator = "=="
        rhs = f"{nameA:>12s} {operator:>2s} {nameB:>12s}"
        rhsVal = f"{valA:>12d} {operator:>2s} {valB:>12d}"
        
    return f"{nameC} = {rhs:<32s}" + " ---     " + f"{nameC} = {rhsVal:<30s}" + f" = {eval(rhsVal):>12d}"

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
registers = [5970144,0,0,0,0,0]

while 0 <= i < len(instructions):
    registers[pointerReg] = i
    print(f"{i+2:>2d}", debug(registers, instructions[i]))
    registers = runInstruction(registers, instructions[i])
    i = registers[pointerReg]
    i += 1

result = registers[0]

with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

