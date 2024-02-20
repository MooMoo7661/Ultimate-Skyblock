using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.WorldBuilding;
using Terraria.IO;
using Terraria.ID;
using UltimateSkyblock.Content.Configs;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace UltimateSkyblock.Content.Subworlds
{
    class BasicPerlinCaveWorldFeatureGenerator : GenPass
    {
        private static FastNoise caveNoise;
        private static int seed = 0;
        private static FastNoise.FractalType fractalType = FastNoise.FractalType.FBm;
        private static FastNoise.NoiseType noiseType = FastNoise.NoiseType.Perlin;

        private static SubworldConfig config = ModContent.GetInstance<SubworldConfig>();
        private static SubworldClientConfig clientConfig = ModContent.GetInstance<SubworldClientConfig>();

        public BasicPerlinCaveWorldFeatureGenerator(string name, double loadWeight) : base(name, loadWeight)
        {
            int noiseSeed = seed;
            caveNoise = new FastNoise(noiseSeed);
            UltimateSkyblock.Instance.Logger.Info("Noise Seed: " + noiseSeed);
            caveNoise.SetFrequency(0.02f); //0.02
            caveNoise.SetFractalOctaves(2); // 2
            caveNoise.SetFractalGain(1.6f); // 1.6f
            caveNoise.SetFractalLacunarity(1.6f); // 1.6f
            caveNoise.SetFractalWeightedStrength(1.3f); // 1.3f
            caveNoise.SetFractalWeightedStrength(1f); // 0
            //caveNoise.SetFractalPingPongStrength(4.9f);
            caveNoise.SetFractalType(fractalType); //fbm
            caveNoise.SetNoiseType(noiseType); //perlin

        }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            seed = new UnifiedRandom((int)DateTime.Now.Ticks).Next(1000) * (int)(((Main.time / 10 + 1) * (Main.GlobalTimeWrappedHourly / 25f) + 1) / 69);
            fractalType = (FastNoise.FractalType)config.FractalType;
            noiseType = (FastNoise.NoiseType)config.PerlinNoiseType;

            string message = "\n\n\n\n";
            if (clientConfig.ShowFastNoiseSeed)
                message += "Seed: " + seed + "\n";
            if (clientConfig.ShowFractalType)
                message += "Fractal Type: " + fractalType + "\n";
            if (clientConfig.ShowPerlinType)
                message += "Noise Type: " + noiseType + "\n";

            progress.Message = "Using Perlin Noise to generate caves" + message;
            int startingPositionX = Main.spawnTileX * Main.tile.Width;
            int startingPositionY = Main.spawnTileY * Main.tile.Height;
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    float noiseValue = (float)(caveNoise.GetNoise((startingPositionX + i), (startingPositionY + j)) * 0.5f);
                    if (noiseValue >= 0.0095f)
                    {
                        WorldGen.KillTile(i, j, noItem: true);
                    }

                    progress.Set((j + i * Main.maxTilesY) / (float)(Main.maxTilesX * Main.maxTilesY));
                }
            }
        }
    }
}
