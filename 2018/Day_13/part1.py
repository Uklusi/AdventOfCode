from AOCClasses import *
result = 0

frame = []
with open("input.txt", "r") as input:
    for line in input:
        frame.append(line)

def isSolid(p):
    return frame[p.y][p.x] == " "

carts = []
for (i,line) in enumerate(frame):
    for (j, c) in enumerate(line):
        orientation = None
        if c == "^":
            orientation = "U"
        elif c == ">":
            orientation = "R"
        elif c == "v":
            orientation = "D"
        elif c == "<":
            orientation = "L"
        if orientation is not None:
            cart = SolidPosition(x=j, y=i, orientation=orientation, reverseY=True, frame=frame, solid=isSolid)
            cart.nturns = 0
            carts.append(cart)

carts.sort(key= lambda p: (p.y, p.x))

def moveCart(cart):
    tile = frame[cart.y][cart.x]
    # print(tile, cart, cart.orientation)
    if tile in ["/", "\\"]:
        orient = cart.orientation
        turnDirection = 1
        if tile == "/":
            turnDirection *= -1
        if orient in [0,2]:
            turnDirection *= -1
        cart.turn(turnDirection)
    elif tile == "+":
        cart.turn((cart.nturns % 3) - 1)
        cart.nturns += 1
    cart.move(n=1)
    # print(cart)

collision = False

while not collision:
    for cart in carts:
        # print(cart)
        moveCart(cart)
        # print(cart)
        if carts.count(cart) > 1:
            collision = True
            result = f"{cart.x},{cart.y}"
            break
    carts.sort(key= lambda p: (p.y, p.x))



with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

