from AOCClasses import Position

result = 0

input = 368078
start = Position(0,0)
filled = {start:1}
current = Position(0,0)
i = 1
flag = False

while not flag:
    current += Position(1,-1)
    for (a, b) in [(0,1), (-1, 0), (0, -1), (1,0)]:
        for j in range(2*i):
            current += Position(a,b)
            filled[current] = sum([filled[p] for p in current.adjacent() if p in filled])
            if filled[current] > input:
                result = filled[current]
                flag = True
                break
        if flag:
            break
    i += 1


with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

