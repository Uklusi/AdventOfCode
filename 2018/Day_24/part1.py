from collections import defaultdict
result = 0

logFile = open("log1.txt", "w")

def printLog(*s):
    s = " ".join([str(i) for i in s])
    logFile.write(s)
    logFile.write("\n")

class Group():
    def __init__(
        self,
        id,
        faction,
        numUnits,
        hitPoints,
        weaknesses,
        immunities,
        damage,
        damageType,
        initiative
    ):
        self.id = id
        self.faction = faction
        self.numUnits = numUnits
        self.hitPoints = hitPoints
        self.effectiveness = defaultdict(lambda: 1)
        self.weaknesses = weaknesses
        self.immunities = immunities
        for w in weaknesses:
            self.effectiveness[w] = 2
        for i in immunities:
            self.effectiveness[i] = 0
        self.damage = damage
        self.damageType = damageType
        self.initiative = initiative
        self.attacking = False
        self.target = None
        self.attacked = False

    @property
    def effectivePower(self):
        n = self.numUnits
        return (n if n > 0 else 0) * self.damage
    
    @property
    def dead(self):
        return self.numUnits <= 0

    def allData(self):
        wAndI = ""
        if len(self.weaknesses) > 0:
            w = ", ".join(self.weaknesses)
            wAndI += f"Weak to {w}; "
        if len(self.immunities) > 0:
            i = ", ".join(self.immunities)
            wAndI += f"Immune to {i}; "
        return f"{self.faction} {self.id:>2d} - EP: {self.effectivePower:>5d}, IN: {self.initiative:>2d}: {self.numUnits:>4d} × {self.hitPoints:>5d} HP - {self.damage:>4d} {self.damageType + ';':<12s} {wAndI}"

    def name(self):
        return f"{self.faction} {self.id:>2d}"
    
    def varData(self):
        return f"{self.name()} - EP: {self.effectivePower:>5d}, {self.numUnits:>4d} units - {self.damageType:<11s}"
    
    def debug(self):
        return [self.numUnits, self.hitPoints, self.damage, self.damageType, self.initiative]

    def __str__(self):
        return self.name()

    def __eq__(self, other):
        return (self.effectivePower, self.initiative) == (other.effectivePower, other.initiative)
        
    def __lt__(self, other):
        return (self.effectivePower, self.initiative) < (other.effectivePower, other.initiative)

    def __le__(self, other):
        return (self.effectivePower, self.initiative) <= (other.effectivePower, other.initiative)

    def attackDamage(self, other):
        modifier = other.effectiveness[self.damageType]
        return self.effectivePower * modifier

    def attack(self):
        if not self.attacking or self.dead:
            return
        other = self.target
        dam = self.attackDamage(other)
        hp = other.hitPoints
        killed = dam // hp
        other.numUnits -= killed

groups = []

with open("input.txt", "r") as input:
    data = input.read().split("\n\n")
    for (i, groupData) in enumerate(data):
        faction = "Immune" if i == 0 else "Infect"
        for (n, groupLine) in enumerate(groupData.split("\n")[1:]):
            groupLine = groupLine.strip()
            (unitData, otherData) = groupLine.split(" hit points")
            (weakImmData, attackData) = otherData.split("with an attack that does ")
            unit = unitData.split()
            weakImm = weakImmData.strip().strip("()").split("; ")
            attack = attackData.split()

            num = int(unit[0])
            hp = int(unit[4])
            dam = int(attack[0])
            damType = attack[1]
            init = int(attack[-1])
            w = []
            i = []
            if len(weakImm) == 1:
                weakImm = weakImm[0]
                if len(weakImm) > 0:
                    weakImm = weakImm.replace(",", "").split()
                    if weakImm[0] == "weak":
                        w = weakImm[2:]
                    else:
                        i = weakImm[2:]
            else:
                w = weakImm[0].replace(",", "").split()
                i = weakImm[1].replace(",", "").split()
                if w[0] == "weak":
                    (w, i) = (w[2:], i[2:])
                else:
                    (w, i) = (i[2:], w[2:])

            
            group = Group(
                id = n + 1,
                faction = faction,
                numUnits = num,
                hitPoints = hp,
                weaknesses = w,
                immunities = i,
                damage = dam,
                damageType = damType,
                initiative = init
            )
            groups.append(group)

# for g in groups:
#     printLog(g.allData())
# printLog()
# printLog()

# dTypes = ["fire", "slashing", "cold", "radiation", "bludgeoning"]
# for t in dTypes:
#     groups.sort(reverse=True, key=lambda g:(g.faction, g.effectiveness[t], g.initiative))
#     printLog(f"{t} Damage")
#     for g in groups:
#         printLog(g.effectiveness[t], g.allData())
#     printLog()
#     printLog()

groups.sort(reverse=True)

immune = [g for g in groups if g.faction == "Immune"]
infect = [g for g in groups if g.faction == "Infect"]
while len(immune) > 0 and len(infect) > 0:
# for counter in range(2, 7):
    
    # Code for battle
    # Target selection
    enemies = {"Immune": infect, "Infect": immune}
    
    # printLog("Immune System:")
    # printLog("Num, HP, Dam")
    # printLog("\n".join([str(s.debug()) for s in immune] + ["DEAD" for _ in range(10 - len(immune))]))
    # printLog()
    # printLog("Infection:")
    # printLog("Num, HP, Dam")
    # printLog("\n".join([str(s.debug()) for s in infect] + ["DEAD" for _ in range(10 - len(infect))]))
    # printLog()
    # printLog()

    for g in groups:
        en = [e for e in enemies[g.faction] if not e.attacked]
        # en = [e for e in enemies[g.faction] if not e.attacked and not e.dead]
        if len(en) > 0:
        # if not g.dead and len(en) > 0:
            target = max(en, key=lambda e: (g.attackDamage(e), e))
            if g.attackDamage(target) > 0:
                g.attacking = True
                g.target = target
                target.attacked = True

    # if counter == 5:
    #     for g in groups:
    #         t = g.target
    #         t = "None" if t is None else t.name()
    #         printLog(f"{g.varData():<35s}  -----  {t}")
    #     printLog()

    # Damage phase
    groups.sort(reverse=True, key=lambda g: g.initiative)
    for g in groups:
        g.attack()
    #     if g.attacking:
    #         damage = g.attackDamage(g.target)
    #         killing = damage // g.target.hitPoints
    #         printLog(f"{g.varData():<35s}  -----  {damage:>8d} damage to {g.target.name():>10s}, killing {killing:>4d}")
    # printLog()
    # printLog()

    # Cleanup
    groups = [g for g in groups if not g.dead]
    groups.sort(reverse=True)
    for g in groups:
        g.attacking = False
        g.target = None
        g.attacked = False
    immune = [g for g in groups if g.faction == "Immune"]
    infect = [g for g in groups if g.faction == "Infect"]
    

result = sum([g.numUnits for g in groups if not g.dead])


with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

