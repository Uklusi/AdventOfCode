from collections import defaultdict
result = 0

instructions = {}
steps = 12919244
state = "A"
turing = defaultdict(lambda: 0)
position = 0

with open("input.txt", "r") as input:
    data = input.read().strip().split("\n\n")
    preamble = data[0] # (initial state A, steps=12919244)
    data = data[1:]
    for block in data:
        lines = block.split("\n")
        bState = lines[0][-2]
        instructions[bState] = {
            0: {
                "write": int(lines[2][-2]),
                "move": 1 if lines[3].split()[-1] == "right." else -1,
                "state": lines[4][-2]
            },
            1: {
                "write": int(lines[6][-2]),
                "move": 1 if lines[7].split()[-1] == "right." else -1,
                "state": lines[8][-2]
            }
        }

for _ in range(steps):
    cval = turing[position]
    inst = instructions[state][cval]
    nval = inst["write"]
    turing[position] = nval
    result += nval - cval
    position += inst["move"]
    state = inst["state"]


with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

