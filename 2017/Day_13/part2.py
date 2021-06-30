result = 0
layers = []

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        (d, r) = [int(n) for n in line.split(": ")]
        layers.append((d, r))

start = 0
stop = False

while not stop:
    start += 1
    caught = False
    for (d, r) in layers:
        if (d + start) % (2 * r - 2) == 0:
            caught = True
            break
    stop = not caught

result = start
    

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

