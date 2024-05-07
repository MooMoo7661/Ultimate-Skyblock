using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.IO;
using Terraria.WorldBuilding;
using UltimateSkyblock.Content.Items.Placeable;
using static UltimateSkyblock.Content.Subworlds.MiningSubworld;
using static UltimateSkyblock.Content.Subworlds.GenUtils;
using UltimateSkyblock.Content.Utils;
using UltimateSkyblock.Content.Tiles.Blocks;

namespace UltimateSkyblock.Content.Subworlds.MiningPasses
{
    public class HellPass : GenPass
    {
        public HellPass(string name, double loadWeight) : base(name, loadWeight) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Generating Hell";

            //Clearing all blocks in the underworld
            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = Main.UnderworldLayer + 5; y < Main.maxTilesY; y++)
                {
                    Tile tile = Framing.GetTileSafely(x, y);
                    tile.ClearTile();
                }
            }

            // Adding a barrier between hell and the deepstone
            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = Main.UnderworldLayer - 6; y < Main.UnderworldLayer + 2; y++)
                {
                    if (Main.rand.NextBool(3))
                        WorldGen.TileRunner(x, y, 7, 25, ModContent.TileType<DeepstoneTile>(), true);
                }
            }

            //Creating the ground for the ash
            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = Main.maxTilesY - 100; y < Main.maxTilesY - 50; y++)
                {
                    if (Main.rand.NextBool(20))
                        WorldGen.TileRunner(x, y, 9, 22, TileID.Ash, true);
                }
            }

            //Placing ash in any places that might have been missed
            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = Main.maxTilesY - 100; y < Main.maxTilesY; y++)
                {
                    WorldGen.PlaceTile(x, y, TileID.Ash, true);
                }
            }

            //Placing chasms of temporary blocks on the  & right edge of the world
            for (int q = -20; q < 20; q++)
            {
                for (int i = 0; i < 100; i++)
                {
                    WorldGen.TileRunner(50 + q, Main.maxTilesY - i, 20, 28, TileID.BoneBlock, true);
                }

                for (int i = 0; i < 100; i++)
                {
                    WorldGen.TileRunner(Main.maxTilesX - 50 + q, Main.maxTilesY - i, 20, 28, TileID.BoneBlock, true);
                }
            }

            //Clearing all of those temporary blocks
            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = Main.UnderworldLayer - 20; y < Main.maxTilesY; y++)
                {
                    Tile tile = Framing.GetTileSafely(x, y);
                    if (tile.HasTile && tile.TileType == TileID.BoneBlock)
                    {
                        tile.ClearTile();
                    }
                }
            }

            //Growing grass on ash tiles
            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = Main.UnderworldLayer + 20; y < Main.maxTilesY; y++)
                {
                    Tile tile = Framing.GetTileSafely(x, y);
                    if (tile.TileType == TileID.Ash)
                    {
                        GetSurroundingTiles(x, y, out Tile left, out Tile right, out Tile top, out Tile bottom);
                        if (!top.HasTile || !left.HasTile || !right.HasTile || !bottom.HasTile || !Framing.GetTileSafely(x - 1, y - 1).HasTile || !Framing.GetTileSafely(x + 1, y - 1).HasTile || !Framing.GetTileSafely(x + 1, y + 1).HasTile || !Framing.GetTileSafely(x - 1, y + 1).HasTile)
                        {
                            WorldGen.PlaceTile(x, y, TileID.AshGrass, true);
                        }
                    }
                }
            }

            WorldGen.AddHellHouses();

            //Generates locked demon chests
            //Scans an 8x8 area for other demon chests, if so - doesn't place.
            for (int x = 50; x < Main.maxTilesX - 50; x++)
            {
                for (int y = Main.UnderworldLayer + 30; y < Main.maxTilesY; y++)
                {
                    Tile tile = Framing.GetTileSafely(x, y);
                    Tile right = Framing.GetTileSafely(x + 1, y);
                    if (Main.rand.NextBool(15) && tile.HasTile && tile.TileType == TileID.ObsidianBrick && right.HasTile && right.TileType == TileID.ObsidianBrick && tile.Slope == SlopeType.Solid && right.Slope == SlopeType.Solid)
                    {
                        if (GenUtils.SuitableFor2x2(x, y) && !GenUtils.AreaContainsSensitiveTiles(new List<int> { TileID.Containers }, x, y, 8, 8))
                        {
                            Generator.GenerateStructure("Content/Subworlds/SubStructures/UnderworldChest", new Point16(x, y - 2), UltimateSkyblock.Instance);
                        }
                    }
                }
            }

            //Creating & growing ash trees
            for (int x = 50; x < Main.maxTilesX - 50; x++)
            {
                for (int y = Main.UnderworldLayer + 30; y < Main.maxTilesY; y++)
                {
                    if (WorldGen.InWorld(x, y) && WorldGen.genRand.NextBool(4))
                        WorldGen.GrowTreeWithSettings(x, y, WorldGen.GrowTreeSettings.Profiles.Tree_Ash);
                }
            }

            //This is handling generating a "platform" right below the bottom of the world, so lava doesn't fall out.
            //TileRunner is run in a straight line, to provide a better variation and blending.
            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = Main.maxTilesY - 20; y < Main.maxTilesY; y++)
                {
                    Tile tile = Framing.GetTileSafely(x, y);
                    if (!tile.HasTile)
                    {
                        WorldGen.TileRunner(x, y, 8, 4, TileID.Ash, true);
                    }

                    progress.Set((y + x * Main.maxTilesY) / (float)(Main.maxTilesX * Main.maxTilesY));
                }

                for (int y = Main.UnderworldLayer; y < Main.maxTilesY; y++)
                {
                    Tile tile = Framing.GetTileSafely(x, y);

                    //Smooths the line between stone and ash generation
                    //if (tile.HasTile && (tile.TileType == TileID.Stone || tile.TileType == MiningSubworld.Deepstone))
                    //{
                    //    WorldGen.TileRunner(x, y, 25, 10, TileID.Ash);
                    //}

                    //Places lava everywhere from the bottom of the world up by 100 on the edges
                    if (y >= Main.maxTilesY - 100 && !tile.HasTile && (x < 100 || x > Main.maxTilesX - 100))
                    {
                        WorldGen.PlaceLiquid(x, y, (byte)LiquidID.Lava, 255);
                    }

                    progress.Set((y + x * Main.maxTilesY) / (float)(Main.maxTilesX * Main.maxTilesY));
                }
            }

            // Hellstone generation
            LoopWorldAndGenerateTiles(2, Main.rand.Next(6, 10), Main.rand.Next(6, 10), type: TileID.Hellstone, tilesThatCanBeGeneratedOn: new List<int> { TileID.Ash }, minHeightRequirement: Main.UnderworldLayer + 60);

            //Removing some of the unusually extra obsidian house furntiture
            for (int x = 100; x < Main.maxTilesX - 100; x++)
            {
                for (int y = Main.UnderworldLayer; y < Main.maxTilesY - 50; y++)
                {
                    Tile tile = Main.tile[x, y];
                    if (WorldGen.genRand.NextBool(4) && (tile.TileType == TileID.HangingLanterns || tile.TileType == TileID.Chandeliers || tile.TileType == TileID.Lamps || tile.TileType == TileID.Banners))
                    {
                        WorldGen.KillTile(x, y, noItem: true);
                    }
                }
            }
        }
    }
}
