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
                    if (tile.HasTile && tile.LiquidAmount == 0 /*&& !tileUp.HasTile*/ && WorldGen.genRand.NextBool(30) && traps < 50 && y < Main.UnderworldLayer - 300)
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
