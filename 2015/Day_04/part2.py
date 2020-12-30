from hashlib import md5

input = "iwrupvqb"

n = 1

while True:
    strn = str(n)
    hashResult = md5( (input + strn).encode() ).hexdigest()
    if hashResult[:6] == "000000":
        result = strn
        break
    n += 1
    
with open("output2.txt", "w") as output:
    output.write( str(result) )
    print(str(result))