result = 0

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        (l,w,h) = [int(n) for n in line.split("x")]
        result += 2*(l + w + h - max(l, w, h) ) + l*w*h

with open("output2.txt", "w") as output:
    output.write( str(result) )
    print(str(result))