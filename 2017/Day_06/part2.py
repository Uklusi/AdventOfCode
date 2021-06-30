result = 0
count = 0

mems = []
with open("input.txt", "r") as input:
    for line in input:
        line = line.strip().split()
        mems = [int(n) for n in line]

def h(mems):
    return ",".join([str(n) for n in mems])

seen = {}
l = len(mems)

while h(mems) not in seen:
    seen[h(mems)] = count
    m = max(mems)
    k = mems.index(m)
    mems[k] = 0
    for i in range(m):
        mems[(k+i + 1) % l] += 1
    count += 1

result = count - seen[h(mems)]

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

