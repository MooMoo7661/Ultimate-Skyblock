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
    public class TrapsPass : GenPass
    {
        public TrapsPass(string name, double loadWeight) : base(name, loadWeight) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            int traps = 0;
            progress.Message = "Placing Traps";

            for (int x = 100; x < Main.maxTilesX - 100; x++)
            {
                for (int y = 100; y < Main.maxTilesY - 100; y++)
                {
                    Tile tile = Framing.GetTileSafely(x, y);
                    Tile tileUp = Framing.GetTileSafely(x, y - 1);
                    if (tile.HasTile && tile.LiquidAmount == 0 /*&& !tileUp.HasTile*/ && Main.rand.NextBool(15) && traps < 100)
                    {
                        if (WorldGen.placeTrap(x, y))
                        {
                            traps++;

                        }
                    }

                    progress.Value += ((y + x * Main.maxTilesY) / (float)(Main.maxTilesX * Main.maxTilesY));
                }
            }
        }
    }
}
