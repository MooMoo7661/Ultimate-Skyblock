using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ObjectData;
using UltimateSkyblock.Content.Items.Placeable;

namespace UltimateSkyblock.Content.Tiles.Environment
{
    public abstract class DeepstoneRubbleSmallBase : ModTile
    {
        public override string Texture => "UltimateSkyblock/Content/Tiles/Environment/DeepstoneRubbleSmall";

        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoFail[Type] = true;
            Main.tileObsidianKill[Type] = true;

            DustType = DustID.Stone;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.DrawYOffset = 2;
            TileObjectData.addTile(Type);

            AddMapEntry(new Color(60, 60, 60));
        }
    }

    public class DeepstoneRubbleSmallFake : DeepstoneRubbleSmallBase
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            // Add rubble variant, all existing styles, to Rubblemaker, allowing to place this tile by consuming ExampleBlock
            FlexibleTileWand.RubblePlacementSmall.AddVariations(ModContent.ItemType<Deepstone>(), Type, 0, 1, 2, 3, 4, 5);
            RegisterItemDrop(ModContent.ItemType<Deepstone>());
        }
    }

    public class DeepstoneRubbleSmallNatural : DeepstoneRubbleSmallBase
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            TileObjectData.GetTileData(Type, 0).LavaDeath = false;
        }

        public override void DropCritterChance(int i, int j, ref int wormChance, ref int grassHopperChance, ref int jungleGrubChance)
        {
            wormChance = 8;
        }
    }
}

