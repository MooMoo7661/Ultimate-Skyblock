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
    public class AutoExtractorTier1Entity : AutoExtractor_BaseEntity
    {
        protected override int Timer => 120;
        protected override int ConsumeMultiplier => 1;
        protected override int LootMultiplier => 1;
        protected override int TileToBeValidOn => ModContent.TileType<AutoExtractorTier1Tile>();
        protected override bool HighPlatinum => false;
    }

    public class AutoExtractorTier2Entity : AutoExtractor_BaseEntity
    {
        protected override int Timer => 90;
        protected override int ConsumeMultiplier => 1;
        protected override int LootMultiplier => 2;
        protected override int TileToBeValidOn => ModContent.TileType<AutoExtractorTier2Tile>();
        protected override bool HighPlatinum => false;
    }

    public class AutoExtractorTier3Entity : AutoExtractor_BaseEntity
    {
        protected override int Timer => 60;
        protected override int ConsumeMultiplier => 2;
        protected override int LootMultiplier => 3;
        protected override int TileToBeValidOn => ModContent.TileType<AutoExtractorTier3Tile>();
        protected override bool HighPlatinum => false;
    }

    public class AutoExtractorTier4Entity : AutoExtractor_BaseEntity
    {
        protected override int Timer => 30;
        protected override int ConsumeMultiplier => 2;
        protected override int LootMultiplier => 4;
        protected override int TileToBeValidOn => ModContent.TileType<AutoExtractorTier4Tile>();
        protected override bool HighPlatinum => false;
    }

    public class AutoExtractorTier5Entity : AutoExtractor_BaseEntity
    {
        protected override int Timer => 20;
        protected override int ConsumeMultiplier => 3;
        protected override int LootMultiplier => 8;
        protected override int TileToBeValidOn => ModContent.TileType<AutoExtractorTier5Tile>();
        protected override bool HighPlatinum => true;
    }
}
