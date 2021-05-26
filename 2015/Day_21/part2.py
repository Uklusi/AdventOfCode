result = 0

hp = "hp"
damage = "damage"
armour = "armour"
cost = "cost"

boss = {
    hp: 103,
    damage: 9,
    armour: 2
}

def calcDamage(d, a):
    return max(d-a, 1)

player = {
    hp: 100
}

weapons = [
    {cost:  8, damage: 4, armour: 0},
    {cost: 10, damage: 5, armour: 0},
    {cost: 25, damage: 6, armour: 0},
    {cost: 40, damage: 7, armour: 0},
    {cost: 74, damage: 8, armour: 0}
]

mails = [
    {cost:   0, damage: 0, armour: 0},
    {cost:  13, damage: 0, armour: 1},
    {cost:  31, damage: 0, armour: 2},
    {cost:  53, damage: 0, armour: 3},
    {cost:  75, damage: 0, armour: 4},
    {cost: 102, damage: 0, armour: 5}
]

rings = [
    {cost:   0, damage: 0, armour: 0, "id": 0},
    {cost:  20, damage: 0, armour: 1},
    {cost:  25, damage: 1, armour: 0},
    {cost:  40, damage: 0, armour: 2},
    {cost:  50, damage: 2, armour: 0},
    {cost:  80, damage: 0, armour: 3},
    {cost: 100, damage: 3, armour: 0}
]

stats = []
for w in weapons:
    for m in mails:
        for (i, r1) in enumerate(rings[:-1]):
            j = i + 1 if i else i
            for r2 in rings[j:]:
                stats.append({
                    p: sum([o[p] for o in [w,m,r1,r2]]) for p in [cost, damage, armour]
                })

stats.sort(reverse=True, key=lambda x: x[cost])

def combat(stat):
    bossHP = boss[hp]
    playerHP = player[hp]
    turn = 0
    while bossHP > 0 and playerHP > 0:
        if turn == 0:
            bossHP -= calcDamage(stat[damage], boss[armour])
        else:
            playerHP -= calcDamage(boss[damage], stat[armour])
        turn = 1 - turn
    return bossHP <= 0


for stat in stats:
    victory = combat(stat)
    if not victory:
        result = stat[cost]
        break

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

