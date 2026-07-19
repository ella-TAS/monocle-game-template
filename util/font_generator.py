"""
Usage: python3 file.py FILE TILE_WIDTH TILE_HEIGHT SPACING [CHARACTERS]
Generates a .fnt file for the input image.
Character widths are detected based on pixels.
"""

import cv2
from sys import argv
from pathlib import Path

SUCCESS = 0
FAIL = 1

default_chars = " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~"

if len(argv) != 6 and len(argv) != 5:
    print(f"Usage: python3 {argv[0]} FILE TILE_WIDTH TILE_HEIGHT SPACING [CHARACTERS]")
    print(f"Default CHARACTERS: {default_chars}")
    print("Creates a .fnt file for an existing tiled font image. Character widths are detected based on pixels.")
    exit(FAIL)

try:
    img = cv2.imread(argv[1], cv2.IMREAD_UNCHANGED)
    img_height, img_width, _ = img.shape
    path = Path(argv[1])
    out_path = argv[1][:argv[1].rfind(".")] + ".fnt"
except Exception:
    print(f"Error reading image {argv[1]}")
    exit(FAIL)

try:
    tile_width = int(argv[2])
    tile_height = int(argv[3])
    spacing = int(argv[4])
except Exception:
    print("Error: tile width/height and spacing must be numbers")
    exit(FAIL)

chars = argv[5] if len(argv) > 5 else default_chars

row = 0
col = 0
out = []
for c in chars:
    if row + tile_height > img_height:
        print(f"Error: not enough ({len(chars)}) tiles ({tile_width}x{tile_height}) in the image ({img_width}x{img_height})")
        exit(FAIL)

    # read one tile and find the rightmost non-transparent pixel
    rightmost = col
    for y in range(row, row + tile_height):
        for x in range(col, col + tile_width):
            if img[y][x][3] > 0:
                rightmost = max(rightmost, x)
    width = rightmost - col + 1

    out.append([
        ord(c),
        col,
        row,
        width,
        tile_height,
        0,
        0,
        width + spacing * (2 if c == ' ' else 1),
        0,
        15
    ])

    # move on to the next tile
    col += tile_width
    if col + tile_width > img_width:
        row += tile_height
        col = 0

out.sort(key=lambda o: o[0])

with open(out_path, "w+") as w:
    w.write('<?xml version="1.0"?>\n')
    w.write('<!-- The format of this file is documented at https://angelcode.com/products/bmfont/doc/file_format.html -->\n')
    w.write('<font>\n')
    w.write(f'    <info face="{path.name.split(".")[0].title()}" size="{tile_height}" bold="0" italic="0" charset="" unicode="" stretchH="100" smooth="0" aa="1" padding="2,2,2,2" spacing="0,0" outline="0" />\n')
    w.write(f'    <common lineHeight="{tile_height}" base="{int(tile_height * 0.8)}" scaleW="{img_width}" scaleH="{img_height}" pages="1" packed="0" />\n')
    w.write('    <pages>\n')
    w.write(f'        <page id="0" file="{path.name}" />\n')
    w.write('    </pages>\n')
    w.write(f'    <chars count="{len(out)}">\n')
    for o in out:
        w.write(f'        <char id="{o[0]}" x="{o[1]}" y="{o[2]}" width="{o[3]}" height="{o[4]}" xoffset="{o[5]}" yoffset="{o[6]}" xadvance="{o[7]}" page="{o[8]}" chnl="{o[9]}" />\n')
    w.write('    </chars>\n')
    w.write('    <kernings count="1">\n')
    w.write('        <kerning first="32" second="32" amount="-1" />\n')
    w.write('    </kernings>\n')
    w.write('</font>')
