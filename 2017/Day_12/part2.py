result = 0

pipes = []

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        (_, connected) = line.split(" <-> ")
        pipes.append([int(n) for n in connected.split(", ")])

l = len(pipes)

checked = {}
start = 0
while len(checked) < 2000:
    while start in checked:
        start += 1
    queue = [start]
    result += 1

    while len(queue) > 0:
        n = queue.pop()
        checked[n] = 1
        for k in pipes[n]:
            if k not in checked:
                queue.append(k)

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

