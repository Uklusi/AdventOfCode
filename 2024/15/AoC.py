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


def calc_move(p: Position, frame: Frame, dir: Vector) -> tuple[bool, list[Position]]:
    q = p + dir
    if frame[q] == "#":
        return (False, [])
    elif frame[q] == ".":
        return (True, [p])
    else:
        t = calc_move(q, frame, dir)
        return (t[0], t[1] + [p])


def solve_p1(useExample: bool = False) -> str:
    result = 0

    input_reader = InputReader(useExample=useExample)  # noqa: F841
    input = input_reader.paragraphs()

    frame = Frame(input[0])
    robot = [p for p in frame.get_iterator() if frame[p] == "@"][0]

    moves = "".join(input[1])

    for d in moves:
        dir = VectorDir(d)
        can_move, moved = calc_move(robot, frame, dir)
        if can_move:
            for p in moved:
                frame[p + dir] = frame[p]
            frame[robot] = "."
            robot = robot + dir

    for p in frame.get_iterator():
        if frame[p] == "O":
            result += 100 * p.y + p.x

    return str(result)


def calc_move_p2(
    starts: set[Position], frame: Frame, dir: Vector
) -> tuple[bool, list[set[Position]]]:
    left = VectorDir("L")
    right = VectorDir("R")

    if dir in (left, right):
        q = list(starts)[0] + dir
        if frame[q] == "#":
            return (False, [])
        elif frame[q] == ".":
            return (True, [starts])
        t = calc_move_p2({q}, frame, dir)
        return (t[0], t[1] + [starts])

    nexts_naive = {p + dir for p in starts}
    if any([frame[q] == "#" for q in nexts_naive]):
        return (False, [])
    elif all([frame[q] == "." for q in nexts_naive]):
        return (True, [starts])
    nexts = copy(nexts_naive)
    for q in nexts_naive:
        if frame[q] == "[":
            other = q + right
            nexts.add(other)
        elif frame[q] == "]":
            other = q + left
            nexts.add(other)
        elif frame[q] == '.':
            nexts.discard(q)
        else:
            raise ValueError(f"Unknown object {frame[q]} in target at {q}")

    t = calc_move_p2(nexts, frame, dir)
    return (t[0], t[1] + [starts])


def solve_p2(useExample: bool = False) -> str:
    result = 0

    input_reader = InputReader(useExample=useExample)  # noqa: F841
    input = input_reader.paragraphs()
    strFrame = input[0]
    for i in range(len(strFrame)):
        strFrame[i] = (
            strFrame[i]
            .replace("#", "##")
            .replace(".", "..")
            .replace("@", "@.")
            .replace("O", "[]")
        )

    frame = Frame(strFrame)
    robot = [p for p in frame.get_iterator() if frame[p] == "@"][0]

    moves = "".join(input[1])

    for d in moves:
        dir = VectorDir(d)
        can_move, moved = calc_move_p2({robot}, frame, dir)
        if can_move:
            for s in moved:
                for p in s:
                    frame[p + dir] = frame[p]
                    frame[p] = '.'
            robot = robot + dir

    for p in frame.get_iterator():
        if frame[p] == "[":
            result += 100 * p.y + p.x

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
