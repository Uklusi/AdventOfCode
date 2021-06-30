from collections import defaultdict

result = 0

requires = defaultdict(lambda: [])
blocks = defaultdict(lambda: [])
steps = set()

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip().split()
        a = line[1]
        b = line[7]
        steps.add(a)
        steps.add(b)
        requires[b].append(a)
        blocks[a].append(b)

queue = [l for l in steps if len(requires[l]) == 0]
queue.sort()
result = ""

while len(queue) > 0:
    step = queue[0]
    queue = queue[1:]
    result += step
    new = [l for l in blocks[step] if set(result).issuperset(set(requires[l]))]
    queue = queue + new
    queue.sort()

with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

