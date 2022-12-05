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
        'User-Agent': "https://github.com/Uklusi/AdventOfCode/cs-create.py by aurelio.polinto@gmail.com"
    }

    args = sys.argv[1:]
    if len(args) != 2:
        print("The script must be called with two parameters, year and day")
        sys.exit(1)
    
    year = int(args[0])
    day = int(args[1])
    folder_to_create = Path(f"{year}/{day:02d}")
    original_folder = os.getcwd()
    os.makedirs(folder_to_create, exist_ok=True)

    if not os.path.isfile(folder_to_create / "input.txt" ):
        with open(folder_to_create / "input.txt", "w") as f:
            r = requests.get(
                url=f"https://adventofcode.com/{year}/day/{day}/input",
                headers=headers
            )
            if r.ok:
                f.write(r.text.replace("\r", ""))
            else:
                print(f"request failed with error {r.text}")
            pass

    try:
        os.chdir(folder_to_create)
        if not os.path.isfile(f"{day:02d}.csproj"):
            os.system("dotnet new console --no-update-check --no-restore")
            os.remove("Program.cs")
    finally:
        os.chdir(original_folder)

    if not os.path.isfile(folder_to_create / "Main.cs"):
        shutil.copy(Path("templates/templateMain.cs"), folder_to_create / "Main.cs")
        shutil.copy(Path("templates/templateSolver.cs"), folder_to_create / "Solver.cs")
        open(folder_to_create / "example.txt", 'a').close()
        open(folder_to_create / "output1.txt", 'a').close()
        open(folder_to_create / "output2.txt", 'a').close()
        os.symlink(Path("../../libraries/AoCUtils.cs"), folder_to_create / "AoCUtils.cs")

    sys.exit(0)

if __name__ == "__main__":
    main()
    