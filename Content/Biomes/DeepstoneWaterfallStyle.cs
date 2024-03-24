namespace UltimateSkyblock.Content.Biomes
{
    public class DeepstoneWaterfallStyle : ModWaterfallStyle
    {
        public override void AddLight(int i, int j) =>
        Lighting.AddLight(new Vector2(i, j).ToWorldCoordinates(), Color.Black.ToVector3() * 0.5f);
    }
}

