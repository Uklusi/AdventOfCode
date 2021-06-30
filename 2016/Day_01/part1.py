from position import Position

result = 0
directions = []

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        commands = line.split(", ")
        for c in commands:
            directions.append({
                "turn": c[0],
                "blocks": int(c[1:])
            })

pos = Position(0,0,0)

for direction in directions:
    pos.turn(direction["turn"])
    pos.move(direction["blocks"])

result = abs(pos.x) + abs(pos.y)


with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

