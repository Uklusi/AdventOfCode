﻿from AoCUtils import *  # noqa: F401
from argparse import ArgumentParser
from typing import Any  # noqa: F401


logger = Logger("log", write_to_log=False)


def solve_p1(useExample: bool = False) -> str:
    result = 0

    input_reader = InputReader(useExample=useExample)  # noqa: F841

    return str(result)


def solve_p2(useExample: bool = False) -> str:
    result = 0

    input_reader = InputReader(useExample=useExample)  # noqa: F841

    return str(result)


def run_debug(useExample: bool = False) -> None:
    input_reader = InputReader(useExample=useExample)  # noqa: F841

    return


def main():
    parser = ArgumentParser(prog="AoC")
    parser.add_argument("-p2", "--only_part_2", action="store_true")
    parser.add_argument("-e", "--example", action="store_true")
    parser.add_argument("-d", "--debug", action="store_true")

    args = parser.parse_args()
    only_part_2: bool = args.only_part_2
    useExample: bool = args.example
    debug: bool = args.debug

    if debug:
        run_debug(useExample)

        logger.close()

        return

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
