result = 0
input = 335

l = [0]
i = 0

for n in range(1, 2018):
    i = ((i + input) % len(l)) + 1
    l = l[:i] + [n] +l[i:]

result = l[i+1]

with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

