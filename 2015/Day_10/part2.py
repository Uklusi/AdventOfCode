result = 0
rounds = 50

starting_seq = "1113122113"
lookSaySeq = starting_seq
# print(starting_seq)

for _ in range(rounds):
    lookSaySeq += "#"
    newLookSaySeq = ""
    curr_char = lookSaySeq[0]
    curr_count = 0
    for char in lookSaySeq:
        if char == curr_char:
            curr_count += 1
        else:
            newLookSaySeq += str(curr_count) + curr_char
            curr_char = char
            curr_count = 1
    lookSaySeq = newLookSaySeq
    # print(lookSaySeq)

result = len(lookSaySeq)

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

