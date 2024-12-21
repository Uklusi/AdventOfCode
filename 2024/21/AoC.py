from collections import Counter
from itertools import accumulate, permutations, chain
from AoCUtils import *  # noqa: F401
from argparse import ArgumentParser
import time
from io import TextIOWrapper
import re
from typing import Any


class Logger:
    def __init__(self, log_file_name: str, write_to_log: bool = False):
        self.log_file: TextIOWrapper | None
        if write_to_log:
            self.log_file = open(log_file_name + ".log", "w", encoding="utf8")
        else:
            self.log_file = None

    def log(self, *t: Any, end: str = "\n"):
        if self.log_file is None:
            print(*t, end=end)
        else:
            self.log_file.write(" ".join([str(o) for o in t]) + end)
            self.log_file.flush()

    def close(self):
        if self.log_file is not None:
            self.log_file.close()


logger = Logger("log", write_to_log=False)
# logger = Logger("log", write_to_log=True)


class MyCounter(Counter[T]):
    def __mul__(self, other: int):
        return MyCounter({k: self[k] * other for k in self})

    def __hash__(self):
        return hash(tuple(self.items()))


@dataclass(frozen=True)
class KeyPad:
    keypad: tuple[str, ...]

    def whereis(self, curr_button: str):
        for y, row in enumerate(self.keypad):
            for x, button in enumerate(row):
                if button == curr_button:
                    return Position(x, y)
        raise ValueError(f"Wrong button: {curr_button}")

    @property
    def A(self):
        return self.whereis("A")

    @property
    def X(self):
        return self.whereis(" ")

    def __getitem__(self, key: Position):
        return self.keypad[key.y][key.x]


KEYPAD_NUM = KeyPad(("789", "456", "123", " 0A"))
KEYPAD_DIR = KeyPad((" ^A", "<v>"))


def perms(a: list[T], b: list[T]):
    return list(set(permutations(a + b)))


@dataclass(frozen=True)
class KeyCode:
    code: tuple[str, ...]
    keypad: KeyPad

    def __str__(self):
        return "".join(self.code)

    def __repr__(self):
        return "".join(self.code)

    def __len__(self):
        return len(self.code)

    def to_string(self):
        return str(self)

    def split_sections(self):
        sections = "".join(self.code).split("A")[:-1]
        return [KeyCode(tuple(section + "A"), self.keypad) for section in sections]

    def decode(self, new_keypad: KeyPad):
        k = new_keypad
        ret: list[str] = []
        p = k.A
        for instr in self.code:
            if instr == "A":
                ret.append(new_keypad[p])
            else:
                p += Vector.fromDirection(instr)
        return KeyCode(tuple(ret), new_keypad)

    def encode(self):
        k = self.keypad

        current = k.A
        BAD_POS = k.X

        def add_direction(p: Position, dir: str):
            return p + Vector.fromDirection(dir)

        ret: list[list[str]] = [[]]
        for button in self.code:
            target = k.whereis(button)
            movement = target - current
            code_v: list[str] = []
            code_h: list[str] = []
            if movement.vy > 0:
                code_v = ["v"]
            elif movement.vy < 0:
                code_v = ["^"]
            if movement.vx > 0:
                code_h = [">"]
            elif movement.vx < 0:
                code_h = ["<"]

            p = [code_h * abs(movement.vx) + code_v * abs(movement.vy)] + [
                code_v * abs(movement.vy) + code_h * abs(movement.vx)
            ]
            if p[0] == p[1]:
                p = [p[0]]
            ret = [
                list(a) + list(b)
                for (a, b) in product(
                    ret,
                    [
                        list(r) + ["A"]
                        for r in p
                        if BAD_POS not in accumulate(r, add_direction, initial=current)
                    ],
                )
            ]
            current = target
        return [KeyCode(tuple(listcode), KEYPAD_DIR) for listcode in ret]


def prntlist(lst: list[KeyCode]):
    return prettify(
        [
            {k.to_string(): v for (k, v) in Counter(lst.split_sections()).items()}
            for lst in lst
        ]
    )


def prntcounters(lst: list[MyCounter[KeyCode]]):
    return prettify([{k.to_string(): v for (k, v) in d.items()} for d in lst])


