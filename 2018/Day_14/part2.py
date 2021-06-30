result = 0

input = 380621
# input = 59414

input = list(map(int, str(input)))
l = len(input)
i = 0
j = 1
recipes = [3, 7]

while recipes[-l:] != input and recipes[-l-1:-1] != input:
    n = recipes[i] + recipes[j]
    if n > 9:
        recipes.append(n // 10)
        recipes.append(n % 10)
    else:
        recipes.append(n)
    i = (i + recipes[i] + 1) % len(recipes)
    j = (j + recipes[j] + 1) % len(recipes)

result = len(recipes) - l
if recipes[-l-1:-1] == input:
    result -= 1

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

