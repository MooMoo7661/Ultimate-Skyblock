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
using UltimateSkyblock.Content.Tiles.Blocks;

namespace UltimateSkyblock.Content.Subworlds.MiningPasses
{
    public class DeepstoneCaveFeaturesPass : GenPass
    {
        public DeepstoneCaveFeaturesPass(string name, double loadWeight) : base(name, loadWeight) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Deepstone Cave Features";

            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = 0; y < Main.maxTilesY; y++)
                {
                    if (WorldGen.genRand.NextBool(14) && !Framing.GetTileSafely(x, y - 1).HasTile && Framing.GetTileSafely(x - 1, y).HasTile && Framing.GetTileSafely(x + 1, y).HasTile && Main.tile[x, y].TileType == ModContent.TileType<DeepstoneTile>())
                    {

                        int height = WorldGen.genRand.Next(8, 15);

                        if (WorldGen.genRand.NextBool(3))
                        {
                            int scan = WorldGen.genRand.Next(8, 15) * 3;

                            for (int i = 0; i < 13; i++)
                            {
                                Tile tile = Main.tile[x, y];
                                if (tile.HasTile)
                                {
                                    scan = i;
                                    break;
                                }
                            }

                            scan /= 3;

                            for (int j = 1; j < scan; j++)
                            {
                                int decrease = (j % 2 == 0) ? j - 1 : j;
                                WorldGen.TileRunner(x, y - j + 2, Math.Clamp(scan - decrease, 1, 255), 2, TileID.BoneBlock, true, 0, -3f);
                            }
                        }
                        else
                        {
                            for (int i = 1; i < height; i++)
                            {
                                int decrease = (i % 2 == 0) ? i - 1 : i;
                                WorldGen.TileRunner(x, y - i + 2, Math.Clamp(height - decrease, 1, 255), 2, TileID.BoneBlock, true, 0, -3f);
                            }
                        }
                    }

                    progress.Set((y + x * Main.maxTilesY) / (float)(Main.maxTilesX * Main.maxTilesY));
                }
            }

            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = 0; y < Main.maxTilesY; y++)
                {
                    if (WorldGen.genRand.NextBool(17) && !Framing.GetTileSafely(x, y + 1).HasTile && Main.tile[x, y].TileType == ModContent.TileType<DeepstoneTile>())
                    {
                        int height = WorldGen.genRand.Next(8, 15);
                        for (int i = 1; i < height; i++)
                        {
                            int decrease = (i % 2 == 0) ? i - 1 : i;
                            WorldGen.TileRunner(x, y + i - 2, Math.Clamp(height - decrease, 1, 255), 2, TileID.BoneBlock, true, 0, 3f);
                        }
                    }
                }
            }

            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = 0; y < Main.maxTilesY; y++)
                {
                    if (Main.tile[x, y].TileType == TileID.BoneBlock)
                        WorldGen.PlaceTile(x, y, ModContent.TileType<DeepstoneTile>(), true, true);
                }
            }
        }
    }
}
