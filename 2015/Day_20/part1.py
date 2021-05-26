from math import sqrt
result = 0
"""problem is:find min(n) s.t. sigma_1(n) > input
we know sigma_1(n) < n(n-1)/2 ~ n^2/ 2"""

input = 3400000 #renormalized

divSum = [1 + i for i in range(input)]
divSum[0] = 0
divSum[1] = 1
for i in range(2, input//2+1):
    for j in range(i+i, input, i):
        divSum[j] += i

for n in range(input):
    if divSum[n] >= input:
        result = n
        break

with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

