"""
p=<60,851,-2490>, v=<8,120,-353>, a=<0,-14,24>
"""
result = 0

def sign(n):
    return 1 if n > 0 else 0 if n == 0 else -1

particles = []
with open("input.txt", "r") as input:
    for line in input:
        line = line.strip().split(", ")
        a = [int(n) for n in line[2][3:-1].split(",")]
        v = [int(n) for n in line[1][3:-1].split(",")]
        p = [int(n) for n in line[0][3:-1].split(",")]
        particles.append([a,v,p])

def signpart(part):
    s = []
    for i in range(3):
        if part[0][i] != 0:
            s.append(sign(part[0][i]))
        elif part[1][i] != 0:
            s.append(sign(part[1][i]))
        else:
            s.append(sign(part[2][i]))
    return s

minvals = [99999,0,0]
minpart = []
for (i, particle) in enumerate(particles):
    a = particle[0]
    v = particle[1]
    p = particle[2]
    s = signpart(particle)
    ascal = sum([a[i]*s[i] for i in range(3)])
    vscal = sum([v[i]*s[i] for i in range(3)])
    pscal = sum([p[i]*s[i] for i in range(3)])
    vals = [ascal, vscal, pscal]
    if vals < minvals:
        minpart = [i]
        minvals = vals
    elif vals == minvals:
        minpart.append(i)
        print(i, particle)
        print(ascal, vscal, pscal)


        

print(len(minpart))
if len(minpart) == 1:
    result = minpart[0]


with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

