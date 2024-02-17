using Terraria.ModLoader;

namespace UltimateSkyblock.Content.Tiles.Extractinators
{
    public class AutoExtractorTier1Entity : AutoExtractor_BaseEntity
    {
        protected override int Timer => 120;
        protected override int ConsumeMultiplier => 1;
        protected override int LootMultiplier => 1;
        protected override int TileToBeValidOn => ModContent.TileType<AutoExtractorTier1Tile>();
    }

    public class AutoExtractorTier2Entity : AutoExtractor_BaseEntity
    {
        protected override int Timer => 90;
        protected override int ConsumeMultiplier => 1;
        protected override int LootMultiplier => 2;
        protected override int TileToBeValidOn => ModContent.TileType<AutoExtractorTier2Tile>();
    }

    public class AutoExtractorTier3Entity : AutoExtractor_BaseEntity
    {
        protected override int Timer => 60;
        protected override int ConsumeMultiplier => 2;
        protected override int LootMultiplier => 3;
        protected override int TileToBeValidOn => ModContent.TileType<AutoExtractorTier3Tile>();
    }

    public class AutoExtractorTier4Entity : AutoExtractor_BaseEntity
    {
        protected override int Timer => 30;
        protected override int ConsumeMultiplier => 2;
        protected override int LootMultiplier => 4;
        protected override int TileToBeValidOn => ModContent.TileType<AutoExtractorTier4Tile>();
    }

    public class AutoExtractorTier5Entity : AutoExtractor_BaseEntity
    {
        protected override int Timer => 20;
        protected override int ConsumeMultiplier => 3;
        protected override int LootMultiplier => 8;
        protected override int TileToBeValidOn => ModContent.TileType<AutoExtractorTier5Tile>();
    }
}
