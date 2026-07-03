using Monocle;

namespace Gamespace.Data;

public static class GFX {
    public static Atlas Game;
    public static SpriteBank Sprites;

    public static void Load() {
        Game = Atlas.FromAtlas("Atlases/.xml", Atlas.AtlasDataFormat.CrunchXml);
        Sprites = new SpriteBank(Game, "Sprites.xml");
    }
}
