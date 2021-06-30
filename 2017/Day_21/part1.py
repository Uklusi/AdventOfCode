from AOCClasses import Image
from functools import reduce
from operator import add, and_
result = 0

start = Image("""\
.#.
..#
###\
""".split())

image = start.copy()

converter = {}

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip().split(" => ")
        inImg = Image(line[0].split("/"))
        outImg = Image(line[1].split("/"))
        for img in inImg.variations():
            converter[img] = outImg

for _ in range(5):
    s = image.shape[0]
    if s % 2 == 0:
        k = 2
    elif s % 3 == 0:
        k = 3
    else:
        raise(Exception(f"UnknownDimension: {s}"))

    image = reduce(and_, [
        reduce(add, [
            converter[image.slice(x=(i, i+k), y=(j, j+k))] for i in range(0, s, k)
        ] ) for j in range(0, s, k)
    ] )

result = image.image().count("#")


with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

