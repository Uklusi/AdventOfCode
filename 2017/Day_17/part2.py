result = 0
input = 335

i = 0
l = 1

for n in range(1, 50 * 10**6 + 1):
    i = ((i + input) % l) + 1
    if i == 1:
        result = n
    l += 1


with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

