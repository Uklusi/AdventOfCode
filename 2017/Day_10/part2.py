from functools import reduce
from operator import xor

result = 0

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        lengths = list(map(ord, line)) + [17, 31, 73, 47, 23]

SIZE = 256
numberList = list(range(SIZE))
curr = 0
skipsize = 0

def reverse(l):
    return list(reversed(l))

for _ in range(64):
    for length in lengths:
        # if (length, curr) == (73, 240):
        #     breakpoint()
        if length + curr > SIZE:
            untilEnd = SIZE - curr
            fromBegin = length - untilEnd
            torev = numberList[curr:] + numberList[:fromBegin]
            torev = reverse(torev)
            numberList = torev[-fromBegin:] + numberList[fromBegin:curr] + torev[:untilEnd]
        else:
            torev = numberList[curr:curr+length]
            torev = reverse(torev)
            numberList = numberList[:curr] + torev + numberList[curr+length:]
        # print(len(torev))
        curr = (curr + length + skipsize) % SIZE
        skipsize += 1
xorsize = 16
xorNums = []
for n in range(0, SIZE, xorsize):
    xorNums.append(reduce(xor, numberList[n:n + xorsize]))

result = ""
for n in xorNums:
    result += f"{n:02x}"


with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

