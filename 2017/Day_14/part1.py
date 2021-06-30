from knotHash import knotHash
result = 0

input = "jzgqcdpd"
# input = "flqrgnkx"

input += "-"

for i in range(128):
    hash_i = knotHash(input + str(i))
    n = int(hash_i, 16)
    bin_n = f"{n:b}"
    result += bin_n.count("1")

with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

