"""d = a
c = 7
b = 362
d++
b--
if b goto 4
c--
if c goto 3
a=d
null
b = a
a = 0
c = 2
if b goto 16
goto 21
b--
c--
if c goto 14
a++
goto 13
b= 2
if c goto 24
goto 27
b--
c--
goto 22
null
out b
if a goto 10
goto 9"""
# This is the translated code

# Python equivalent is
if False:
    a = input()
    d = a + 362 * 7
    while True:
        a = d
        while a > 0:
            b = a % 2
            a = a // 2
            print(b)

    # Alternatively
    while True:
        for c in f"{d:b}":
            print(c)

# Result is 
result = 0b101010101010 - 362 * 7

with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))
