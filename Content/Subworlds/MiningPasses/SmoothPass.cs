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

namespace UltimateSkyblock.Content.Subworlds.MiningPasses
{
    public class SmoothPass : GenPass
    {
        public SmoothPass(string name, double loadWeight) : base(name, loadWeight) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Smoothing the World";
            for (int x = 50; x < Main.maxTilesX - 50; x++)
            {
                for (int y = 50; y < Main.maxTilesY - 50; y++)
                {
                    if (Main.rand.NextBool(7) && WorldGen.InWorld(x, y) && Framing.GetTileSafely(x, y + 1).TileType != TileID.WaterDrip && Framing.GetTileSafely(x, y + 1).TileType != TileID.LavaDrip)
                        Tile.SmoothSlope(x, y);
                    progress.Set((y + x * Main.maxTilesY) / (float)(Main.maxTilesX * Main.maxTilesY));
                }
            }

            // Removes blocks with no neighboring tiles, to avoid floating singular tiles.
            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = 0; y < Main.maxTilesY; y++)
                {
                    Tile tile = Framing.GetTileSafely(x, y);
                    if (tile.HasTile)
                    {
                        Tile tileUp = Framing.GetTileSafely(x, y - 1);
                        Tile tileDown = Framing.GetTileSafely(x, y + 1);
                        Tile tileLeft = Framing.GetTileSafely(x - 1, y);
                        Tile tileRight = Framing.GetTileSafely(x + 1, y);

                        if (!tileUp.HasTile && !tileDown.HasTile && !tileLeft.HasTile && !tileRight.HasTile)
                        {
                            tile.ClearTile();
                        }
                    }
                }
            }
        }
    }
}
