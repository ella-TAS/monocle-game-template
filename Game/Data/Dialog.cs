using Microsoft.Xna.Framework;
using Monocle;
using System.Text.RegularExpressions;

namespace Gamespace;

public static partial class Dialog {
    [GeneratedRegex("^\\[(\\w+)\\]([^\\[]*)$", RegexOptions.Multiline)]
    private static partial Regex DialogRegex();

    private const string FILE = "Dialog/English.txt";

    private static readonly Dictionary<string, string> Data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

    public static void Load() {
        Stream stream = TitleContainer.OpenStream(Path.Combine(Engine.ContentDirectory, FILE));
        StreamReader reader = new StreamReader(stream);
        string fullTxt = reader.ReadToEnd();

        MatchCollection matches = DialogRegex().Matches(fullTxt);
        foreach (Match match in matches) {
            if (!Data.TryAdd(match.Groups[1].Value, match.Groups[2].Value.Trim())) {
                Logger.DebugCrash("Dialog", $"Duplicate dialog key [{match.Groups[0].Value}] in {FILE}");
            }
        }
    }

    public static string Get(string key) {
        if (!Data.TryGetValue(key, out string value)) {
            Logger.DebugCrash("Dialog", $"Missing dialog key [{key}] in {FILE}");
            return "";
        }
        return value;
    }

    public static string GetLine(string key, int line = 0) {
        string[] lines = GetLines(key);
        if (line >= lines.Length) {
            Logger.DebugCrash("Dialog", $"Too few lines in dialog key [{key}] in {FILE}: {lines.Length} present, {line} requested");
            return "";
        }
        return lines[line];
    }

    public static string[] GetLines(string key) {
        return Get(key).Split("\n");
    }
}
