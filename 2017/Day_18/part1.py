result = 0

a = 2 ** 31 - 1

p = 826
for i in range(127):
    p = (p * 8505) % a
    p = (p * 129749 + 12345) % a
    b = p % 10000

result = b

with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

