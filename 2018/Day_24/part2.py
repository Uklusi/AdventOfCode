from collections import defaultdict
from copy import deepcopy
result = 0

logFile = open("log2.txt", "w")

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

def calcWinner(groups, boost=0):

    groups = deepcopy(groups)

    immune = [g for g in groups if g.faction == "Immune"]
    infect = [g for g in groups if g.faction == "Infect"]
    for g in immune:
        g.damage += boost
    
    groups.sort(reverse=True)
    
    oldTotUnitsAlive = -10
    infiniteLoop = False

    while len(immune) > 0 and len(infect) > 0:
        
        # Code for battle
        # Target selection
        enemies = {"Immune": infect, "Infect": immune}
        totUnitsAlive = sum([g.numUnits for g in groups])
        if totUnitsAlive == oldTotUnitsAlive:
            infiniteLoop = True
            break
        
        if boost == 49:
            printLog("Immune System:")
            printLog("Num, HP, Dam")
            printLog("\n".join([str(s.debug()) for s in immune] + ["DEAD" for _ in range(10 - len(immune))]))
            printLog()
            printLog("Infection:")
            printLog("Num, HP, Dam")
            printLog("\n".join([str(s.debug()) for s in infect] + ["DEAD" for _ in range(10 - len(infect))]))
            printLog()
            printLog()

        for g in groups:
            en = [e for e in enemies[g.faction] if not e.attacked]
            if len(en) > 0:
                target = max(en, key=lambda e: (g.attackDamage(e), e))
                if g.attackDamage(target) > 0:
                    g.attacking = True
                    g.target = target
                    target.attacked = True

        # Damage phase
        groups.sort(reverse=True, key=lambda g: g.initiative)
        for g in groups:
            g.attack()
        
        # Cleanup
        groups = [g for g in groups if not g.dead]
        groups.sort(reverse=True)
        for g in groups:
            g.attacking = False
            g.target = None
            g.attacked = False
        immune = [g for g in groups if g.faction == "Immune"]
        infect = [g for g in groups if g.faction == "Infect"]
        oldTotUnitsAlive = totUnitsAlive

    if infiniteLoop:
        return ("Infect", None)
    groups = [g for g in groups if not g.dead]
    return (groups[0].faction, groups)
        

reindeerWon = False
boost = 0
while not reindeerWon:
    boost += 1
    (winningFaction, alive) = calcWinner(groups, boost)
    reindeerWon = winningFaction == "Immune"
    # print(boost)

result = sum([g.numUnits for g in alive if not g.dead])

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

