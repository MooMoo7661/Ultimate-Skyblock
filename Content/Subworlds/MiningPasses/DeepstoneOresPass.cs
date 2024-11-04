using System;
using UltimateSkyblock.Content.Subworlds.DungeonRoomUtils;
using UltimateSkyblock.Content.Tiles.Blocks;
using UltimateSkyblock.Content.Utils;
using static UltimateSkyblock.Content.Subworlds.FastNoise;

namespace UltimateSkyblock.Content.Subworlds.MiningPasses
{
    public class DeepstoneOresPass : GenPass
    {
        public DeepstoneOresPass(string name, double loadWeight) : base(name, loadWeight) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            FastNoise caveNoise = new FastNoise(FractalType.PingPong, NoiseType.Perlin, seed: WorldGen.genRand.Next(1600));
            caveNoise.SetFractalPingPongStrength(3f);
            caveNoise.SetFractalOctaves(1);

            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = Main.UnderworldLayer - 300; y < Main.UnderworldLayer; y++)
                {
                    float noiseValue = (float)(caveNoise.GetNoise(Main.spawnTileX * Main.tile.Width + x, Main.spawnTileY * Main.tile.Height + y) * 0.17f);
                    if ((Main.tile[x, y].TileType == ModContent.TileType<DeepstoneTile>() || Main.tile[x, y].TileType == ModContent.TileType<HardenedDeepstoneTile>()) && noiseValue >= 0.137f) // 0.15 for cool effect
                    {
                        Main.tile[x, y].TileType = (ushort)ModContent.TileType<HardenedDeepstoneTile>();
                    }
                }
            }

            for (int k = 0; k < (int)(Main.maxTilesX * Main.maxTilesY * 0.002); k++)
            {
                int x = WorldGen.genRand.Next(Main.maxTilesX);
                int y = WorldGen.genRand.Next(Main.UnderworldLayer - 250, Main.UnderworldLayer);

                float depth = Math.Clamp(y - (Main.UnderworldLayer - 250), 1, Main.maxTilesY);
                depth /= 90f;

                if (WorldGen.genRand.NextBool(4 + (int)(30 / depth)) && depth > 1.9f)
                WorldGen.TileRunner(x, y, 3f + depth,(int)(3 * depth), WorldGen.SavedOreTiers.Adamantite);
            }

            for (int k = 0; k < (int)(Main.maxTilesX * Main.maxTilesY * 0.002); k++)
            {
                int x = WorldGen.genRand.Next(Main.maxTilesX);
                int y = WorldGen.genRand.Next(Main.UnderworldLayer - 250, Main.UnderworldLayer);

                if (WorldGen.genRand.NextBool(40))
                    WorldGen.TileRunner(x, y, 4f, 6, WorldGen.SavedOreTiers.Mythril);
            }

            for (int k = 0; k < (int)(Main.maxTilesX * Main.maxTilesY * 0.002); k++)
            {
                int x = WorldGen.genRand.Next(Main.maxTilesX);
                int y = WorldGen.genRand.Next(Main.UnderworldLayer - 200, Main.UnderworldLayer);

                float depth = Math.Clamp(Main.UnderworldLayer - y, 1, Main.maxTilesY);
                depth /= 80f;

                if (WorldGen.genRand.NextBool((int)(2 + (30 / depth))))
                    WorldGen.TileRunner(x, y, 3.3f + depth, (int)(5 * depth), WorldGen.SavedOreTiers.Cobalt);
            }
        }
    }
}
