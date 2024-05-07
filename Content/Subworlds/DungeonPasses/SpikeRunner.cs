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
    public class SpikeRunner : GenPass
    {
        public SpikeRunner(string name, double loadWeight) : base(name, loadWeight) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = 0; y  < Main.maxTilesY; y++)
                {
                    if (WorldGen.genRand.NextBool(80) && Main.tile[x, y].HasTile && Main.tile[x, y].TileType == TileID.BlueDungeonBrick && !GenUtils.AreaContainsSensitiveTiles(new List<int> { TileID.OpenDoor, TileID.Spikes }, x, y, 8, 8))
                    {
                        GenUtils.GetSurroundingTiles(x, y, out Tile left, out Tile right, out Tile up, out Tile down);
                        if (!up.HasTile || !down.HasTile || !right.HasTile || !left.HasTile)
                            WorldGen.TileRunner(x, y, 6f, 4, TileID.Spikes, true, overRide: false);
                    }

                    progress.Set((y + x * Main.maxTilesY) / (float)(Main.maxTilesX * Main.maxTilesY));
                }
            }
        }
    }
}
