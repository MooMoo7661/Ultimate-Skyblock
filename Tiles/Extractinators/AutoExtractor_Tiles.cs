using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using TileFunctionLibrary.API;
using OneBlock.Items.Bombs;
using static Terraria.ModLoader.PlayerDrawLayer;
using Terraria.GameInput;

namespace OneBlock.Tiles.Extractinators
{
    public class AutoExtractorTier1Tile : AutoExtractor_BaseTile
    {
        protected override int ExtractorTile => this.Type;
        protected override string TilesheetPath => "OneBlock/Tiles/Extractinators/AutoExtractorTier1Tile";
        protected override ModTileEntity Entity => ModContent.GetInstance<AutoExtractorTier1Entity>();
    }

    public class AutoExtractorTier2Tile : AutoExtractor_BaseTile
    {
        protected override int ExtractorTile => this.Type;
        protected override string TilesheetPath => "OneBlock/Tiles/Extractinators/AutoExtractorTier2Tile";
        protected override ModTileEntity Entity => ModContent.GetInstance<AutoExtractorTier2Entity>();
    }

    public class AutoExtractorTier3Tile : AutoExtractor_BaseTile
    {
        protected override int ExtractorTile => this.Type;
        protected override string TilesheetPath => "OneBlock/Tiles/Extractinators/AutoExtractorTier3Tile";
        protected override ModTileEntity Entity => ModContent.GetInstance<AutoExtractorTier3Entity>();
    }
    public class AutoExtractorTier4Tile : AutoExtractor_BaseTile
    {
        protected override int ExtractorTile => this.Type;
        protected override string TilesheetPath => "OneBlock/Tiles/Extractinators/AutoExtractorTier4Tile";
        protected override ModTileEntity Entity => ModContent.GetInstance<AutoExtractorTier4Entity>();
    }

    public class AutoExtractorTier5Tile : AutoExtractor_BaseTile
    {
        protected override int ExtractorTile => this.Type;
        protected override string TilesheetPath => "OneBlock/Tiles/Extractinators/AutoExtractorTier5Tile";
        protected override ModTileEntity Entity => ModContent.GetInstance<AutoExtractorTier5Entity>();
    }
}
