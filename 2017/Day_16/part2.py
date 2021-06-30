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
                instructions.append([letter, n])
            else:
                if letter == "x":
                    (n1, n2) = map(int, rest.split("/"))
                    instructions.append([letter, n1, n2])
                elif letter == "p":
                    (l1, l2) = rest.split("/")
                    instructions.append([letter, l1, l2])

nums = list(range(16))
for s in instructions:
    (letter, rest) = (s[0], s[1:])
    if letter == "s":
        n = rest[0]
        nums = nums[-n:] + nums[:-n]
    elif letter == "x":
        (n1, n2) = rest
        (nums[n2], nums[n1]) = (nums[n1], nums[n2])
    # instruction "p" commutes with everything and is idempotent
    # meaning that applying it two times has no effect
    # We can ignore it since we would need to repeat it 10**9 times, which is an even number

# Ignoring "p" means that each iteration is a permutation of the programs,
# and this permutation is represented by nums

test = nums.copy()

for k in range(1, 10**9):
    test2 = test.copy()
    for i in range(16):
        test[i] = test2[nums[i]]
    if test == nums:
        break

# k is the permutation order (aka the number of times it needs to be repeated)
# to get to the trivial permutation). For my input k = 12
# Since applying the permutation k times is equivalent to doing nothing,
# We can reduce drastically the iteration number by taking the remainder

for _ in range((10**9) % k):
    programs2 = programs.copy()
    for i in range(16):
        programs[i] = programs2[nums[i]]

result = "".join(programs)


with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

