using System;
using Terraria.ModLoader;
using TileFunctionLibrary.API;
using UltimateSkyblock.Content.Items.Placeable.Objects;

namespace UltimateSkyblock.Content.Tiles.Extractinators
{
    public class AutoExtractorTier1Tile : AutoExtractor_BaseTile
    {
        protected override bool ChlorophyteTable => false;
        protected override bool ChlorophyteTrades => false;

        protected override int ExtractorTile => Type;
        protected override string TilesheetPath => "UltimateSkyblock/Content/Tiles/Extractinators/AutoExtractorTier1Tile";
        protected override ModTileEntity Entity => ModContent.GetInstance<AutoExtractorTier1Entity>();
        protected override Func<int> MyItemType => ModContent.ItemType<AutoExtractor>;
	}

    public class AutoExtractorTier2Tile : AutoExtractor_BaseTile
    {
        protected override bool ChlorophyteTable => false;
        protected override bool ChlorophyteTrades => false;

        protected override int ExtractorTile => Type;
        protected override string TilesheetPath => "UltimateSkyblock/Content/Tiles/Extractinators/AutoExtractorTier2Tile";
        protected override ModTileEntity Entity => ModContent.GetInstance<AutoExtractorTier2Entity>();
		protected override Func<int> MyItemType => ModContent.ItemType<AutoExtractorTier2>;
	}

    public class AutoExtractorTier3Tile : AutoExtractor_BaseTile
    {
        protected override bool ChlorophyteTable => false;
        protected override bool ChlorophyteTrades => false;

        protected override int ExtractorTile => Type;
        protected override string TilesheetPath => "UltimateSkyblock/Content/Tiles/Extractinators/AutoExtractorTier3Tile";
        protected override ModTileEntity Entity => ModContent.GetInstance<AutoExtractorTier3Entity>();
		protected override Func<int> MyItemType => ModContent.ItemType<AutoExtractorTier3>;
	}
    public class AutoExtractorTier4Tile : AutoExtractor_BaseTile
    {
        protected override bool ChlorophyteTable => true;
        protected override bool ChlorophyteTrades => true;

        protected override int ExtractorTile => Type;
        protected override string TilesheetPath => "UltimateSkyblock/Content/Tiles/Extractinators/AutoExtractorTier4Tile";
        protected override ModTileEntity Entity => ModContent.GetInstance<AutoExtractorTier4Entity>();
		protected override Func<int> MyItemType => ModContent.ItemType<AutoExtractorTier4>;
	}

    public class AutoExtractorTier5Tile : AutoExtractor_BaseTile
    {
        protected override bool ChlorophyteTable => true;
        protected override bool ChlorophyteTrades => true;

        protected override int ExtractorTile => Type;
        protected override string TilesheetPath => "UltimateSkyblock/Content/Tiles/Extractinators/AutoExtractorTier5Tile";
        protected override ModTileEntity Entity => ModContent.GetInstance<AutoExtractorTier5Entity>();
		protected override Func<int> MyItemType => ModContent.ItemType<AutoExtractorTier5>;
	}
}
