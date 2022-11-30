import os
import sys
from pathlib import Path
import requests
import shutil


def main():
    
    with open("session_cookie.txt", "r") as f:
        session_cookie = f.read().strip()

    headers = {
        'cookie': f'session={session_cookie}',
        'User-Agent': "https://github.com/Uklusi/AdventOfCode/py-create.py by aurelio.polinto@gmail.com"
    }

    args = sys.argv[1:]
    if len(args) != 2:
        print("The script must be called with two parameters, year and day")
        sys.exit(1)
    
    year = int(args[0])
    day = int(args[1])
    folderToCreate = Path(f"{year}/{day:02d}")
    os.makedirs(folderToCreate, exist_ok=True)

    if not os.path.isfile(folderToCreate / "input.txt" ):
        with open(folderToCreate / "input.txt", "w") as f:
            r = requests.get(
                url=f"https://adventofcode.com/{year}/day/{day}/input",
                headers=headers
            )
            if r.ok:
                f.write(r.text.replace("\r", ""))
            else:
                print(f"request failed with error {r.text}")

    if not os.path.isfile(folderToCreate / "part1.py"):
        shutil.copy(Path("templates/template.py"), folderToCreate / "part1.py")
        open(folderToCreate / "part2.py", 'a').close()
        open(folderToCreate / "example.txt", 'a').close()
        open(folderToCreate / "output1.txt", 'a').close()
        open(folderToCreate / "output2.txt", 'a').close()
        os.symlink(Path("../../libraries/AoCUtils.py"), folderToCreate / "AoCUtils.py")

    sys.exit(0)

if __name__ == "__main__":
    main()
    