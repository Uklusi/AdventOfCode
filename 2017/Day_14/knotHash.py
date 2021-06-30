from functools import reduce
from operator import xor

def reverse(l):
    return list(reversed(l))

def knotHash(input):
    lengths = list(map(ord, input)) + [17, 31, 73, 47, 23]

    SIZE = 256
    numberList = list(range(SIZE))
    curr = 0
    skipsize = 0

    for _ in range(64):
        for length in lengths:
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
            curr = (curr + length + skipsize) % SIZE
            skipsize += 1
    xorsize = 16
    xorNums = []
    for n in range(0, SIZE, xorsize):
        xorNums.append(reduce(xor, numberList[n:n + xorsize]))

    result = ""
    for n in xorNums:
        result += f"{n:02x}"
    
    return result
