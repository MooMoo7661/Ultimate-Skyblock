using UltimateSkyblock.Content.Configs;

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
            int noiseSeed = new UnifiedRandom().Next(int.MaxValue);
            seed = noiseSeed;
            caveNoise = new FastNoise(noiseSeed);
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
            UltimateSkyblock.Instance.Logger.Info("Noise Seed: " + seed);

            fractalType = (FastNoise.FractalType)config.FractalType;
            noiseType = (FastNoise.NoiseType)config.PerlinNoiseType;

            string message = "\n\n\n\n";
            if (clientConfig.ShowFastNoiseSeed)
                message += "Seed: " + seed + "\n";
            if (clientConfig.ShowFractalType)
                message += "Fractal Type: " + fractalType + "\n";
            if (clientConfig.ShowPerlinType)
                message += "Noise Type: " + noiseType + "\n";

            progress.Message = "Generating Noise Caves" + message;
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    float noiseValue = (float)(caveNoise.GetNoise(Main.spawnTileX * Main.tile.Width + i, Main.spawnTileY * Main.tile.Height + j) * 0.5f);
                    if (noiseValue >= 0.0095f)
                    {
                        Main.tile[i, j].Clear(TileDataType.All);
                    }

                    progress.Set((j + i * Main.maxTilesY) / (float)(Main.maxTilesX * Main.maxTilesY));
                }
            }
        }
    }
}
    