from AoCUtils import *
import sys
import time
from io import TextIOWrapper
import re
from typing import Any
from itertools import combinations
from sympy import symbols, linsolve, Eq


def solve_p1(useExample: bool = False) -> str:
    logger = Logger("part1", write_to_log=False)
    result = 0

    input_reader = InputReader(useExample=useExample)  # noqa: F841

    if useExample:
        bound = (7, 27)
    else:
        bound = (200000000000000, 400000000000000)

    ints = input_reader.read_ints()
    hail_info = [
        (Position(a, b, reverseY=False), Vector(d, e, reverseY=False).direction())
        for (a, b, _, d, e, _) in ints
    ]

    def solve_system(
        p: Position, u: Vector, q: Position, v: Vector
    ) -> tuple[float, float, Position]:
        """
        a * u - b * v == q - p
        """
        if u == v or u == -v:
            return None
        d = q - p
        under = u.vx * v.vy - u.vy * v.vx
        a = (d.vx * v.vy - d.vy * v.vx) / under
        b = (d.vx * u.vy - d.vy * u.vx) / under
        return (a, b, p + a * u)

    def is_in_bounds(p: Position, u: Vector, q: Position, v: Vector):
        t = solve_system(p, u, q, v)
        if t is None:
            return false
        return (
            t[0] > 0
            and t[1] > 0
            and (bound[0] <= t[2].x <= bound[1])
            and (bound[0] <= t[2].y <= bound[1])
        )

    for (p, u), (q, v) in combinations(hail_info, 2):
        # logger.write_line(solve_system(p, u, q, v))
        if is_in_bounds(p, u, q, v):
            result += 1
    # logger.write_line(
    #     solve_system(hail_info[0][0], hail_info[0][1], hail_info[1][0], hail_info[1][1])
    # )

    # logger.write_line(hail_info[0])

    logger.close()
    return str(result)


def solve_p2(useExample: bool = False) -> str:
    """
    since p + v*t1 = p1 + v1*t1, p - p1 = -t*(v - v1)
    this means that (p - p1) // (v - v1), or (p - p1)×(v - v1) = 0
    Opening with bilinearity gives p1 × v + p × v1 - p1 × v1 = - p × v
    rhs is the same for each i, this gives the equation used to solve (since this is linear in p and v)
    """
    logger = Logger("part2", write_to_log=False)
    result = 0

    input_reader = InputReader(useExample=useExample)  # noqa: F841

    ints = input_reader.read_ints()
    hail_info = [
        (PositionNDim(a, b, c), PositionNDim(d, e, f)) for (a, b, c, d, e, f) in ints
    ]

    [(p1, v1), (p2, v2), (p3, v3)] = hail_info[:3]

    px, py, pz, vx, vy, vz = symbols("px py pz vx vy vz")
    p = PositionNDim((px, py, pz))
    v = PositionNDim((vx, vy, vz))

    a = p1 @ v + p @ v1 - p1 @ v1
    b = p2 @ v + p @ v2 - p2 @ v2
    c = p3 @ v + p @ v3 - p3 @ v3

    system_solution = linsolve(
        [Eq(a1, b1) for (a1, b1) in zip(a.coordinates, b.coordinates)]
        + [Eq(a1, c1) for (a1, c1) in zip(a.coordinates, c.coordinates)],
        (*p.coordinates, *v.coordinates),
    )

    solution = list(system_solution)[0]

    result = solution[0] + solution[1] + solution[2]

    logger.close()
    return str(result)


class Logger:
    def __init__(self, log_file_name: str, write_to_log: bool = False):
        self.log_file: TextIOWrapper | None
        if write_to_log:
            self.log_file = open(log_file_name + ".log", "w")
        else:
            self.log_file = None

    def write_line(self, *t: Any):
        if self.log_file is None:
            print(*t)
        else:
            self.log_file.write(" ".join([str(o) for o in t]) + "\n")
            self.log_file.flush()

    def write(self, *t: Any):
        if self.log_file is None:
            print(*t, end="")
        else:
            self.log_file.write(" ".join([str(o) for o in t]))
            self.log_file.flush()

    def close(self):
        if self.log_file is not None:
            self.log_file.close()


class InputReader:
    def __init__(self, useExample: bool):
        self.data = ""
        inputFile = "example.txt" if useExample else "input.txt"

        with open(inputFile, "r") as f:
            self.data = f.read().rstrip()

    def read(self):
        return self.data

    def read_lines(self):
        return self.data.split("\n")

    def read_double_lines(self):
        return [paragraph.split("\n") for paragraph in self.data.split("\n")]

    def read_ints(self):
        return [
            [int(n) for n in re.findall(r"(?:\b|-)\d+\b", par)]
            for par in self.read_lines()
        ]

    def read_tokens(self):
        return [par.split(" ") for par in self.read_lines()]

    def read_words(self, additional_chars="", skip_blank_rows=True) -> list[str]:
        return [
            re.findall(r"[a-zA-Z0-9" + additional_chars + r"]+", par)
            for par in self.read_lines()
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
    args = sys.argv[1:]

    if not (1 <= len(args) <= 2):
        print("Please pass the part you want to solve to the program,")
        print("And optionally the --example flag")
        sys.exit(1)

    part = int(args[0])
    if len(args) == 2 and args[1] == "--example":
        useExample = True
    else:
        useExample = False

    timer = Timer(f"Part {part}")

    result = ""
    if part == 1:
        result = solve_p1(useExample)
    elif part == 2:
        result = solve_p2(useExample)
    else:
        print("Wrong part specified, exiting")
        sys.exit(1)

    time_str = timer.stop()
    print(time_str)

    with open(f"output{part}.txt", "w") as outputFile:
        outputFile.write(result)
    print(result)


if __name__ == "__main__":
    main()
