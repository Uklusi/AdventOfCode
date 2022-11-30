import os
import sys
from pathlib import Path


def main():

    args = sys.argv[1:]
    if len(args) < 3 or len(args) > 4:
        print("The script must be called with three parameters plus one optional.")
        print("Required parameters: year, day and part.")
        print("Optional parameter: --copy or --no-copy")
        sys.exit(1)
    
    year = int(args[0])
    day = int(args[1])
    part = int(args[2])
    if len(args) == 3:
        switch = None
    else:
        switch = args[3]
    
    original_folder = os.getcwd()
    folder = Path(f"{year}/{day:02d}")

    try:
        os.chdir(folder)
        os.system(f"python part{part}.py")
        if (part == 1 and switch != "--no-copy"):
            with open("part2.py", "r") as f:
                part2data = f.read().strip()
            if (part2data == "" or switch == "--copy"):
                with open("part1.py", "r") as f:
                    part1data = f.read()
                part2data = part1data.replace('partNumber = "1"', 'partNumber = "2"')
                with open("part2.py", "w") as g:
                    g.write(part2data)
    finally:
        os.chdir(original_folder)

    sys.exit(0)

if __name__ == "__main__":
    main()
    