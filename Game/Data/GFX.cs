using Monocle;

namespace Gamespace;

public static class GFX {
    public static Atlas Game;
    public static SpriteBank Sprites;

    public static void Load() {
        Game = Atlas.FromAtlas("Atlases/.xml", Atlas.AtlasDataFormat.CrunchXml);
        Sprites = new SpriteBank(Game, "Sprites.xml");
    }

    public static Image Image(string path) {
        return new Image(
            Game.GetOrDefault(path, Game["missing"])
        );
    }

    public static Sprite Sprite(string path) {
        return Sprites.Create(path);
    }
}
