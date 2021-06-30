result = 0

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip().split()
        treeList = list(map(int, line))

class Node():
    def __init__(self, children, metadata):
        self.children = children
        self.metadata = metadata

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
    return (index, Node(children, metadata))

(_, root) = readNode(treeList, 0)

stack = [root]
while len(stack) > 0:
    node = stack.pop()
    stack += node.children
    result += sum(node.metadata)

with open("output1.txt", "w") as output:
    output.write(str(result))
    print(str(result))

