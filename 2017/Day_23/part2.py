"""b = 99
c = b
if a != 0:
    
    b = 109900
    
    c = 126900 // c = b + 17000

f = 1
d = 2
do{ 
    e = 2
    do{ 
        
        if d*e == b:
            f = 0
        e = e + 1
        
        
    } while e != b
    d = d + 1
    
    
while d != b
if f == 0:
    h = h + 1


if b != c:

    b = b + 17
    goto 9
"""
result = 0
for n in range(109900, 126901, 17):
    composite = False
    sqn = int(n**(1/2))
    for k in range(2, sqn + 1):
        if n % k == 0:
            composite = True
            break
    if composite:
        result += 1

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))