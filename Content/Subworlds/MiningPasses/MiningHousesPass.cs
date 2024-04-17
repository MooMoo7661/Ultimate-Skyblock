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
using UltimateSkyblock.Content.Tiles.Blocks;
using System.IO;
using Terraria.ModLoader.IO;
using Terraria.ModLoader;

namespace UltimateSkyblock.Content.Subworlds.MiningPasses
{
    public class MiningHousesPass : GenPass
    {
        public MiningHousesPass(string name, double loadWeight) : base(name, loadWeight) { }

        public List<Rectangle> houses = new List<Rectangle>();

        public static List<int> restrictedTiles = new List<int>
            {
                TileID.Marble,
                TileID.Heart,
                TileID.Containers,
                TileID.WoodBlock,
                TileID.WoodenBeam,
                TileID.Platforms,
                TileID.GrayBrick,
                ModContent.TileType<DeepstoneBrickTile>()
            };

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            for (int q = 0; q < 8; q++)
            {
                Point16 pos = RollHousePoint(0);
                if (pos == Point16.Zero)
                    continue;                

                Generator.GenerateMultistructureRandom("Content/Subworlds/SubStructures/MiningHouses", pos, UltimateSkyblock.Instance);
            }
        }

        Point16 RollHousePoint(byte iterations)
        {
            if (iterations >= 100)
                return Point16.Zero;

            int x = WorldGen.genRand.Next(100, Main.maxTilesX - 100);

            int y = WorldGen.genRand.Next(100, Main.maxTilesY - 500);

            if (!Framing.GetTileSafely(x, y).HasTile || GenUtils.MostlyAir(45, 45, x, y) || GenUtils.AreaContainsSensitiveTiles(restrictedTiles, x, y, 60, 60))
            {
                iterations++;
                return RollHousePoint(iterations);
            }

            return new(x, y);
        }
    }
}
