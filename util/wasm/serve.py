#!/usr/bin/env python3
from http.server import HTTPServer, SimpleHTTPRequestHandler, test
from sys import argv

if len(argv) != 2:
    print(f"Usage: python3 {argv[0]} [PATH]")
    exit(1)

class CORSRequestHandler (SimpleHTTPRequestHandler):
    extensions_map = {
        **SimpleHTTPRequestHandler.extensions_map,
        ".mjs": "application/javascript",
        ".wasm": "application/wasm",
    }

    def translate_path(self, path):
            print(argv[1] + path)
            return argv[1] + path

    def end_headers(self):
        self.send_header("Access-Control-Allow-Origin", "*")
        self.send_header("Cross-Origin-Embedder-Policy", "require-corp")
        self.send_header("Cross-Origin-Opener-Policy", "same-origin")
        SimpleHTTPRequestHandler.end_headers(self)

if __name__ == "__main__":
    test(CORSRequestHandler, HTTPServer, port=5000)
