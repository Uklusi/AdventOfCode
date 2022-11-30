import os
import sys
from pathlib import Path


def main():

    args = sys.argv[1:]
    if len(args) < 3 or len(args) > 4:
        print("The script must be called with three parameters plus one optional.")
        print("Required parameters: year, day and part.")
        print("Optional parameter: --copy")
        sys.exit(1)
    
    year = int(args[0])
    day  = int(args[1])
    part = int(args[2])
    if len(args) == 3:
        switch = None
    else:
        switch = args[3]
    
    original_folder = os.getcwd()
    folder = Path(f"{year}/{day:02d}")

    try:
        os.chdir(folder)
        os.system(f"dotnet run -- {part}")

        if (part == 1 and switch == "--copy"):
            with open("Part2.cs", "r") as f:
                part2data = f.read().strip()

            with open("Part1.cs", "r") as f:
                part1data = f.read()
            part2data = part1data.replace('Part1', 'Part2')
            with open("Part2.cs", "w") as g:
                g.write(part2data)
    
    finally:
        os.chdir(original_folder)

    sys.exit(0)

if __name__ == "__main__":
    main()
    