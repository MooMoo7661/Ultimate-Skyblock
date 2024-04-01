namespace UltimateSkyblock.Content.Biomes
{
    public class DeepstoneWaterStyle : ModWaterStyle
    {
        private static Asset<Texture2D> rain;

        public override void Load()
        {
            rain = ModContent.Request<Texture2D>("UltimateSkyblock/Content/Biomes/DeepstoneRain");
        }

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
            return rain;
        }
    }
}
