from AOCClasses import Position

result = 0

input = 368078

def oddsqrt(n):
    m = int(n ** (1/2))
    if m % 2 == 0:
        m -= 1
    return m

start = Position(0,0)

odd = oddsqrt(input)
even = odd + 1
oddSquare = odd ** 2
squarePosition = Position(odd // 2, - (odd // 2))

diff = input - oddSquare
offsetPos = Position(0,0)
offsetPos += (diff if diff <= even else even) * Position(0,1)
diff = max(diff - even, 0)
offsetPos += (diff if diff <= even else even) * Position(-1,0)
diff = max(diff - even, 0)
offsetPos += (diff if diff <= even else even) * Position(0,-1)
diff = max(diff - even, 0)
offsetPos += (diff if diff <= even else even) * Position(1,0)
diff = max(diff - even, 0)

inputPosition = squarePosition + offsetPos

result = start - inputPosition


with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

