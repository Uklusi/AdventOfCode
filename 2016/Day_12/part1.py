result = 0

registers = {
    "a": 0,
    "b": 0,
    "c": 0,
    "d": 0
}

instructions = []

def isNumeric(string):
    s = string.replace("-", "")
    return s.isnumeric()

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        instructionString = line.split()
        opcode = instructionString[0]
        if opcode == "cpy":
            instruction = {
                "opcode": "cpy",
                "val": instructionString[1],
                "reg": instructionString[2]
            }
        elif opcode in ["inc", "dec"]:
            instruction = {
                "opcode": opcode,
                "reg": instructionString[1]
            }
        elif opcode == "jnz":
            instruction = {
                "opcode": "jnz",
                "check": instructionString[1],
                "val": instructionString[2]
            }
        else:
            raise("Opcode error")
        instructions.append(instruction)

def checkValue(regOrValue):
    if isNumeric(regOrValue):
        return int(regOrValue)
    else:
        return registers[regOrValue]

i = 0
while i < len(instructions):
    instruction = instructions[i]
    if instruction["opcode"] == "inc":
        registers[instruction["reg"]] += 1
        i += 1
    elif instruction["opcode"] == "dec":
        registers[instruction["reg"]] += -1
        i += 1
    elif instruction["opcode"] == "cpy":
        registers[instruction["reg"]] = checkValue(instruction["val"])
        i += 1
    elif instruction["opcode"] == "jnz":
        if checkValue(instruction["check"]) != 0:
            i += checkValue(instruction["val"])
        else:
            i += 1

result = registers["a"]

with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

