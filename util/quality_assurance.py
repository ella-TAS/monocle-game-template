"""
Usage: python3 file.py [FILE]
Checks the input file argv[1] for the following criteria:
- There is exactly one namespace definition
- The namespace matches the folder structure
- The first class definition matches the file name
"""

from sys import argv
import re
from pathlib import Path

SUCCESS = 0
FAIL = 1

def error(msg, doExit=True):
    print(f"\nError in file {argv[1]}")
    print(msg)
    if doExit: exit(FAIL)

if len(argv) != 2:
    print(f"Usage: python3 {argv[0]} [FILE]")
    exit(FAIL)

try:
    with open(argv[1], "r", encoding="utf-8-sig") as file:
        content = file.read().splitlines()
except Exception:
    print(f"File {argv[1]} not found")
    exit(FAIL)

path = Path(argv[1])
filename = path.name.split(".")[0]

namespaces = []
classname = None
for line in content:
    match = re.match(r"^namespace (.+);$", line)
    if match:
        namespaces.append(match)

    match = re.match(r"^.*(?:class|enum|interface) (.+?) *(?::.+)?\{$", line)
    if match and not classname:
        classname = match.groups()[0]

if not classname:
    error("No class/enum/interface definition found")
if classname != filename:
    error(f"First class name {classname} and file name {filename} don't match")

if classname == "Program":
    # Program file may be in no namespace
    exit(SUCCESS)

if len(namespaces) == 0:
    error("No namespace definition found")
if len(namespaces) > 1:
    error(f"Too many namespace definitions found ({len(namespaces)})", False)
    print(*(match.group() for match in namespaces), sep="\n")
    exit(FAIL)

namespace = namespaces[0].groups()[0]
name = namespace.split(".")[1:]
name = "/".join(name)
if name: name = "/" + name

if "/" + str(path.parent) != "/Game" + name:
    error(f"Namespace {namespace} doesn't match folder structure {path.parent}")

exit(SUCCESS)
