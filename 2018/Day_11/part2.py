from AOCClasses import *
from functools import cache
from itertools import product

result = 0

gridSerialNumber = 4151
# gridSerialNumber = 18

@cache
def power(x, y):
        rID = x + 10
        power = rID * y + gridSerialNumber
        power *= rID
        power = (power // 100) % 10 - 5
        return power

@cache
def squarespower(x, y, k):
    if k == 1:
        return power(x, y)
    else:
        n = k // 2
        ret = 0
        ret += squarespower(x, y, n)
        ret += squarespower(x+n, y, n)
        ret += squarespower(x, y+n, n)
        ret += squarespower(x+n, y+n, k - n)
        if k % 2 == 1:
            ret += sum([power(x+k-1, y+i) + power(x+i, y+k -1) for i in range(n)])
    return ret

p = (-1, -1)
for (x, y) in product(range(1, 301), repeat=2):
    for k in range(1, min(301-x, 301-y)):
        ret = squarespower(x,y,k)
        if ret > result:
            result = ret
            p = (x, y, k)

    
print(result)
result = ",".join(map(str, p))
with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