def solve_generic(num_iters: int, useExample: bool = False):
    result = 0

    input_reader = InputReader(useExample=useExample)  # noqa: F841
    codes = [KeyCode(tuple(line), KEYPAD_NUM) for line in input_reader.lines()]
    # logger.log([c.to_string() for c in codes[0].encode()])
    for curr_code in codes:
        mylist = [curr_code]
        for i in range(num_iters):
            newlist: list[KeyCode] = []
            for code in mylist:
                newlist += code.encode()
            min_len = min([len(code) for code in newlist])
            mylist = [code for code in newlist if len(code) == min_len]
            # if i == 2:
            #     logger.log(
            #         prntlist(mylist)
            #     )
            #     # logger.log(mylist)
        # logger.log(curr_code.to_string(), len(mylist[0]), len(mylist))
        result += int("".join(curr_code.code[:3])) * len(mylist[0])

    return result


def transform(input: list[KeyCode]) -> list[KeyCode]:
    ret = set()
    for k in input:
        ret.update(k.encode())
    min_len = min([len(code) for code in ret])
    return [code for code in ret if len(code) == min_len]


def flatten(input: Iterable[Iterable[T]]) -> list[T]:
    return list(chain.from_iterable(input))


# def choose_num_alternative(num_code: KeyCode):
#     alternatives = num_code.encode()
#     a = set(flatten([alt.split_sections() for alt in alternatives]))
#     for k in a:
#         choose_alternative(k)

@cache
def choose_alternative(curr_code: KeyCode):
    alternatives = curr_code.encode()
    if len(alternatives) == 1:
        return MyCounter(alternatives[0].split_sections())

    # transformations = {possible: [possible] for possible in alternatives}
    # for i in range(5):
    #     if len(transformations) == 1:
    #         ret = list(transformations.keys())[0]
    #         break
    #     next_tranformations: dict[KeyCode, list[KeyCode]] = {}
    #     for possible in transformations:
    #         next_tranformations[possible] = transform(transformations[possible])
    #     transf_length = {
    #         possible: min(len(code) for code in next_tranformations[possible])
    #         for possible in transformations
    #     }
    #     transformations: dict[KeyCode, list[KeyCode]] = {
    #         possible: next_tranformations[possible]
    #         for possible in next_tranformations
    #         if transf_length[possible] == min(transf_length.values())
    #     }
    # ret = [k.split_sections() for k in transformations][0]
    # return MyCounter(ret)

    r: Counter[KeyCode] = Counter()
    for alt in alternatives:
        mylist = [alt]
        for i in range(2):
            newlist: list[KeyCode] = []
            for code in mylist:
                newlist += code.encode()
            min_len = min([len(code) for code in newlist])
            mylist = [code for code in newlist if len(code) == min_len]
        r[alt] = len(mylist[0])

    min_len = min(r.values())
    # TODO: With -1 here it works, with 0 it doesn't
    # In the first case uses ^>A instead of >^A, i don't know the difference and I frankly don't care
    ret = [k.split_sections() for k in r if r[k] == min_len][-1]
    # logger.log(curr_code, ret)
    return MyCounter(ret)


def solve_generic_new(num_iters: int, useExample: bool = False):
    result = 0

    input_reader = InputReader(useExample=useExample)  # noqa: F841
    codes = [KeyCode(tuple(line), KEYPAD_NUM) for line in input_reader.lines()]
    # logger.log([c.to_string() for c in codes[0].encode()])
    for curr_code in codes:
        start_count = MyCounter(curr_code.split_sections())
        count = start_count.copy()

        for i in range(num_iters):
            newCount: Counter[KeyCode] = Counter()
            for k in count:
                # choose_num_alternative(k)
                newCount += choose_alternative(k) * count[k]
            count = MyCounter(newCount)
        result += int("".join(curr_code.code[:3])) * sum(
            len(k) * count[k] for k in count
        )

        #     # logger.log(i)
        #     newlist: set[MyCounter[KeyCode]] = set()
        #     for count in mylist:
        #         rets: set[MyCounter[KeyCode]] = {MyCounter()}
        #         for code in count:
        #             rets = {
        #                 MyCounter(a + b * count[code])
        #                 for (a, b) in product(rets, code_output(code))
        #             }
        #         newlist.update(rets)
        #     min_tot = min([count.total() for count in newlist])
        #     mylist = {count for count in newlist if count.total() == min_tot}
        #     # if i == 2:
        #     #     logger.log(
        #     #         prntcounters(mylist)
        #     #     )
        # # logger.log(curr_code.to_string(), sum(len(k) * v for (k, v) in mylist.pop().items()), len(mylist))
        # # result += int("".join(curr_code.code[:3])) * len(mylist[0])

    return result


