result = 0

hp = "hp"
damage = "damage"
armour = "armour"
cost = "cost"
mana = "mana"
duration = "duration"
heal = "heal"
manaheal = "manaheal"
magname= "magname"
totmana = "totmana"

boss = {
    hp: 55,
    damage: 8,
    armour: 0
}

player = {
    hp: 50,
    armour: 0,
    mana: 500,
    totmana: 0
}

magmis = "magmis"
drain = "drain"
shield = "shield"
poison = "poison"
recharge = "recharge"

magicList = [
    {
        magname: magmis,
        mana: 53,
        damage: 4,
        duration: 0
    },
    {
        magname: drain,
        mana: 73,
        damage: 2,
        heal: 2,
        duration: 0
    },
    {
        magname: shield,
        mana: 113,
        duration: 6,
        armour: 7
    },
    {
        magname: poison,
        mana: 173,
        duration: 6,
        damage: 3
    },
    {
        magname: recharge,
        mana: 229,
        duration: 5,
        manaheal: 101
    }
]

magicNames = [m[magname] for m in magicList]

magDuration = {m: 0 for m in magicNames}

def castMagic(pstats, bstats, magDuration, cast=None):
    pstats = pstats.copy()
    bstats = bstats.copy()
    magDuration = magDuration.copy()
    castName = cast[magname]
    if pstats[mana] < cast[mana]:
        return (pstats, bstats, magDuration, True)
    if magDuration[castName] > 0:
        return (pstats, bstats, magDuration, True)
    pstats[mana] -= cast[mana]
    pstats[totmana] += cast[mana]
    magDuration[castName] = cast[duration]
    if castName == magmis:
        bstats[hp] -= cast[damage]
    elif castName == drain:
        bstats[hp] -= cast[damage]
        pstats[hp] += cast[heal]
    return (pstats, bstats, magDuration, False)
    
def applyDurationEffects(pstats, bstats, magDuration):
    pstats = pstats.copy()
    bstats = bstats.copy()
    magDuration = magDuration.copy()
    for (m, d) in magDuration.items():
        if d != 0:
            cast = magicList[magicNames.index(m)]
            magDuration[m] = d-1
            if m == poison:
                bstats[hp] -= cast[damage]
            elif m == shield:
                pstats[armour] = cast[armour]
            elif m == recharge:
                pstats[mana] += cast[manaheal]
        if m == shield and d == 0: # Sono un idiota
            pstats[armour] = 0
    return (pstats, bstats, magDuration, False)

def checkEnd(pstats, bstats):
    if pstats[hp] <= 0:
        return (True, False)
    elif bstats[hp] <= 0:
        return (True, True)
    else:
        return (False, None)


def calcDamage(d, a):
    return max(d-a, 1)

result = 999999



def turn(pstats, bstats, magDuration, active=0, cast=None):
    pstats = pstats.copy()
    bstats = bstats.copy()
    magDuration = magDuration.copy()
    (pstats, bstats, magDuration, error) = applyDurationEffects(pstats, bstats, magDuration)
    (end, pwin) = checkEnd(pstats, bstats)
    if end:
        return (pstats, bstats, magDuration, False, end, pwin)
    if active == 0:
        (pstats, bstats, magDuration, failure) = castMagic(pstats, bstats, magDuration, cast)
        (end, pwin) = checkEnd(pstats, bstats)
        return (pstats, bstats, magDuration, failure, end, pwin)
    elif active == 1:
        pstats[hp] -= calcDamage(bstats[damage], pstats[armour])
        (end, pwin) = checkEnd(pstats, bstats)
        return (pstats, bstats, magDuration, False, end, pwin)

def calcGame(pstats, bstats, magDuration, active=0, castList=[]):
    pstats = pstats.copy()
    bstats = bstats.copy()
    magDuration = magDuration.copy()
    castList = castList.copy()
    global result
    if pstats[totmana] >= result:
        return
    if active == 1:
        (pstats2, bstats2, magDuration, failure, end, pwin) = turn(pstats, bstats, magDuration, active=active)
        if end:
            if pwin:
                result = min(result, pstats2[totmana])
            return
        calcGame(pstats2, bstats2, magDuration, 0, castList)
        return
    else:
        for c in magicList:
            (pstats2, bstats2, magDuration2, failure, end, pwin) = turn(pstats, bstats, magDuration, active=active, cast=c)
            if failure:
                continue
            elif end:
                if pwin:
                    result = min(result, pstats2[totmana])
                    # print(castList +  [c[magname]], pstats2[totmana])
                continue
            calcGame(pstats2, bstats2, magDuration2, 1, castList + [c[magname]])
        return
        
calcGame(player.copy(), boss.copy(), magDuration, 0, [])


with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

