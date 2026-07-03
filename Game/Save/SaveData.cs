using Monocle;

namespace Gamespace.Save;

public class SaveData {
    private const string FILE = "Save.json";
    private const SaveLoad.SerializeMode MODE = SaveLoad.SerializeMode.Json;
    public static SaveData Instance;

    public int Money = 100;

    public static void Save() {
        SaveLoad.SafeSave(Instance, FILE, MODE);
    }

    public static void Load() {
        Instance = SaveLoad.Load<SaveData>(FILE, MODE) ?? new SaveData();
    }
}
