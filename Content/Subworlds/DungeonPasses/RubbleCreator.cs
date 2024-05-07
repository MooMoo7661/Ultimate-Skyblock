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
    public class RubbleCreator : GenPass
    {
        public RubbleCreator(string name, double loadWeight) : base(name, loadWeight) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = 0; y  < Main.maxTilesY; y++)
                {
                    Tile tileUp = Framing.GetTileSafely(x, y - 1);
                    if ((!tileUp.HasTile && WorldGen.genRand.NextBool(7)) || (Main.tile[x, y].TileType == TileID.Platforms && WorldGen.genRand.NextBool(3)))
                    {
                        if (Main.tile[x, y].Slope == SlopeType.Solid && Main.tile[x, y].TileType != TileID.Spikes)
                        {
                            switch (WorldGen.genRand.Next(9))
                            {
                                default:
                                    WorldGen.PlaceSmallPile(x, y - 1, WorldGen.genRand.NextBool(2) ? WorldGen.genRand.Next(1, 6) : WorldGen.genRand.Next(12, 28), 0);
                                    break;

                                case 1 or 3 or 6:
                                    WorldGen.PlaceSmallPile(x, y - 1, WorldGen.genRand.NextBool() ? WorldGen.genRand.Next(6) : WorldGen.genRand.Next(6, 16), 1);
                                    break;

                                case 4:
                                    WorldGen.PlaceObject(x, y, TileID.LargePiles, true, WorldGen.genRand.Next(6));
                                    break;

                            }
                        }
                    }

                    progress.Set((y + x * Main.maxTilesY) / (float)(Main.maxTilesX * Main.maxTilesY));
                }
            }
        }
    }
}
