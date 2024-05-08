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
using static UltimateSkyblock.Content.Subworlds.FastNoise;

namespace UltimateSkyblock.Content.Subworlds.MiningPasses
{
    public class DeepstonePass : GenPass
    {
        public DeepstonePass(string name, double loadWeight) : base(name, loadWeight) { }   

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Generating Deepstone";
            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = Main.UnderworldLayer - 205; y < Main.UnderworldLayer - 200; y++)
                {
                    WorldGen.TileRunner(x, y + Main.rand.Next(-6, 6), Main.rand.Next(5, 12), Main.rand.Next(2, 5), MiningSubworld.Deepstone, true);
                    progress.Set((y + x * Main.maxTilesY) / (float)(Main.maxTilesX * Main.maxTilesY));
                }

                for (int y = Main.UnderworldLayer - 200; y < Main.maxTilesY; y++)
                {
                    if (Framing.GetTileSafely(x, y).TileType == TileID.Stone || Framing.GetTileSafely(x, y).TileType == ModContent.TileType<SlateTile>())
                    {
                        Tile tile = Main.tile[x, y];
                        tile.TileType = (ushort)ModContent.TileType<DeepstoneTile>();
                    }
                    progress.Set((y + x * Main.maxTilesY) / (float)(Main.maxTilesX * Main.maxTilesY));
                }

                for (int y = Main.UnderworldLayer - 200; y < Main.maxTilesY; y++)
                {
                    if (WorldGen.genRand.NextBool(100) && GenUtils.TileHasAir(x, y) && !GenUtils.AreaContainsSensitiveTiles(new List<int> { ModContent.TileType<DeepsoilTile>() }, x, y, 15, 15) && Main.tile[x, y].TileType == ModContent.TileType<DeepstoneTile>())
                        WorldGen.TileRunner(x, y, WorldGen.genRand.Next(12, 16), WorldGen.genRand.Next(7, 28), ModContent.TileType<DeepsoilTile>());
                }
            }

            FastNoise caveNoise = new FastNoise(FractalType.PingPong, NoiseType.OpenSimplex2S, seed:new UnifiedRandom().Next(400));
  
            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = Main.UnderworldLayer - 300; y < Main.UnderworldLayer; y++)
                {
                    float noiseValue = (float)(caveNoise.GetNoise(Main.spawnTileX * Main.tile.Width + x, Main.spawnTileY * Main.tile.Height + y) * 0.25f);
                    if (Main.tile[x, y].TileType == ModContent.TileType<DeepstoneTile>() && noiseValue >= 0.0095f)
                    {
                        Main.tile[x, y].TileType = (ushort)ModContent.TileType<HardenedDeepstoneTile>();
                    }
                }
            }
        }

    }
}
