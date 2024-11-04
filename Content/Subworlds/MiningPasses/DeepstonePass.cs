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
            }

            //FastNoise dirtNoise = new FastNoise(FractalType.PingPong, NoiseType.Perlin, seed: new UnifiedRandom(DateTime.Now.Microsecond + Environment.TickCount).Next(1600));
            //dirtNoise.SetFractalPingPongStrength(2.5f);
            //dirtNoise.SetFractalOctaves(4);

            //for (int x = 0; x < Main.maxTilesX; x++)
            //{
            //    for (int y = Main.UnderworldLayer - 300; y < Main.UnderworldLayer; y++)
            //    {
            //        float noiseValue = (float)(dirtNoise.GetNoise(Main.spawnTileX * Main.tile.Width + x, Main.spawnTileY * Main.tile.Height + y) * 0.25f);
            //        if (Main.tile[x, y].TileType == ModContent.TileType<DeepstoneTile>() && noiseValue >= 0.01f)
            //        {
            //            Main.tile[x, y].TileType = (ushort)ModContent.TileType<DeepsoilTile>();
            //        }
            //    }
            //}

            FastNoise caveNoise = new FastNoise(FractalType.PingPong, NoiseType.OpenSimplex2S, seed: WorldGen.genRand.Next(69420));
            caveNoise.SetFractalPingPongStrength(2f);
            caveNoise.SetFractalOctaves(4);
  
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
