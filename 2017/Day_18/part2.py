result = 0

from concurrent.futures import ThreadPoolExecutor
from collections import defaultdict
from threading import Lock
from queue import Queue

# class Pipeline():
#     def __init__(self):
#         self.queue = queue()
#         self.counter = Semaphore()
    
#     def get(self):
#         self.counter.acquire()
#         return self.queue

instructions = []


def readval(regs, regOrVal):
    if regOrVal.isalpha():
        return regs[regOrVal]
    else:
        return int(regOrVal)

with open("input.txt", "r") as input:
    for line in input:
        line = line.strip().split()
        instructions.append(line)

queue0 = Queue()
queue1 = Queue()
locks = [Lock(), Lock()]
snumb0 = []
snumb1 = []

def thread_function(i, queues):
    global snumb0
    global snumb1
    snumb = [snumb0,snumb1][i]
    receive = queues[i]
    send = queues[1-i]
    regs = defaultdict(lambda: 0)
    regs["p"] = i
    index = 0
    while 0 <= index < len(instructions):
        instr = instructions[index]
        name = instr[0]
        # print(f"{name} {i} - l{index+1}")
        rv1 = instr[1]
        rv2 = instr[2] if len(instr) == 3 else "0"
        v1 = readval(regs, rv1)
        v2 = readval(regs, rv2)

        if name == "snd":
            snumb.append(1)
            send.put(v1)
        elif name == "rcv":
            lockrcv = locks[i]
            locksnd = locks[1-i]
            lockrcv.acquire()
            if locksnd.locked() and lockrcv.locked() and send.empty() and receive.empty():
                return
            try:
                regs[rv1] = receive.get(timeout=0.2)
            except:
                return
            lockrcv.release()
        elif name == "set":
            regs[rv1] = v2
        elif name == "add":
            regs[rv1] += v2
        elif name == "mul":
            regs[rv1] *= v2
        elif name == "mod":
            regs[rv1] %= v2
        elif name == "jgz":
            if v1 > 0:
                # -1 to compensate the index += 1 as default
                index += v2 - 1
        else:
            breakpoint()
            raise(Exception(f"Forgot instruction: {name}"))
        index += 1

queues = [queue0, queue1]

# thread_function(0,queues)

with ThreadPoolExecutor(max_workers=2) as executor:
    executor.map(thread_function, range(2), [queues, queues])

result = len(snumb1)

with open("output2.txt", "w") as output:
    output.write(str(result))
    print(str(result))

