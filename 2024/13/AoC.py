from AoCUtils import *  # noqa: F401
from argparse import ArgumentParser
import time
from io import TextIOWrapper
import re
from typing import Any

# import pdb


class Logger:
    def __init__(self, log_file_name: str, write_to_log: bool = False):
        self.log_file: TextIOWrapper | None
        if write_to_log:
            self.log_file = open(log_file_name + ".log", "w")
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


def solve_p1(useExample: bool = False) -> str:
    result = 0

    input_reader = InputReader(useExample=useExample)  # noqa: F841
    input = input_reader.paragraph_ints()

    for claw_machine in input:
        a_data = claw_machine[0]
        b_data = claw_machine[1]
        prize_data = claw_machine[2]
        a = Vector(a_data[0], a_data[1])
        b = Vector(b_data[0], b_data[1])
        prize = Vector(prize_data[0], prize_data[1])
        na = 0
        nb = 0

        winning = inf
        for na, nb in product(range(100), range(100)):
            if na * a + nb * b == prize:
                new = 3 * na + nb
                if new < winning:
                    winning = new
        if winning != inf:
            result += winning

    logger.close()
    return str(result)


def solve_p2(useExample: bool = False) -> str:
    result = 0
    STEPS_ADDED = 10000000000000

    input_reader = InputReader(useExample=useExample)  # noqa: F841
    input = input_reader.paragraph_ints()

    def solve_division(dividend: int, divisor: int, m: int):
        d = gcd(divisor, m)
        if dividend % d != 0:
            return None
        dividend1 = dividend // d
        divisor1 = divisor // d
        m1 = m // d
        inverse = pow(divisor1, -1, m1)
        return (inverse * dividend1) % m1

    for claw_machine in input:
        a_data = claw_machine[0]
        b_data = claw_machine[1]
        prize_data = claw_machine[2]
        a = Vector(a_data[0], a_data[1])
        b = Vector(b_data[0], b_data[1])
        prize = Vector(prize_data[0] + STEPS_ADDED, prize_data[1] + STEPS_ADDED)
        na = 0
        nb = min(prize.vx // b.vx, prize.vy // b.vy)
        rem = prize - nb * b
        # pdb.set_trace()
        if rem.vx != 0:
            k = solve_division(rem.vx, a.vx, b.vx)
            if k is None:
                continue
            na += k
            rem = prize - na * a - nb * b
            nb += rem.vx // b.vx
            rem = prize - na * a - nb * b
        # pdb.set_trace()
        if rem.vy != 0:
            d = gcd(a.vx, b.vx)
            nb1 = a.vx // d
            na1 = b.vx // d
            changey = a.vy * na1 - b.vy * nb1
            if rem.vy % changey != 0:
                continue
            d1 = rem.vy // changey
            nb -= nb1 * d1
            na += na1 * d1
        # pdb.set_trace()
        result += 3 * na + nb

    logger.close()
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


if __name__ == "__main__":
    main()
