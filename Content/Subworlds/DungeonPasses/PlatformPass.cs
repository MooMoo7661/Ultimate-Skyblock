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
    public class PlatformPass : GenPass
    {
        public PlatformPass(string name, double loadWeight) : base(name, loadWeight) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = 0; y  < Main.maxTilesY; y++)
                {
                    switch(Main.tile[x, y].TileType)
                    {
                        case TileID.TeamBlockRedPlatform:
                            GenUtils.ShelfRunner(x, y, TileID.TeamBlockRedPlatform);
                            break;
                        case TileID.TeamBlockBluePlatform:
                            GenUtils.ShelfRunner(x, y, TileID.TeamBlockRedPlatform, failChance: 2);
                            break;
                        case TileID.TeamBlockRed:
                            GenUtils.ShelfRunner(x, y, TileID.TeamBlockRed, 0);
                            break;
                    }

                    progress.Set((y + x * Main.maxTilesY) / (float)(Main.maxTilesX * Main.maxTilesY));
                }
            }
        }
    }
}
