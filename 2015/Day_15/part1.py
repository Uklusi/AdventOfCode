from math import prod
result = 0

ingrList = []
properties = ["cap", "dur", "fla", "tex"] #, "cal"]

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip().replace(",","").replace(":", "")
        [name, _, cap, _, dur, _, fla, _, tex, _, cal] = line.split()
        ingrList.append({
            "name": name, 
            "cap": int(cap), 
            "dur": int(dur),
            "fla": int(fla),
            "tex": int(tex),
            "cal": int(cal)
        })

def score(quants):
    total = {p: sum([ingrList[i][p]*val for (i, val) in enumerate(quants)]) for p in properties}
    total = {p: total[p] if total[p] >= 0 else 0 for p in properties}
    return prod([total[p] for p in properties])
    

for i in range(101):
    for j in range(101-i):
        for k in range(101-i-j):
            result = max(result, score([i,j,k,100-i-j-k]))



with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

