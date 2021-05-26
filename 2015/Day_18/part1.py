result = 0
from gameoflife import GameOfLife

with open("input.txt", "r") as input:
    lines = input.read().strip().split()
    board = GameOfLife(lines, on="#", off=".")

for i in range(100):
    board.step()

result = str(board).count("#")

with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

