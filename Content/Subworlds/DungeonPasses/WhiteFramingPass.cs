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
using StructureHelper;

namespace UltimateSkyblock.Content.Subworlds.DungeonPasses
{
    public class WhiteFramingPass : GenPass
    {
        public WhiteFramingPass(string name, double loadWeight) : base(name, loadWeight) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = 0; y < Main.maxTilesY; y++)
                {
                    if (Main.tile[x, y].TileType == TileID.TeamBlockWhite)
                    {
                        int width = 0;
                        int height = 0;

                        for (int i = 0; i <= 20; i++)
                        {
                            Tile tile = Framing.GetTileSafely(x + i, y);
                            if (!tile.HasTile || tile.TileType != TileID.TeamBlockWhite)
                            {
                                break;
                            }

                            width = i;
                        }

                        for (int j = 0; j <= 20; j++)
                        {
                            Tile tile = Framing.GetTileSafely(x, y + j);
                            if (!tile.HasTile || tile.TileType != TileID.TeamBlockWhite)
                            {
                                break;
                            }

                            height = j;
                        }

                        if (WorldGen.genRand.NextBool())
                            for (int i = x; i <= x + width; i++)
                            {
                                for (int j = y; j <= y + height; j++)
                                {
                                    if (WorldGen.InWorld(i, j))
                                    {
                                        WorldGen.PlaceTile(i, j, TileID.BlueDungeonBrick, forced: true);

                                        if (Framing.GetTileSafely(i - 1, j).TileType == TileID.TeamBlockPinkPlatform)
                                            GenUtils.ShelfRunner(i - 1, j, TileID.TeamBlockPinkPlatform, -1);
                                        else if (Framing.GetTileSafely(i + 1, j).TileType == TileID.TeamBlockPinkPlatform)
                                            GenUtils.ShelfRunner(i + 1, j, TileID.TeamBlockPinkPlatform, 1);

                                        for (int b = i - 1; b <= i + 1; b++)
                                        {
                                            for (int q = j - 1; q <= j + 1; q++)
                                            {
                                                Tile tile = Framing.GetTileSafely(b, q);
                                                tile.WallType = WallID.BlueDungeon;
                                            }
                                        }
                                    }
                                }
                            }
                        else
                        {
                            for (int i = x; i <= x + width; i++)
                            {
                                for (int j = y; j <= y + height; j++)
                                {
                                    if (WorldGen.InWorld(i, j))
                                    {
                                        WorldGen.KillTile(i, j, noItem: true);

                                        if (Framing.GetTileSafely(i - 1, j).TileType == TileID.TeamBlockPinkPlatform)
                                        {
                                            Tile tile = Framing.GetTileSafely(i - 1, j);
                                            tile.Clear(TileDataType.Tile);
                                        }
                                        else if (Framing.GetTileSafely(i + 1, j).TileType == TileID.TeamBlockPinkPlatform)
                                        {
                                            Tile tile = Framing.GetTileSafely(i + 1, j);
                                            tile.Clear(TileDataType.Tile);
                                        }
                                    }
                                }
                            }
                        }

                        progress.Set((y + x * Main.maxTilesY) / (float)(Main.maxTilesX * Main.maxTilesY));
                    }
                }
            }
        }
    }
}
