result = 0
input = 3014387

shift = input // 2 + (1 if input % 2 else 2)
l = input - (1 if input % 2 else 2)
m = 0
p = 0
while l > 1:
    p += 1
    if l == 2:
        l = 1
        break
    elif l == 4:
        m = m + pow(3, p, input)
        l = 1
        break
    elif l % 6 == 2:
        m = m + pow(3, p, input)
    elif l % 6 == 4:
        m = m + 2 * pow(3, p, input)
    l = (l // 6) * 2

result = (m + shift) % input + 1

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

