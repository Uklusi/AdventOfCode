result = 0

input = 380621
# input = 9

i = 0
j = 1
recipes = [3, 7]

while len(recipes) < input + 10:
    n = recipes[i] + recipes[j]
    if n > 9:
        recipes.append(n // 10)
        recipes.append(n % 10)
    else:
        recipes.append(n)
    i = (i + recipes[i] + 1) % len(recipes)
    j = (j + recipes[j] + 1) % len(recipes)

result = "".join(map(str, recipes[input: input+10]))

with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

