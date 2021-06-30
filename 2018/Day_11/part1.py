from AOCClasses import *
from functools import cache
from itertools import product

result = 0

gridSerialNumber = 4151


@cache
def power(x, y):
        rID = x + 10
        power = rID * y + gridSerialNumber
        power *= rID
        power = (power // 100) % 10 - 5
        return power

p = (-1, -1)
for (x, y) in product(range(298), repeat=2):
    ret = 0
    for (i, j) in product(range(3), repeat=2):
        ret += power(x+i, y+j)
    if ret > result:
        result = ret
        p = (x, y)

result = p
with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

