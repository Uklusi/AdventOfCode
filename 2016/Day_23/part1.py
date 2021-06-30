result = 0

registers = {
    "a": 7,
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
                "val1": instructionString[1],
                "val2": instructionString[2]
            }
        elif opcode in ["inc", "dec"]:
            instruction = {
                "opcode": opcode,
                "reg": instructionString[1]
            }
        elif opcode == "jnz":
            instruction = {
                "opcode": "jnz",
                "val1": instructionString[1],
                "val2": instructionString[2]
            }
        elif opcode == "tgl":
            instruction = {
                "opcode": "tgl",
                "reg": instructionString[1]
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
        reg = instruction["reg"]
        if not isNumeric(reg):
            registers[reg] += 1
        i += 1
    elif instruction["opcode"] == "dec":
        reg = instruction["reg"]
        if not isNumeric(reg):
            registers[reg] += -1
        i += 1
    elif instruction["opcode"] == "cpy":
        reg = instruction["val2"]
        if not isNumeric(reg):
            registers[reg] = checkValue(instruction["val1"])
        i += 1
    elif instruction["opcode"] == "jnz":
        if checkValue(instruction["val1"]) != 0:
            i += checkValue(instruction["val2"])
        else:
            i += 1
    elif instruction["opcode"] == "tgl":
        newPos = i + checkValue(instruction["reg"])
        if 0 <= newPos < len(instructions):
            if instructions[newPos]["opcode"] in ["dec", "tgl"]:
                instructions[newPos]["opcode"] = "inc"
            elif instructions[newPos]["opcode"] in ["inc"]:
                instructions[newPos]["opcode"] = "dec"
            elif instructions[newPos]["opcode"] in ["jnz"]:
                instructions[newPos]["opcode"] = "cpy"
            else:
                instructions[newPos]["opcode"] = "jnz"
        i += 1

result = registers["a"]

with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

