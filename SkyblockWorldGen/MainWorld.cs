using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using static Terraria.WorldGen;
using StructureHelper;
using Terraria.Localization;
using Terraria.Audio;
using System.IO;
using tModPorter;
using static OneBlock.OneBlock;
using Microsoft.Xna.Framework.Graphics;
using static WorldHelpers;
using OneBlock.Items.Placeable;
using OneBlock.Tiles.Blocks;

namespace OneBlock.SkyblockWorldGen
{
    public partial class MainWorld : ModSystem
    {
        /// <summary>
        /// Used with RollHellIslands to pick an island that is not the same as the previous one.
        /// </summary>
        public static string previousStructure;
        public static float ScaleBasedOnWorldSizeX;
        public static float ScaleBasedOnWorldSizeY;
        public static WorldSizes WorldSize;

        public static OneBlockModConfig config = ModContent.GetInstance<OneBlockModConfig>();
        public enum WorldSizes
        {
            Small,
            Medium,
            Large
        }
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            tasks.RemoveAll(task => task.Name == "Full Desert" || task.Name == "Buried Chests" || task.Name == "Mushroom Patches" ||
            task.Name == "Micro Biomes" || task.Name == "Moss" || task.Name == "Guide");
        }

        public override void PreWorldGen()
        {           
            WorldSize = WorldGen.GetWorldSize() switch
            {
                0 => WorldSizes.Small,
                1 => WorldSizes.Medium,
                _ => WorldSizes.Large,
            };

            ScaleBasedOnWorldSizeX = WorldGen.GetWorldSize() switch
            { 
                0 => 1,
                1 => 30,
                2 => 60,
                _ => 80,

            };

            ScaleBasedOnWorldSizeY = WorldGen.GetWorldSize() switch
            {
                0 => 20,
                1 => 30,
                2 => 40,
                _ => 50,
            };
        }

        public override void OnWorldLoad()
        {
            WorldSize = WorldGen.GetWorldSize() switch
            {
                0 => WorldSizes.Small,
                1 => WorldSizes.Medium,
                _ => WorldSizes.Large,
            };
        }

        /// <summary>
        /// Overridden to prevent the "spreading evil" tasks, as they can completely ruin islands with stone on them. The hallow is manually generated in this as well.
        /// </summary>
        public override void ModifyHardmodeTasks(List<GenPass> list)
        {
            list.RemoveAll(task => task.Name != "Hardmode Announcement");

            GenHallowedIslands();
        }
    }
    

    public class RefinableDirt : GlobalItem
    {
        public override void SetStaticDefaults()
        {
            if (ModContent.GetInstance<OneBlockModConfig>().DirtAndSandCanBeExtracted)
            {
                ItemID.Sets.ExtractinatorMode[ItemID.DirtBlock] = 0;
                ItemID.Sets.ExtractinatorMode[ItemID.SandBlock] = 0;
            }
        }
    }

    public class GlobalSapling : GlobalTile
    {
        public override void RandomUpdate(int i, int j, int type)
        {
            var config = ModContent.GetInstance<OneBlockModConfig>();

            if ((type == TileID.Saplings || type == TileID.GemSaplings) && config.FastTrees)
            {
                if (Main.rand.NextBool(4))
                {
                    GrowTree(i, j);
                }
            }

            Tile tile = Main.tile[i, j];

           
        }

        //public override void PostDraw(int i, int j, int type, SpriteBatch spriteBatch)
        //{
        //    if (Main.rand.NextBool(100))
        //    {
        //        if (type == TileID.Extractinator)
        //        {
        //            Point webPoint = new(Main.rand.Next(i - 5, i + 5), Main.rand.Next(j - 5, j + 5));

        //            Tile tile = Main.tile[webPoint.X, webPoint.Y];

        //            if (!tile.HasTile)
        //            WorldGen.PlaceTile(webPoint.X, webPoint.Y, TileID.Cobweb, true, false);
        //        }
        //    }
        //}

        public override bool CanKillTile(int i, int j, int type, ref bool blockDamaged)
        {
           //if (type == TileID.Titanstone && !Main.hardMode)
           //{
           //     return false;
           //}

            return true;
        }
        public override void SetStaticDefaults()
        {
            
            Main.tileMerge[TileID.Hellstone][TileID.Ash] = true;
            Main.tileMerge[TileID.Hellstone][TileID.ReefBlock] = true;
            Main.tileMerge[TileID.Hellstone][TileID.Coralstone] = true;
            Main.tileMerge[TileID.Ash][ModContent.TileType<VolcanicStoneTile>()] = true;
            Main.tileMerge[TileID.Stone][ModContent.TileType<VolcanicStoneTile>()] = true;
            Main.tileMerge[TileID.LavaMoss][ModContent.TileType<VolcanicStoneTile>()] = true;
            Main.tileMerge[TileID.AshGrass][ModContent.TileType<VolcanicStoneTile>()] = true;

            TileID.Sets.PreventsSandfall[TileID.Sand] = true;
            TileID.Sets.PreventsSandfall[TileID.Slush] = true;
        }

        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            Tile tile = Main.tile[i, j];

            if (type == TileID.Saplings && !fail && tile.TileFrameY != 0 && ModContent.GetInstance<OneBlockModConfig>().SaplingsDropAcorns)
            {
                Item.NewItem(GetItemSource_FromTileBreak(i, j), new Vector2(i * 16, j * 16), Vector2.Zero, ItemID.Acorn);
            }

            if (type == TileID.Crystals && !Main.hardMode)
            {
                noItem = true;
            }
        }
    }
}