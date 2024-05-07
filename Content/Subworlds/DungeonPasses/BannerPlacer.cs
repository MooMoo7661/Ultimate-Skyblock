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
using UltimateSkyblock.Content.Subworlds.DungeonRoomUtils;
using System.Collections;

namespace UltimateSkyblock.Content.Subworlds.DungeonPasses
{
    public class BannerPlacer : GenPass
    {
        public BannerPlacer(string name, double loadWeight) : base(name, loadWeight) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = 0; y < Main.maxTilesY; y++)
                {
                    Tile tile = Main.tile[x, y];
                    if (WorldGen.genRand.NextBool(40) && tile.HasTile && tile.TileType == TileID.BlueDungeonBrick)
                    {
                        if (!Framing.GetTileSafely(x, y + 1).HasTile)
                        {
                            WorldGen.PlaceBanner(x, y + 1, TileID.Banners, WorldGen.genRand.NextBool() ? 12 : 13);
                        }
                    }

                    progress.Set((y + x * Main.maxTilesY) / (float)(Main.maxTilesX * Main.maxTilesY));
                }
            }
        }
    }
}
