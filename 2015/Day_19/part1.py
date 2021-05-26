import re

result = 0
target = ""
subs = {}

flag = False
with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        if line == "":
            flag = True
        elif flag:
            target = line
        else:
            (a,b) = line.split(" => ")
            subs[a] = subs.get(a, []) + [b]

atoms = re.findall(r"[A-Z][a-z]*", target)

possibs = set()
for (i, atom) in enumerate(atoms):
    for repl in subs.get(atom, []):
        newatoms = atoms.copy()
        newatoms[i] = repl
        possibs.add("".join(newatoms))
    
result = len(possibs)

with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

