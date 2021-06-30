result = 0

from collections import defaultdict

instructions = []

regs = defaultdict(lambda: 0)

def readval(regOrVal):
    if regOrVal.isalpha():
        return regs[regOrVal]
    else:
        return int(regOrVal)

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip().split()
        instructions.append(line)

index = 0
while 0 <= index < len(instructions):
    instr = instructions[index]
    name = instr[0]
    # print(f"{name} {i} - l{index+1}")
    rv1 = instr[1]
    rv2 = instr[2] if len(instr) == 3 else "0"
    v1 = readval(rv1)
    v2 = readval(rv2)

    if name == "set":
        regs[rv1] = v2
    elif name == "sub":
        regs[rv1] -= v2
    elif name == "mul":
        regs[rv1] *= v2
        result += 1
    elif name == "jnz":
        if v1 != 0:
            # -1 to compensate the index += 1 as default
            index += v2 - 1
    else:
        breakpoint()
        raise(Exception(f"Forgot instruction: {name}"))
    index += 1

with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

