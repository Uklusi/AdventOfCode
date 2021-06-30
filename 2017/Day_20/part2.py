"""
p=<60,851,-2490>, v=<8,120,-353>, a=<0,-14,24>
"""
from AOCClasses import Position3D
from copy import deepcopy
result = 0

def sign(n):
    return 1 if n > 0 else 0 if n == 0 else -1

particles = []
with open("input.txt", "r") as input:
    for line in input:
        line = line.strip().split(", ")
        a = Position3D(*[int(n) for n in line[2][3:-1].split(",")])
        v = Position3D(*[int(n) for n in line[1][3:-1].split(",")])
        p = Position3D(*[int(n) for n in line[0][3:-1].split(",")])
        particles.append([a,v,p])

collisions = {}
for count in range(100):
    newParticles = set()
    toDelete = set()
    for (a,v,p) in particles:
        v = v + a
        p = p + v
        if p in collisions:
            newParticles.discard(collisions[p])
        else:
            collisions[p] = (a,v,p)
            newParticles.add((a,v,p))
    particles = newParticles

result = len(particles)

    

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

