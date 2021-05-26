result = 0
from gameoflife import GameOfLife

with open("input.txt", "r") as input:
    lines = input.read().strip().split()
    board = GameOfLife(lines, on="#", off=".")
    board.state[0][0] = 1
    board.state[0][-1] = 1
    board.state[-1][0] = 1
    board.state[-1][-1] = 1


for i in range(100):
    board.step()
    board.state[0][0] = 1
    board.state[0][-1] = 1
    board.state[-1][0] = 1
    board.state[-1][-1] = 1

result = str(board).count("#")

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

