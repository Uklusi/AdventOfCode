from collections import defaultdict
result = 0
GEN = 50000000000

translate = defaultdict(lambda:".")
with open("input.txt", "r") as input:
    data = input.read().split("\n\n")
    state = data[0].split(": ")[1]
    # state = "#..#.#..##......###...###"
    # data[1] = \
    """...## => #
..#.. => #
.#... => #
.#.#. => #
.#.## => #
.##.. => #
.#### => #
#.#.# => #
#.### => #
##.#. => #
##.## => #
###.. => #
###.# => #
####. => #"""
    for line in data[1].split("\n"):
        (a, b) = line.split(" => ")
        translate[a] = b

totadd = 0
oldstate= "@"
for _ in range(GEN):
    add = 0
    newstate = []
    flag = True
    if state[0:2] == "##":
        newstate.append("#")
        add += 1
        flag = False

    c = translate[".." + state[:3]]
    if flag and c == ".":
        add += -1
    else:
        newstate.append(c)
        flag = False

    c = translate["." + state[:4]]
    if flag and c == ".":
        add += -1
    else:
        newstate.append(c)
        flag = False

    for i in range(0, len(state) - 4):
        newstate.append(translate[state[i:i+5]])

    newstate.append(translate[state[-4:] + "." ])

    c = translate[state[-3:] + ".."]
    if c == "#" or state[-2:] == ".#":
        newstate.append(c)

    if state[-2:] == ".#":
        newstate.append("#")
        
    oldstate = state
    state = "".join(newstate)
    totadd += add
    if oldstate == state:
        totadd += add * (GEN - _ - 1)
        break

for (i, c) in enumerate(state):
    n = i - totadd
    if c == "#":
        result += n

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))
