using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria;
using Terraria.Map;
using Terraria.ModLoader;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using System.IO;

namespace OneBlock.MapDrawing
{
    public class MapIconToggleDrawing : ModMapLayer
    {
        public static readonly string path = MainMapLayer.path;
        

        public override void Draw(ref MapOverlayDrawContext context, ref string text)
        {
            ToggleButton DungeonToggle = new()
            {
                BesideTexture = ModContent.Request<Texture2D>(path + "IconMushroom").Value,
                DrawPos = new Point(-200, -200)
            };
            DungeonToggle.Draw(ref context, ref text);
        }
    }

    public class ToggleButton 
    {
        public Texture2D BesideTexture = ModContent.Request<Texture2D>("OneBlock/MapDrawing/Icons/IconMushroom").Value;

        public static bool enabled = true;

        string on = "Toggle off";
        string off = "Toggle on";

        public Point DrawPos = new(0, 0);
        public static void Toggle() => enabled = !enabled;

        public static Texture2D GetToggleTexture()
        {
            return enabled ? ModContent.Request<Texture2D>("OneBlock/MapDrawing/Icons/IconToggleOn").Value : ModContent.Request<Texture2D>("OneBlock/MapDrawing/Icons/IconToggleOff").Value;
        }

        public void Draw(ref MapOverlayDrawContext context, ref string text)
        {
            const float scaleIfNotSelected = 1f;
            const float scaleIfSelected = 1.5f;

            var result = context.Draw(GetToggleTexture(), new Vector2(DrawPos.X, DrawPos.Y), Color.White, new SpriteFrame(1, 1, 0, 0), scaleIfNotSelected, scaleIfSelected, Alignment.Center);
            context.Draw(BesideTexture, new Vector2(DrawPos.X - 50, DrawPos.Y), Color.White, new SpriteFrame(1, 1, 0, 0), 1f, 1f, Alignment.Center);

            if (result.IsMouseOver) { text = enabled ? on : off; }
            if (result.IsMouseOver && Main.mouseLeft && Main.mouseLeftRelease) { Toggle(); }
        }
    }
}
