using Terraria.ModLoader;
using TileFunctionLibrary.API;

namespace UltimateSkyblock.Content.Tiles.Extractinators
{
    public class AutoExtractorTier1Tile : AutoExtractor_BaseTile
    {
        protected override int ExtractorTile => Type;
        protected override string TilesheetPath => "UltimateSkyblock/Content/Tiles/Extractinators/AutoExtractorTier1Tile";
        protected override ModTileEntity Entity => ModContent.GetInstance<AutoExtractorTier1Entity>();
    }

    public class AutoExtractorTier2Tile : AutoExtractor_BaseTile
    {
        protected override int ExtractorTile => Type;
        protected override string TilesheetPath => "UltimateSkyblock/Content/Tiles/Extractinators/AutoExtractorTier2Tile";
        protected override ModTileEntity Entity => ModContent.GetInstance<AutoExtractorTier2Entity>();
    }

    public class AutoExtractorTier3Tile : AutoExtractor_BaseTile
    {
        protected override int ExtractorTile => Type;
        protected override string TilesheetPath => "UltimateSkyblock/Content/Tiles/Extractinators/AutoExtractorTier3Tile";
        protected override ModTileEntity Entity => ModContent.GetInstance<AutoExtractorTier3Entity>();
    }
    public class AutoExtractorTier4Tile : AutoExtractor_BaseTile
    {
        protected override int ExtractorTile => Type;
        protected override string TilesheetPath => "UltimateSkyblock/Content/Tiles/Extractinators/AutoExtractorTier4Tile";
        protected override ModTileEntity Entity => ModContent.GetInstance<AutoExtractorTier4Entity>();
    }

    public class AutoExtractorTier5Tile : AutoExtractor_BaseTile
    {
        protected override int ExtractorTile => Type;
        protected override string TilesheetPath => "UltimateSkyblock/Content/Tiles/Extractinators/AutoExtractorTier5Tile";
        protected override ModTileEntity Entity => ModContent.GetInstance<AutoExtractorTier5Entity>();
    }
}
