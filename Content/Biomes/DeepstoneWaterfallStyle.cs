using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace UltimateSkyblock.Content.Biomes
{
    public class DeepstoneWaterfallStyle : ModWaterfallStyle
    {
        public override void AddLight(int i, int j) =>
        Lighting.AddLight(new Vector2(i, j).ToWorldCoordinates(), Color.Black.ToVector3() * 0.5f);
    }
}

