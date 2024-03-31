using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Map;

namespace UltimateSkyblock.Content.UI.MapDrawing
{
    public class TileIconDrawing : ModMapLayer
    {
        public static List<MapIcon> icons = new List<MapIcon>();

        public override void Draw(ref MapOverlayDrawContext context, ref string text)
        {
            if (icons.Count == 0) return;

            foreach (var icon in icons)
            {
                if (icon.enabled)
                {
                    var name = context.Draw(icon.Texture, icon.Position, icon.DrawColor, new SpriteFrame(1, 1, 0, 0), icon.DeselectedScale, icon.SelectedScale, Alignment.Center);
                    if (name.IsMouseOver) { text = icon.HoverName; }
                }
            }

            icons.Clear();
        }
    }

    public class MapIcon
    {
        public Vector2 Position;
        public Texture2D Texture;
        public Color DrawColor;
        public float SelectedScale;
        public float DeselectedScale;
        public string HoverName;
        public bool enabled;

        public MapIcon(Vector2 position, Texture2D texture, Color drawColor, float selectedScale, float deselectedScale, string hoverName)
        {
            Position = position;
            Texture = texture;
            DrawColor = drawColor;
            SelectedScale = selectedScale;
            DeselectedScale = deselectedScale;
            HoverName = hoverName;

            enabled = true;
        }

        public void Toggle() => enabled = !enabled;
    }
}
