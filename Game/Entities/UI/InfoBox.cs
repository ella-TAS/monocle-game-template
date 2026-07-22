using Gamespace.Data;
using Monocle;

namespace Gamespace.Entities.UI;

public class InfoBox : Entity {
    private NineSliceBox box;

    public InfoBox() {
        Depth = -10000;
        box = new NineSliceBox(GFX.Game["ui/infoBox"], 99, 21, tileSize: 3);
        Add(box);
    }


    public override void Update() {
        base.Update();

        box.Position = MInput.Mouse.Position;
    }
}
