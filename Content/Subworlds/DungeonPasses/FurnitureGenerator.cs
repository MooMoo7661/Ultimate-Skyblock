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
    public class FurnitureGenerator : GenPass
    {
        public FurnitureGenerator(string name, double loadWeight) : base(name, loadWeight) { }

        static int[] previousFurniture = new int[] { 0, 0 };

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = 0; y  < Main.maxTilesY; y++)
                {
                    Tile tileUp = Framing.GetTileSafely(x, y - 1);
                    if (WorldGen.genRand.NextBool(20) && !tileUp.HasTile && Main.tile[x, y].HasTile && Main.tile[x, y].TileType != TileID.Platforms && Main.tile[x, y].TileType != TileID.Spikes)
                    {
                        int?[] tileType = GetFurniture(x, y);
                        if (tileType != null)
                            WorldGen.PlaceObject(x, y - 1, tileType[0].Value, true, tileType[1].Value);
                    }

                    progress.Set((y + x * Main.maxTilesY) / (float)(Main.maxTilesX * Main.maxTilesY));
                }
            }
        }

        // The returned int[0] is the TileID, and int[1] is the subID of the tile.
        // Used to roll furniture with specific subIDs
        public static int?[] GetFurniture(int x, int y, ushort iterations = 0)
        {
            GenLogger.QuickLog("Rolling Furniture");
            if (iterations >= 20)
                return null;

            int[] tileType = new UnifiedRandom(DateTime.Now.Millisecond + iterations + 1).Next(24) switch
            {
                1 or 2 => new int[2] { TileID.GrandfatherClocks, 30 },
                3 or 4 => new int[2] { TileID.Pianos, 11 },
                5 or 6 => new int[2] { TileID.Dressers, 5 },
                7 or 8 or 9 => new int[2] { TileID.Benches, 6 },
                10 or 11 => new int[2] { TileID.Sinks, 10 },
                12 or 13 or 14 or 15 => new int[2] { TileID.Statues, 46 },
                16 or 17 => new int[2] { TileID.WorkBenches, 11 },
                18 or 19 or 20 or 21 => new int[2] { TileID.Lamps, 24 },
                _ => new int[2] { TileID.Bookcases, 1 },
            };

            if (tileType == previousFurniture || GenUtils.AreaContainsSensitiveTiles(new List<int> { tileType[0] }, x, y, 8, 8))
                return GetFurniture(x, y, iterations += 1);

            previousFurniture = tileType;
            return new int?[] { tileType[0], tileType[1] };
        }
    }
}
