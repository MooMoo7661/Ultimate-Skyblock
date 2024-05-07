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
    public class PurpleFramingPass : GenPass
    {
        public PurpleFramingPass(string name, double loadWeight) : base(name, loadWeight) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = 0; y  < Main.maxTilesY; y++)
                {
                    if (Main.tile[x, y].TileType == TileID.TeamBlockPink)
                    {
                        int width = 0;
                        int height = WorldGen.genRand.Next(4, 7);

                        for (int i = 0; i < 20; i++)
                        {
                            Tile tile = Framing.GetTileSafely(x + i, y);
                            if (!tile.HasTile || tile.TileType != TileID.TeamBlockPink)
                                break;

                            width = i;
                        }

                        for (int i = 0; i <= width; i++)
                        {
                            for (int j = 0; j <= height; j++)
                            {
                                Tile tile = Framing.GetTileSafely(x + i, y + j);
                                if (tile.TileType == TileID.TeamBlockBlue)
                                    break;

                                WorldGen.PlaceTile(x + i, y + j, TileID.BlueDungeonBrick, true, forced: true);

                                for (int b = i - 1; b <= i + 1; b++)
                                {
                                    for (int q = j - 1; q <= j + 1; q++)
                                    {
                                        tile = Framing.GetTileSafely(x + b, y + q);
                                        tile.WallType = WallID.BlueDungeon;
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
