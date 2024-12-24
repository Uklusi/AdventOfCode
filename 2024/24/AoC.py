from AoCUtils import *  # noqa: F401
from argparse import ArgumentParser
from typing import Any  # noqa: F401


logger = Logger("log", write_to_log=False)


@dataclass
class Wire:
    label: str
    gates: dict[str, "Gate"]
    _value: int | None = None
    _orig_label: str = ""

    def __post_init__(self):
        self._orig_label = self.label

    @property
    def value(self):
        if self._value is None:
            self.gates[self.label].calculate()
        if self._value is None:
            raise ValueError()
        return self._value


@dataclass
class Gate:
    type: str
    input1: Wire
    input2: Wire
    output: Wire

    def calculate(self):
        if self.type == "AND":
            self.output._value = self.input1.value & self.input2.value
        elif self.type == "OR":
            self.output._value = self.input1.value | self.input2.value
        elif self.type == "XOR":
            self.output._value = self.input1.value ^ self.input2.value
        else:
            raise ValueError()

        return
        # return self.output


def solve_p1(useExample: bool = False) -> str:
    result = 0

    input_reader = InputReader(useExample=useExample)  # noqa: F841
    data = input_reader.paragraph_words()
    known_wires = data[0]
    gates_as_str = data[1]

    gates: dict[str, Gate] = {}
    wires: dict[str, Wire] = {}
    for line in known_wires:
        (label, value) = line[0], int(line[1])
        wires[label] = Wire(label, gates, value)

    for i1, g, i2, o in gates_as_str:
        if i1 not in wires:
            wires[i1] = Wire(i1, gates)
        if i2 not in wires:
            wires[i2] = Wire(i2, gates)
        if o not in wires:
            wires[o] = Wire(o, gates)
        gate = Gate(g, wires[i1], wires[i2], wires[o])
        gates[o] = gate

    ret_wires = [label for label in wires if label[0] == "z"]
    ret_wires.sort(reverse=True)
    for label in ret_wires:
        result = 2 * result + wires[label].value
    return str(result)


def solve_p2(useExample: bool = False) -> str:
    result = 0

    input_reader = InputReader(useExample=useExample)  # noqa: F841
    data = input_reader.paragraph_words()
    known_wires = data[0]
    gates_as_str = data[1]

    gates_dict: dict[str, Gate] = {}
    gates: list[Gate] = []
    wires: dict[str, Wire] = {}
    for line in known_wires:
        (label, value) = line[0], int(line[1])
        wires[label] = Wire(label, gates_dict, value)

    for i1, g, i2, o in gates_as_str:
        if i1 not in wires:
            wires[i1] = Wire(i1, gates_dict)
        if i2 not in wires:
            wires[i2] = Wire(i2, gates_dict)
        if o not in wires:
            wires[o] = Wire(o, gates_dict)
        gate = Gate(g, wires[i1], wires[i2], wires[o])
        gates_dict[o] = gate
        gates.append(gate)

    for i in range(100):
        for gate in gates:
            if {gate.input1.label, gate.input2.label} == {f"x{i:02d}", f"y{i:02d}"}:
                if i == 0 and gate.type == "XOR":
                    pass
                elif i == 0 and gate.type == "AND":
                    gate.output.label = f"p{i:02d}"
                elif gate.type == "AND":
                    gate.output.label = f"b{i:02d}"
                elif gate.type == "XOR":
                    gate.output.label = f"a{i:02d}"
        for gate in gates:
            if {gate.input1.label, gate.input2.label} == {
                f"a{i:02d}",
                f"p{i-1:02d}",
            } and gate.type == "AND":
                gate.output.label = f"c{i:02d}"
        for gate in gates:
            if {gate.input1.label, gate.input2.label} == {
                f"b{i:02d}",
                f"c{i:02d}",
            } and gate.type == "OR":
                gate.output.label = f"p{i:02d}"

    logged = []
    logged_output = []
    for gate in gates:
        a = f"{gate.input1.label} ({gate.input1._orig_label})"
        b = gate.type
        c = f"{gate.input2.label} ({gate.input2._orig_label})"
        d = f"{gate.output.label} ({gate.output._orig_label})"
        if a > c:
            (a, c) = (c, a)
        logged.append(f"{a} {b} {c} -> {d}")
        logged_output.append(f"{d} <- {a} {b} {c}")
    logged.sort()
    logged_output.sort()

    # logger.log(prettify(logged))
    # logger.log()
    # logger.log(prettify(logged_output))

    # graph = nx.DiGraph()

    # for (wire, _) in known_wires:
    #     graph.add_node(wire)
    # for (i, (a, b, c, d)) in enumerate(gates_as_str):
    #     b = f"GATE_{i:02d}_{b}"
    #     graph.add_nodes_from([a, b, c, d])
    #     graph.add_edges_from([
    #         (a, b), (c, b), (b, d)
    #     ])

    result_list = ["qnw", "z15", "cqr", "z20", "ncd", "nfj", "vkg", "z37"]
    # risolto AMMANO
    result = ",".join(sorted(result_list))

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
