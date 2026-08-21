"""
Usage: python3 file.py FILE OUTPATH [NAME]
Generates a .cs enum file based on the given JSON file, based on the keys of the root object.
"""

from json import load
from sys import argv

SUCCESS = 0
FAIL = 1

if len(argv) != 3 and len(argv) != 4:
    print(f"Usage: python3 {argv[0]} FILE OUTPATH [NAME]")
    print("Generates a .cs enum file based on the given JSON file, based on the keys of the root object.")
    exit(FAIL)

path = argv[1]
out_path = argv[2]
name = argv[3] if len(argv) > 3 else path.split("/")[-1].split(".")[0].title()

try:
    with open(path) as r:
        json = load(r)
except:
    print(f"Error reading JSON file {path}")
    exit(FAIL)

if not type(json) is dict:
    print(f"Unsupported root element: {type(json)}")
    exit(FAIL)

with open(out_path, "w+") as w:
    w.write("namespace Gamespace;\n\n")
    w.write("public enum " + name + " {\n")
    for item in sorted(json.keys(), key=len):
        w.write(f"    {item},\n")
    w.write("}\n")
