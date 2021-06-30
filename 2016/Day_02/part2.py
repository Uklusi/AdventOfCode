from AOCClasses import Position
result = ""
document = []

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        document.append(list(line))

pos = Position(-2,0)

def keypad(pos):
    return {
        ( 0,  2): "1",
        (-1,  1): "2",
        ( 0,  1): "3",
        ( 1,  1): "4",
        (-2,  0): "5",
        (-1,  0): "6",
        ( 0,  0): "7",
        ( 1,  0): "8",
        ( 2,  0): "9",
        (-1, -1): "A",
        ( 0, -1): "B",
        ( 1, -1): "C",
        ( 0, -2): "D"
    }[pos.current()]

for instructions in document:
    for movement in instructions:
        pos2 = pos.copy()
        pos.move(1, movement)
        if abs(pos.x) + abs(pos.y) > 2:
            pos = pos2
    result += keypad(pos)
    
with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

