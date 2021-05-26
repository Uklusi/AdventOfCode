result = 0
instructions = []
registers = {
    "a": 1,
    "b": 0
}

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip().replace(",","").split()
        opcode = line[0]
        if opcode == "jmp":
            offset = int(line[1])
            reg = None
        else:
            offset = None
            reg = line[1]
        if opcode in ["jie", "jio"]:
            offset = int(line[2])
        instructions.append({
            "opcode": opcode,
            "reg": reg,
            "offset": offset
        })

i = 0

while 0 <= i < len(instructions):
    instruction = instructions[i]
    opcode = instruction["opcode"]
    reg = instruction["reg"]
    offset = instruction["offset"]
    if opcode == "jmp":
        i += offset
    elif opcode == "jie":
        if registers[reg] % 2 == 0:
            i += offset
        else: 
            i += 1
    elif opcode == "jio":
        if registers[reg] == 1:
            i += offset
        else: 
            i += 1
    else:
        i += 1
        if opcode == "inc":
            registers[reg] += 1
        elif opcode == "hlf":
            registers[reg] //= 2
        elif opcode == "tpl":
            registers[reg] *= 3
        else:
            print("ERROR")

result = registers["b"]

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

