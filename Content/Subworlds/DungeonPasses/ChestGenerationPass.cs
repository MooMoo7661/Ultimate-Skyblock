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
    public class ChestGenerationPass : GenPass
    {
        public ChestGenerationPass(string name, double loadWeight) : base(name, loadWeight) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = 0; y  < Main.maxTilesY; y++)
                {
                    switch (Main.tile[x, y].TileType)
                    {
                        case TileID.TeamBlockYellow:
                            GenUtils.PlaceDungeonChest(x, y, "LockedGoldenChest", false, 8);
                            break;
                        case TileID.TeamBlockYellowPlatform:
                            GenUtils.PlaceDungeonChest(x, y, "LockedGoldenChest", true, 8);
                            break;
                        case TileID.TeamBlockGreenPlatform:
                            GenUtils.PlaceDungeonChest(x, y, "DungeonWoodenChest", true, 6);
                            break;
                        case TileID.TeamBlockGreen:
                            GenUtils.PlaceDungeonChest(x, y, "DungeonWoodenChest", false, 6);
                            break;
                    }

                    progress.Set((y + x * Main.maxTilesY) / (float)(Main.maxTilesX * Main.maxTilesY));
                }
            }
        }
    }
}
