from functools import cache
result = 0

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip().split()
        treeList = list(map(int, line))

class Node():
    def __init__(self, id, children, metadata):
        self.children = children
        self.metadata = metadata
        self.id = id
    
    def __hash__(self):
        return hash(self.id)
    

def readNode(tape, index):
    childrenNum = tape[index]
    metadataNum = tape[index+1]
    index += 2
    children = []
    metadata = []
    for _ in range(childrenNum):
        (index, child) = readNode(tape, index)
        children.append(child)
    metadata = tape[index : index + metadataNum]
    index += metadataNum
    return (index, Node(index, children, metadata))

(_, root) = readNode(treeList, 0)

@cache
def value(node):
    if len(node.children) == 0:
        return sum(node.metadata)
    else:
        ret = 0
        for k in node.metadata:
            n = k-1
            if 0 <= n < len(node.children):
                ret += value(node.children[n])
        return ret

result = value(root)

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

