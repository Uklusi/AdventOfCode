result = 0
"452 players; last marble is worth 70784 points"

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip().split()
        playerNum = int(line[0])
        LIMIT = int(line[6])

marbles = [0]
i = 0
playerPoints = [0 for _ in range(playerNum)]

player = 0
for k in range(1, LIMIT + 1):
    l = len(marbles)
    if k % 23 != 0:
        i = (i+2) % l
        marbles = marbles[:i] + [k] + marbles[i:]
    else:
        playerPoints[player] += k
        i = (i - 7) % l
        playerPoints[player] += marbles[i]
        marbles = marbles[:i] + marbles[i+1:]
    player = (player + 1) % playerNum

result = max(playerPoints)

with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

