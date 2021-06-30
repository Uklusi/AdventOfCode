from AOCClasses import LinkedList
result = 0
"452 players; last marble is worth 70784 points"

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip().split()
        playerNum = int(line[0])
        LIMIT = int(line[6]) * 100

marbles = LinkedList(data=0)
playerPoints = [0 for _ in range(playerNum)]

player = 0
for k in range(1, LIMIT + 1):
    if k % 23 != 0:
        marbles = marbles.move(1)
        marbles = marbles.add(k)
    else:
        playerPoints[player] += k
        marbles = marbles.move(-7)
        playerPoints[player] += marbles.data
        marbles = marbles.delete()
    player = (player + 1) % playerNum

result = max(playerPoints)

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

