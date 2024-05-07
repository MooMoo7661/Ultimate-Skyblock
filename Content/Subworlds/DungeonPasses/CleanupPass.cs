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

namespace UltimateSkyblock.Content.Subworlds.DungeonPasses
{
    public class CleanupPass : GenPass
    {
        public CleanupPass(string name, double loadWeight) : base(name, loadWeight) { }


        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = 0; y < Main.maxTilesY; y++)
                {
                    Tile tile = Main.tile[x, y];
                    if (tile.TileType == TileID.TeamBlockWhitePlatform)
                    {
                        tile.HasTile = false;
                        tile.Clear(TileDataType.Tile);
                        if (!WorldGen.genRand.NextBool(3))
                            WorldGen.PlaceTile(x, y, TileID.ClosedDoor, true, style: 16);
                    }
                    else if (tile.TileType == TileID.TeamBlockBlue)
                    {
                        tile.HasTile = false;
                        tile.Clear(TileDataType.Tile);
                    }
                }
            }
        }
    }
}
