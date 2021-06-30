result = 0
ROWS = 6
COLUMNS = 50

screen = [[0 for _ in range(COLUMNS)] for __ in range(ROWS)]

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        parsing = line.split()
        if parsing[0] == "rect":
            [x, y] = [int(n) for n in parsing[1].split("x")]
            for i in range(x):
                for j in range(y):
                    screen[j][i] = 1
        elif parsing[1] == "row":
            (_, row) = parsing[2].split("=")
            row = int(row)
            shift = int(parsing[4])
            screen[row] = screen[row][-shift:] + screen[row][:-shift]
        else:
            (_, col) = parsing[2].split("=")
            col = int(col)
            shift = int(parsing[4])
            for _ in range(shift):
                next = screen[-1][col]
                for row in range(ROWS):
                    temp = screen[row][col]
                    screen[row][col] = next
                    next = temp

def printScreen(screen):
    return "\n".join(["".join([str(p) for p in r]) for r in screen]).replace("0", " ").replace("1", "*")

result = sum([sum(r) for r in screen])


with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

