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
    public class HellBarrierPass : GenPass
    {
        public HellBarrierPass(string name, double loadWeight) : base(name, loadWeight) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Creating Hell Barrier";
            // This is to help prevent gem caves from leaking into hell. Generates a long strip of stone to seperate hell from the caverns.
            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = Main.UnderworldLayer - 210; y < Main.UnderworldLayer - 200; y++)
                {
                    WorldGen.TileRunner(x, y + Main.rand.Next(-6, 6), 12, 4, MiningSubworld.Deepstone, true);

                    progress.Set((y + x * Main.maxTilesY) / (float)(Main.maxTilesX * Main.maxTilesY));
                }
            }
        }
    }
}
