using Microsoft.Xna.Framework;
using Monocle;
using System.Text.RegularExpressions;

namespace Gamespace;

public static partial class Dialog {
    public enum Languages {
        English
    };

    [GeneratedRegex("^\\[(\\w+)\\]([^\\[]*)$", RegexOptions.Multiline)]
    private static partial Regex DialogRegex();

    private const string FILE = "Dialog/English.txt";

    // dialog modifiers
    private const string MOD_NEWLINE = "{n}";

    private static Languages Language;
    private static readonly Dictionary<string, string> Data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

    public static void Load(Languages lang) {
        Data.Clear();
        Language = lang;

        using Stream stream = TitleContainer.OpenStream(Path.Combine(Engine.ContentDirectory, "Dialog", Language + ".txt"));
        using StreamReader reader = new StreamReader(stream);
        string fullTxt = reader.ReadToEnd();

        MatchCollection matches = DialogRegex().Matches(fullTxt);
        foreach (Match match in matches) {
            if (!Data.TryAdd(
                match.Groups[1].Value,
                match.Groups[2].Value.Trim().ReplaceLineEndings("\n")
            )) {
                Logger.DebugCrash("Dialog", $"Duplicate dialog key [{match.Groups[1].Value}] in {Language}");
            }
        }
    }

    public static string Get(string key) {
        if (!Data.TryGetValue(key, out string value)) {
            Logger.DebugCrash("Dialog", $"Missing dialog key [{key}] in {Language}");
            return string.Empty;
        }
        return value;
    }

    public static string GetLine(string key, int line = 0) {
        string[] lines = Get(key).Split("\n");
        if (line >= lines.Length) {
            Logger.DebugCrash("Dialog", $"Too few lines in dialog key [{key}] in {Language}: {lines.Length} present, {line} requested");
            return string.Empty;
        }
        return lines[line].Replace(MOD_NEWLINE, "\n");
    }

    public static IEnumerable<string> GetLines(string key) {
        foreach (string line in Get(key).Split("\n")) {
            yield return line.Replace(MOD_NEWLINE, "\n");
        }
    }
}
