from copy import deepcopy
from math import prod
result = 0
presents = []
minNum = 5
minQE = 9999999999999999999999999999

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip()
        presents.append(int(line))

s = sum(presents)
target = s // 4
presents.sort(reverse=True)
print(s, target)

class State:
    def __init__(self, presents):
        self.presents = presents.copy()
        self.front = []
        self.left = []
        self.right = []
        self.back = []
    
    def copy(self):
        return deepcopy(self)

    def num(self):
        return len(self.front)

    def QE(self):
        return prod(self.front)

    def __repr__(self):
        return "presents: " + str(self.presents) +\
        "\nfront: " + str(self.front) + \
        "\nleft: " + str(self.left) + \
        "\nright: " + str(self.right)
        
def splitPackages(state):
    # breakpoint()
    f = sum(state.front)
    l = sum(state.left)
    r = sum(state.right)
    global minNum
    global minQE
    num = state.num()
    QE = state.QE()
    if num > minNum:
        return
    elif num == minNum and QE >= minQE:
        return
    if f != target:
        if (minNum - num) * (state.front[-1] if len(state.front) else presents[0]) + f < target:
            return
        for present in state.presents:
            if present <= target - f and (
                len(state.front) == 0 or present < state.front[-1]
            ):
                newState = state.copy()
                newState.presents.remove(present)
                newState.front.append(present)
                num = newState.num()
                QE = newState.QE()
                if num > minNum:
                    return
                elif num == minNum and QE >= minQE:
                    return
                splitPackages(newState)
    # No guarantee that the result is correct, but I want to try
    # Thank god it works
    
    # elif l != target:
    #     for present in state.presents:
    #         if present <= target - l and (
    #             len(state.left) == 0 or present < state.left[-1]
    #         ):
    #             newState = state.copy()
    #             newState.presents.remove(present)
    #             newState.left.append(present)
    #             num = newState.num()
    #             QE = newState.QE()
    #             if num > minNum:
    #                 return
    #             elif num == minNum and QE >= minQE:
    #                 return
    #             splitPackages(newState)
    # elif r != target:
    #     for present in state.presents:
    #         if present <= target - r and (
    #             len(state.right) == 0 or present < state.right[-1]
    #         ):
    #             newState = state.copy()
    #             newState.presents.remove(present)
    #             newState.left.append(present)
    #             num = newState.num()
    #             QE = newState.QE()
    #             if num > minNum:
    #                 return
    #             elif num == minNum and QE >= minQE:
    #                 return
    #             splitPackages(newState)
    else:
        num = state.num()
        QE = state.QE()
        if num < minNum:
            minNum = num
            minQE = QE
            print(state)
            print()
        elif num == minNum:
            minQE = min(QE, minQE)
            print(state)
            print()

state = State(presents)

splitPackages(state)

result = minQE

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

