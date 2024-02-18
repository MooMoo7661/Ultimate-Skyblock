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
    public class StalactitesPass : GenPass
    {
        public StalactitesPass(string name, double loadWeight) : base(name, loadWeight) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            UltimateSkyblock.Instance.Logger.Info("Placing Stalactites");
            progress.Message = "Placing Stalactites";
            for (int x = 100; x < Main.maxTilesX - 100; x++)
            {
                for (int y = 100; y < Main.maxTilesY - 100; y++)
                {
                    Tile tile = Framing.GetTileSafely(x, y);
                    if (!tile.HasTile && tile.Slope == SlopeType.Solid && Framing.GetTileSafely(x, y - 1).HasTile && Main.rand.NextBool(5))
                    {
                        WorldGen.PlaceUncheckedStalactite(x, y, Main.rand.NextBool(), Main.rand.Next(3), false);
                    }
                    progress.Set((y + x * Main.maxTilesY) / (float)(Main.maxTilesX * Main.maxTilesY));
                }
            }
        }
    }
}
