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
    public class ShelfInhabitor : GenPass
    {
        public ShelfInhabitor(string name, double loadWeight) : base(name, loadWeight) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = 0; y  < Main.maxTilesY; y++)
                {
                    Tile tile = Main.tile[x, y];
                    if ((tile.TileType == TileID.Platforms || tile.TileType == TileID.Dressers) && WorldGen.genRand.NextBool(2) && tile.Slope == SlopeType.Solid)
                    {
                        if (!Framing.GetTileSafely(x, y - 1).HasTile)
                        {
                            Tile tileUp = Main.tile[x, y - 1];
                            if (tileUp.HasTile)
                                continue;

                            if (WorldGen.genRand.NextBool(14))
                            {
                                WorldGen.PlaceTile(x, y - 1, TileID.WaterCandle, true);
                                continue;
                            }

                            if (!WorldGen.genRand.NextBool(4))
                            {
                                int type = WorldGen.genRand.Next(6);
                                if (WorldGen.genRand.NextBool(20))
                                    type = 6;

                                WorldGen.PlaceTile(x, y - 1, TileID.Books, true, style: WorldGen.genRand.Next(type));
                            }
                            else if (!GenUtils.AreaContainsSensitiveTiles(new List<int> { TileID.Bottles }, x, y, 6, 6))
                            {
                                WorldGen.PlaceTile(x, y - 1, TileID.Bottles, true, style: WorldGen.genRand.NextBool().ToInt() + 1);
                            }
                        }
                    }

                    progress.Set((y + x * Main.maxTilesY) / (float)(Main.maxTilesX * Main.maxTilesY));
                }
            }
        }
    }
}