def solve_p1(useExample: bool = False) -> str:
    result = 0
    result = solve_generic_new(3, useExample)
    # logger.log(result)
    result = solve_generic(3, useExample)
    # logger.log(result)

    return str(result)


def solve_p2(useExample: bool = False) -> str:
    result = 0
    # logger.log(KeyCode(tuple("170A"), KEYPAD_NUM).encode())
    result = solve_generic_new(26, useExample)

    return str(result)


class InputReader:
    def __init__(self, useExample: bool):
        self.data = ""
        inputFile = "example.txt" if useExample else "input.txt"

        with open(inputFile, "r") as f:
            self.data = f.read().rstrip()

    def read(self):
        return self.data

    def lines(self):
        return self.data.split("\n")

    def paragraphs(self):
        return [paragraph.split("\n") for paragraph in self.data.split("\n\n")]

    def ints(self):
        return [
            [int(n) for n in re.findall(r"(?:\b|-)\d+\b", par)] for par in self.lines()
        ]

    def paragraph_ints(self):
        return [
            [[int(n) for n in re.findall(r"(?:\b|-)\d+\b", line)] for line in par]
            for par in self.paragraphs()
        ]

    def words(self, additional_chars="", skip_blank_rows=True) -> list[list[str]]:
        """
        All words made by contiguous alphanum or additional_chars
        """
        return [
            re.findall(r"[a-zA-Z0-9" + additional_chars + r"]+", par)
            for par in self.lines()
            if (par != "" or skip_blank_rows)
        ]


class Timer:
    def __init__(self, name: str = "Timer"):
        now_wall, now_cpu = time.perf_counter(), time.process_time()

        self.name = name
        self.start_wall = now_wall
        self.start_cpu = now_cpu
        self.last_wall = now_wall
        self.last_cpu = now_cpu
        self.lap_number = 0

    # Shamelessly stolen from mebeim (https://github.com/mebeim/aoc/blob/master/utils/timer.py)
    @classmethod
    def seconds_to_most_relevant_unit(cls, s: float):
        s *= 1e6
        if s < 1000:
            return "{:.3f}µs".format(s)

        s /= 1000
        if s < 1000:
            return "{:.3f}ms".format(s)

        s /= 1000
        if s < 60:
            return "{:.3f}s".format(s)

        m = int(s / 60)
        return "{:d}m {:.3f}s".format(m, (s - m * 60))

    def lap(self):
        now_wall, now_cpu = time.perf_counter(), time.process_time()

        dt_wall = Timer.seconds_to_most_relevant_unit(now_wall - self.last_wall)
        dt_cpu = Timer.seconds_to_most_relevant_unit(now_cpu - self.last_cpu)

        self.last_wall = now_wall
        self.last_cpu = now_cpu

        self.lap_number += 1

        return f"{self.name}, lap #{self.lap_number}: {dt_wall} wall, {dt_cpu} CPU"

    def stop(self):
        now_wall, now_cpu = time.perf_counter(), time.process_time()

        dt_wall = Timer.seconds_to_most_relevant_unit(now_wall - self.start_wall)
        dt_cpu = Timer.seconds_to_most_relevant_unit(now_cpu - self.start_cpu)

        return f"{self.name}: {dt_wall} wall, {dt_cpu} CPU"


def main():
    parser = ArgumentParser(prog="AoC")
    parser.add_argument("-p2", "--only_part_2", action="store_true")
    parser.add_argument("-e", "--example", action="store_true")

    args = parser.parse_args()
    only_part_2: bool = args.only_part_2
    useExample: bool = args.example

    for part in [1, 2]:
        if part != 2 and only_part_2:
            continue
        timer = Timer(f"Part {part}")

        result = ""
        if part == 1:
            result = solve_p1(useExample)
        elif part == 2:
            result = solve_p2(useExample)

        time_str = timer.stop()
        print(time_str)

        if not useExample:
            with open(f"output{part}.txt", "w") as outputFile:
                outputFile.write(result)
        print(result)

    logger.close()


if __name__ == "__main__":
    main()
