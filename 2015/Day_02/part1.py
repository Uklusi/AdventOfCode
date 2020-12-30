result = 0

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        (l,w,h) = [int(n) for n in line.split("x")]
        lw = l*w
        lh = l*h
        wh = w*h
        result += 2*(lw + lh + wh) + min(lw, lh, wh)

with open("output1.txt", "w") as output:
    output.write( str(result) )
    print(str(result))