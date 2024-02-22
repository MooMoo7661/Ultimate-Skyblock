using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace UltimateSkyblock.Content.Biomes
{
    public class DeepstoneWaterStyle : ModWaterStyle
    {
        public override int ChooseWaterfallStyle()
        {
            return ModContent.GetInstance<DeepstoneWaterfallStyle>().Slot;
        }

        public override int GetSplashDust()
        {
            return DustID.Water;
        }

        public override int GetDropletGore()
        {
            return GoreID.WaterDripCavern;
        }

        public override void LightColorMultiplier(ref float r, ref float g, ref float b)
        {
            r = 1f;
            g = 1f;
            b = 1f;
        }

        public override Color BiomeHairColor()
        {
            return Color.Black;
        }

        public override byte GetRainVariant()
        {
            return 1;
        }

        public override Asset<Texture2D> GetRainTexture()
        {
            return ModContent.Request<Texture2D>("UltimateSkyblock/Content/Biomes/DeepstoneRain");
        }
    }
}
