using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.WorldBuilding;
using Terraria.IO;
using Terraria.ID;

namespace UltimateSkyblock.Content.Subworlds
{
    class BasicPerlinCaveWorldFeatureGenerator : GenPass
    {
        private static FastNoise caveNoise;

        public BasicPerlinCaveWorldFeatureGenerator(string name, double loadWeight) : base(name, loadWeight)
        {
            int noiseSeed = Main.rand.Next(10000);
            caveNoise = new FastNoise(noiseSeed);
            UltimateSkyblock.Instance.Logger.Info("Noise Seed: " + noiseSeed);
            caveNoise.SetFrequency(0.02f); //0.02
            caveNoise.SetFractalOctaves(2); // 2
            caveNoise.SetFractalGain(1.6f); // 1.6f
            caveNoise.SetFractalLacunarity(1.6f); // 1.6f
            caveNoise.SetFractalWeightedStrength(1.3f); // 1.3f
            caveNoise.SetFractalWeightedStrength(1f); // 0
            //caveNoise.SetFractalPingPongStrength(4.9f);
            caveNoise.SetFractalType(FastNoise.FractalType.FBm); //fbm
            caveNoise.SetNoiseType(FastNoise.NoiseType.Perlin); //perlin
            
        }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
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
                }
            }
        }
    }
}
