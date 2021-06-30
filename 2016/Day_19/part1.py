result = 0
input = 3014387

l = input
m = 0
p = 0
while l > 1:
    p += 1
    if l % 2:
        m = m + pow(2, p, input)
    l //= 2

result = m + 1

with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

