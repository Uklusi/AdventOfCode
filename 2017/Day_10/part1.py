result = 0

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip().split(",")
        lengths = list(map(int, line))

SIZE = 256
numberList = list(range(SIZE))
curr = 0
skipsize = 0

def reverse(l):
    return list(reversed(l))

for length in lengths:
    if length + curr > SIZE:
        untilEnd = SIZE - curr
        fromBegin = length - untilEnd
        torev = numberList[curr:] + numberList[:fromBegin]
        torev = reverse(torev)
        numberList = torev[-fromBegin:] + numberList[fromBegin + 1:curr] + torev[:untilEnd]
    else:
        torev = numberList[curr:curr+length]
        torev = reverse(torev)
        numberList = numberList[:curr] + torev + numberList[curr+length:]
    curr = (curr + length + skipsize) % SIZE
    skipsize += 1

result = numberList[0] * numberList[1]



with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

