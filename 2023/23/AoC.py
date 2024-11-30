from AoCUtils import *
import sys
import time
from io import TextIOWrapper
from typing import Any


def viewDistances(path_start: MapPosition, frame: Frame, is_part_2=False):
    targets: dict[MapPosition, int] = {}
    for current in path_start.adjacent():
        if (
            path_start.y == 0
            or dirToArrow((current - path_start).directionIndicator()) == frame[current]
            or is_part_2
        ):
            pass
        else:
            continue
        previous = path_start
        steps = 1
        # import pdb; pdb.set_trace()
        try:
            while True:
                pr = previous
                for pos in current.adjacent():
                    if pos == pr:
                        continue
                    previous = current
                    current = pos
                    steps += 1
                    if len(current.adjacent()) > 2 or current.y == frame.y - 1:
                        # import pdb; pdb.set_trace()
                        targets[current] = steps
                        raise BreakLoop()
                    elif current.y == 0:
                        raise BreakLoop()
        except BreakLoop:
            pass

    return targets


def solve_p1(useExample: bool = False) -> str:
    logger = Logger("part1", write_to_log=False)
    result = 0

    input_reader = InputReader(useExample=useExample)  # noqa: F841
    strFrame = input_reader.read_lines()
    frame = Frame(strFrame)

    def convertMap(c):
        if c == ".":
            return empty
        elif c == "#":
            return full
        else:
            return "↑→↓←"[dirToNum(c)]

    m = Map(frame=strFrame, visual=lambda p: convertMap(frame[p]))  # noqa: F841

    junctions: list[MapPosition] = []

    for y in range(len(strFrame)):
        for x in range(len(strFrame[0])):
            p = MapPosition(x, y, frame=strFrame, occupied=lambda p: frame[p] == "#")

            if p.isEmpty():
                if p.y == 0:
                    start = p
                elif p.y == frame.y - 1:
                    end = p
                elif len(p.adjacent()) > 2:
                    assert all([frame[q] not in {"#", "."} for q in p.adjacent()])
                    junctions.append(p)

            if p.isEmpty() and len(p.adjacent()) > 2:
                assert all([frame[q] not in {"#", "."} for q in p.adjacent()])

    distances: dict[MapPosition, dict[MapPosition, int]] = {}
    distances[start] = viewDistances(start, frame)
    for p in junctions:
        distances[p] = viewDistances(p, frame)

    # logger.write_line(prettifyDict(distances))
    # logger.write_line(m.image())

    @cache
    def traverse_map(start: Position, end: Position):
        if start == end:
            return 0
        return max(
            [distances[start][p] + traverse_map(p, end) for p in distances[start]]
        )

    result = traverse_map(start, end)

    logger.close()
    return str(result)


def solve_p2(useExample: bool = False) -> str:

    logger = Logger("part2")
    result = 0

    input_reader = InputReader(useExample=useExample)  # noqa: F841
    strFrame = input_reader.read_lines()
    frame = Frame(strFrame)

    def convertMap(c):
        if c == ".":
            return empty
        elif c == "#":
            return full
        else:
            return "↑→↓←"[dirToNum(c)]

    m = Map(frame=strFrame, visual=lambda p: convertMap(frame[p]))  # noqa: F841

    junctions: list[MapPosition] = []

    for y in range(len(strFrame)):
        for x in range(len(strFrame[0])):
            p = MapPosition(x, y, frame=strFrame, occupied=lambda p: frame[p] == "#")

            if p.isEmpty():
                if p.y == 0:
                    start = p
                elif p.y == frame.y - 1:
                    end = p
                elif len(p.adjacent()) > 2:
                    assert all([frame[q] not in {"#", "."} for q in p.adjacent()])
                    junctions.append(p)

            if p.isEmpty() and len(p.adjacent()) > 2:
                assert all([frame[q] not in {"#", "."} for q in p.adjacent()])

    distances: dict[MapPosition, dict[MapPosition, int]] = {}
    distances[start] = viewDistances(start, frame, is_part_2=True)
    for p in junctions:
        distances[p] = viewDistances(p, frame, is_part_2=True)

    # logger.write_line(prettifyDict(distances))
    # logger.write_line(m.image())

    @cache
    def traverse_map(start: Position, end: Position, visited: frozenset):
        if start == end:
            return 0
        visited |= {start}
        return max(
            [
                distances[start][p] + traverse_map(p, end, visited)
                for p in distances[start]
                if (
                    p not in visited
                    and not set(distances.get(p, {p})).issubset(visited)
                )
            ],
            default=-inf,
        )

    result = traverse_map(start, end, frozenset())

    logger.close()
    return str(result)


class Logger:
    def __init__(self, log_file_name: str, write_to_log: bool = False):
        self.log_file: TextIOWrapper | None
        if write_to_log:
            self.log_file = open(log_file_name + ".log", "w", encoding="utf8")
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
