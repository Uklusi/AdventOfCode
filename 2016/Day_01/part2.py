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
visited = {pos: True}
flag = False

for direction in directions:
    pos.turn(direction["turn"])
    for _ in range(direction["blocks"]):
        pos = pos.copy()
        pos.move(1)
        if pos in visited:
            flag = True
            break
        else:
            visited[pos] = True
    if flag:
        break

result = abs(pos.x) + abs(pos.y)

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

