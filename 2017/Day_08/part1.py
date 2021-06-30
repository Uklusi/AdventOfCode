"""
zpw dec 249 if on != -3941
g dec -541 if u == -2614
"""
from collections import defaultdict, namedtuple

Instruction = namedtuple("Instruction", "reg instr val")
Condition = namedtuple("Condition", "reg compar val")
Line = namedtuple("Line", "instruction condition")

registers = defaultdict(lambda: 0)

result = 0

commands = []

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        (instrStr, condStr) = line.split(" if ")
        instrL = instrStr.split()
        condL = condStr.split()
        commands.append(
            Line(
                instruction = Instruction(
                    reg = instrL[0],
                    instr = instrL[1],
                    val = int(instrL[2])
                ),
                condition = Condition(
                    reg = condL[0],
                    compar = condL[1],
                    val = int(condL[2])
                )
            )
        )

def evalCond(condition):
    a = registers[condition.reg]
    b = condition.val
    comp = condition.compar
    return eval(f"{a} {comp} {b}")

for command in commands:
    if evalCond(command.condition):
        instr = command.instruction
        reg = instr.reg
        val = instr.val
        if instr.instr == "dec":
            val = -val
        registers[reg] += val

result = max(registers.values())

with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

