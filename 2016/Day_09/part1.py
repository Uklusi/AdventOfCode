result = 0

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        data = line

i = 0
while i < len(data):
    if data[i] != "(":
        result += 1
        i += 1
    else:
        start = i+1
        end = data[start:].find(")") + start
        [quant, rep] = [int(n) for n in data[start:end].split("x")]
        result += quant * rep
        i = end + 1 + quant

with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

