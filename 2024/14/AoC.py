﻿from collections import Counter
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


logger = Logger("log", write_to_log=True)


def solve_p1(useExample: bool = False) -> str:
    result = 0
    size = Vector(11, 7) if useExample else Vector(101, 103)
    TIME = 100

    input_reader = InputReader(useExample=useExample)  # noqa: F841
    input = input_reader.ints()

    data: list[tuple[Position, Vector]] = []
    quadrants = Counter()
    for line in input:
        px, py, vx, vy = line
        p = Position(px, py)
        v = Vector(vx, vy)
        data.append((p, v))

    # logger.log(data[-2])
    for i in range(TIME):
        newdata: list[tuple[Position, Vector]] = []
        for p, v in data:
            p1 = p + v + size
            p2 = Position(p1.x % size.vx, p1.y % size.vy)
            newdata.append((p2, v))
        data = newdata
        # if i < 10:
        #     logger.log(data[-2])

    for p, _ in data:
        a, b = sign(p.x - size.vx // 2), sign(p.y - size.vy // 2)
        quadrants[a, b] += 1

    # for line in input:
    #     px, py, vx, vy = line
    #     px = (px + vx * TIME) % size.vx
    #     px = (px + size.vx) % size.vx
    #     py = (py + vy * TIME) % size.vy
    #     py = (py + size.vy) % size.vy
    #     a, b = sign(px - size.vx // 2), sign(py - size.vy // 2)
    #     quadrants[a, b] += 1

    # logger.log(data)
    result = quadrants[-1, -1] * quadrants[1, -1] * quadrants[-1, 1] * quadrants[1, 1]

    return str(result)


def solve_p2(useExample: bool = False) -> str:
    result = 0
    size = Vector(11, 7) if useExample else Vector(101, 103)
    TIME = size.vx * size.vy

    input_reader = InputReader(useExample=useExample)  # noqa: F841
    input = input_reader.ints()

    data: list[tuple[Position, Vector]] = []
    for line in input:
        px, py, vx, vy = line
        p = Position(px, py)
        v = Vector(vx, vy)
        data.append((p, v))

    current = {p for p, _ in data}

    frame = Map(
        xmax=size.vx, ymax=size.vy, visual=lambda p: "#" if p in current else empty
    )

    for i in range(TIME):
        newdata: list[tuple[Position, Vector]] = []
        current: set[Position] = set()
        for p, v in data:
            p1 = p + v + size
            p2 = Position(p1.x % size.vx, p1.y % size.vy)
            newdata.append((p2, v))
            current.add(p2)
        data = newdata
        if any(
            [
                all(
                    [
                        q in current and len(p.adjacent(includeCorners=True)) == 8
                        for q in p.adjacent(includeCorners=True)
                    ]
                )
                for p in current
            ]
        ):
            logger.log(f"Step {i + 1}")
            logger.log(frame.image())

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
        return [paragraph.split("\n") for paragraph in self.data.split("\n")]

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
