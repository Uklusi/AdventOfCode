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

def time(step):
    return ord(step) - ord("A") + 61

def workerNum(queue, avail):
    return min(len(queue), avail)

queue = [l for l in steps if len(requires[l]) == 0]
queue.sort()
# print(queue)
done = set()
w = workerNum(queue, 5)
avail = 5 - w
times = {queue[i]: time(queue[i]) for i in range(w)}
queue = queue[w:]

while len(done) < len(steps):
    # print(times)
    result += 1
    for step in times:
        times[step] += -1
        if times[step] == 0:
            done.add(step)

    stepsToCheck = list(times.keys())
    for step in stepsToCheck:
        if times[step] == 0:
            times.pop(step)
            avail += 1
            new = [l for l in blocks[step] if done.issuperset(requires[l])]
            queue = queue + new

    queue.sort()
    # print(queue)
    w = workerNum(queue, avail)
    avail = avail - w

    # print(avail, w)
    for i in range(w):
        # print(queue)
        times[queue[i]] = time(queue[i])
    queue = queue[w:]
    


with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

