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
    public class PotMaker : GenPass
    {
        public PotMaker(string name, double loadWeight) : base(name, loadWeight) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = 0; y  < Main.maxTilesY; y++)
                {
                    Tile tileUp = Framing.GetTileSafely(x, y - 1);
                    if (Main.tile[x, y].TileType == TileID.Spikes)
                        continue;

                    if ((!tileUp.HasTile && WorldGen.genRand.NextBool(18)) || (Main.tile[x, y].TileType == TileID.Platforms && WorldGen.genRand.NextBool(3)) && !GenUtils.AreaContainsSensitiveTiles(new List<int>{ TileID.Pots, TileID.ClosedDoor }, x, y, 3, 3))
                        WorldGen.PlacePot(x, y, style: !WorldGen.genRand.NextBool(4) ? WorldGen.genRand.Next(10, 13) : WorldGen.genRand.Next(4));

                    progress.Set((y + x * Main.maxTilesY) / (float)(Main.maxTilesX * Main.maxTilesY));
                }
            }
        }
    }
}
