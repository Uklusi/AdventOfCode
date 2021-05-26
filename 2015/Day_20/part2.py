result = 0

input = 34000000
nhouse = input // 33

divSum = [0 for _ in range(nhouse + 1)]

for i in range(1, nhouse):
    for j in range(50):
        multiple = i*(j+1)
        if multiple > nhouse:
            continue
        divSum[multiple] += i*11

for n in range(nhouse):
    if divSum[n] >= input:
        result = n
        break

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

