result = 0
GEN = 20

translate = {}
with open("input.txt", "r") as input:
    data = input.read().split("\n\n")
    state = data[0].split(": ")[1]
    for line in data[1].split("\n"):
        (a, b) = line.split(" => ")
        translate[a] = b

for _ in range(GEN):
    state = "...." + state + "...."
    newstate = ""
    for i in range(len(state) - 4):
        newstate += translate[state[i:i+5]]
    state = newstate

for (i, c) in enumerate(state):
    n = i - 2*GEN
    if c == "#":
        result += n



with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

