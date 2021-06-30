from AOCClasses import Image
from functools import reduce
from operator import add, and_
result = 0

start = Image("""\
.#.
..#
###\
""".split())

converter = {}
img3 = []
converter3_9 = {}

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip().split(" => ")
        inImg = Image(line[0].split("/"))
        outImg = Image(line[1].split("/"))
        if inImg.shape[0] == 3:
            img3.append(inImg)
        for img in inImg.variations():
            converter[img] = outImg

for image0 in img3:
    
    image = image0.copy()
    for _ in range(3):
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
    for i in image0.variations():
        converter3_9[i] = image
    
result = converter3_9[start]

converter1 = converter3_9
converter2 = {}
for _ in range(5):
    for image in set(converter1.values()):
        
        s = image.shape[0]
        k = s // 3
        image2 = reduce(and_, [
            reduce(add, [
                converter1[image.slice(x=(i, i+k), y=(j, j+k))] for i in range(0, s, k)
            ] ) for j in range(0, s, k)
        ] )
        converter2[image] = image2
    converter1 = converter2
    converter2 = {}
    result = converter1[result]
    print(result.shape)


result = result.image().count("#")

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

