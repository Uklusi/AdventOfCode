result = 0
programs = [chr(ord("a") + i) for i in range(16)]
instructions = []

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip().split(",")
        for s in line:
            (letter, rest) = (s[0], s[1:])
            if letter == "s":
                n = int(rest)
                programs = programs[-n:] + programs[:-n]
            else:
                if letter == "x":
                    (n1, n2) = map(int, rest.split("/"))
                elif letter == "p":
                    (n1, n2) = map(programs.index, rest.split("/"))
                (programs[n2], programs[n1]) = (programs[n1], programs[n2])
            
result = "".join(programs)


with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

