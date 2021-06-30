from AOCClasses import Position
result = ""
document = []

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        document.append(list(line))

def sign(num):
    return 1 if num > 0 else -1 if num < 0 else 0

pos = Position(0,0)

def keypad(pos):
    return pos.x -pos.y*3 + 5

for instructions in document:
    for movement in instructions:
        pos.move(1, movement)
        if abs(pos.x) > 1:
            pos.x -= sign(pos.x)
        if abs(pos.y) > 1:
            pos.y -= sign(pos.y)
    n = str(keypad(pos))
    result += n
    
with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

