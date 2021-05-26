from itertools import permutations
result = 0

names = ["Alice", "Bob", "Carol", "David", "Eric", "Frank", "George", "Mallory", "X"] 
table = {name: {"X": 0} for name in names[:-1]}
table["X"] = {name: 0 for name in names[:-1]}


with open("input.txt", "r") as input:
    for line in input:
        line = line.strip().strip(".")
        (person, rest) = line.split(" would ")
        (txtval, neigh) = rest.split(" happiness units by sitting next to ")
        (sign, txtval) = txtval.split(" ")
        val = int(txtval)
        if sign == "lose":
            val = -val
        table[person][neigh] = val

perms = [ (names[-1],) + p for p in permutations(names[:-1]) ]

counter = 0
for perm in perms:
    for (a, b) in zip(perm, perm[1:] + perm):
        counter += table[a][b] +  table[b][a]
    if counter > result:
        result = counter
    counter = 0

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

