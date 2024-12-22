from collections import Counter
from dataclasses import field
from AoCUtils import *  # noqa: F401
from argparse import ArgumentParser
from typing import Any  # noqa: F401

logger = Logger("log", write_to_log=False)


@dataclass
class Monkey:
    secret: int
    prices: list[int] = field(default_factory=list, init=false)

    def mix(self, value: int):
        self.secret = self.secret ^ value
        return self

    def prune(self):
        self.secret = self.secret % 16777216
        return self

    def step(self):
        value = self.secret << 6
        self.mix(value)
        self.prune()

        value = self.secret >> 5
        self.mix(value)
        self.prune()

        value = self.secret << 11
        self.mix(value)
        self.prune()
        return self

    def gen_prices(self):
        self.prices.append(self.secret % 10)
        for _ in range(2000):
            self.step()
            self.prices.append(self.secret % 10)
        return self


def solve_p1(useExample: bool = False) -> str:
    result = 0

    input_reader = InputReader(useExample=useExample)  # noqa: F841
    secrets = [int(line) for line in input_reader.lines()]

    for secret in secrets:
        monkey = Monkey(secret)
        for _ in range(2000):
            monkey.step()
        result += monkey.secret

    return str(result)


def solve_p2(useExample: bool = False) -> str:
    result = 0

    input_reader = InputReader(useExample=useExample)  # noqa: F841
    secrets = [int(line) for line in input_reader.lines()]

    price_changes: list[dict[str, int]] = []
    for secret in secrets:
        monkey = Monkey(secret)
        monkey.gen_prices()

        price_change: dict[str, int] = {}
        prices = monkey.prices
        for a, b, c, d, e in zip(
            prices, prices[1:], prices[2:], prices[3:], prices[4:]
        ):
            (x, y, z, w) = (b - a, c - b, d - c, e - d)
            key = f"{w},{z},{y},{x}"
            if key not in price_change:
                price_change[key] = e
        price_changes.append(price_change)

    possible_seqs: Counter[str] = Counter()
    for key in set(flatten([c.keys() for c in price_changes])):
        for price_change in price_changes:
            possible_seqs[key] += price_change.get(key, 0)

    result = max(possible_seqs.values())

    return str(result)


def run_debug(useExample: bool = False) -> None:
    input_reader = InputReader(useExample=useExample)  # noqa: F841
    s = Monkey(123)
    logger.log(s.step().secret)

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
