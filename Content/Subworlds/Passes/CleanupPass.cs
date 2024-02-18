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

namespace UltimateSkyblock.Content.Subworlds.Passes
{
    public class CleanupPass : GenPass
    {
        public CleanupPass(string name, double loadWeight) : base(name, loadWeight) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Final Cleanup";
            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = 0; y < Main.maxTilesY; y++)
                {
                    Tile tile = Framing.GetTileSafely(x, y);

                    //Removing any extra water droplets that may have generated on sloped tiles.
                    if (tile.HasTile && tile.Slope != SlopeType.Solid && Framing.GetTileSafely(x, y + 1).TileType == TileID.WaterDrip)
                    {
                        Framing.GetTileSafely(x, y + 1).ClearTile();
                    }
                    progress.Set((y + x * Main.maxTilesY) / (float)(Main.maxTilesX * Main.maxTilesY));
                }
            }
        }
    }
}
