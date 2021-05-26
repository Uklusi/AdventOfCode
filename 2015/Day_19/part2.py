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
            subs[b] = a

minsteps = 999
endre = r'(?=[A-Z]|e|#|$)'
patterns = {}
for molec in subs:
    patterns[molec] = re.compile(molec + endre)

def reduction(target, steps):
    global minsteps
    numatoms = len(re.findall(r"[A-Z][a-z]*", target))
    if steps + numatoms - 1 > minsteps: # simple heuristic: each sub swaps 2+ atoms for one
        return
    if target == "e":
        minsteps = min(minsteps, steps)
    else:
        for molec in subs:
            pattern = patterns[molec]
            while pattern.search(target):
                reduction(pattern.sub(subs[molec], target, count=1), steps + 1)
                target = pattern.sub('#', target, count=1)
            target = re.sub('#', molec, target)

reduction(target, 0)

result = minsteps


with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

