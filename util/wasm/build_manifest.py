"""
Usage: python3 file.py DIR
Builds a manifest.json containing all content folders and files
"""

from sys import argv
from os import walk
from json import dumps

SUCCESS = 0
FAIL = 1

if len(argv) != 2:
    print(f"Usage: python3 {argv[0]} DIR")
    exit(FAIL)
if not argv[1].endswith("/"):
    argv[1] += "/"

out = []
for dir, _, files in walk(argv[1]):
    dir = dir[len(argv[1]):]
    if len(dir) > 0:
        dir += "/"
        out.append(dir)
    for f in files:
        out.append(dir + f)

with open(argv[1] + "manifest.json", "w+") as w:
    w.write(dumps(out))
