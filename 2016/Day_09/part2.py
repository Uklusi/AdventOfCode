result = 0

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        data = line

def recursiveCalc(data, rep):
    # assuming data is exactly quant characters long
    # print(data[:30])
    if "(" not in data:
        return len(data) * rep
    if len(data) == 0:
        return 0
    tot = 0
    start = data.find("(")
    end = data.find(")")
    tot += start * rep
    [quant2, rep2] = [int(n) for n in data[start+1:end].split("x")]
    tot += rep * recursiveCalc(data[end+1:end+1+quant2], rep2)
    tot += recursiveCalc(data[end+1+quant2:], rep)
    return tot

result = recursiveCalc(data, 1)

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

