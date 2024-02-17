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
    class BasicCellularCaveWorldFeatureGenerator : GenPass
    {
        private static FastNoise caveNoise;

        public BasicCellularCaveWorldFeatureGenerator(string name, double loadWeight) : base(name, loadWeight)
        {
            caveNoise = new FastNoise(Main.rand.Next(1000));
            caveNoise.SetFrequency(0.023f);
            caveNoise.SetFractalOctaves(1);
            caveNoise.SetFractalGain(2f);
            caveNoise.SetFractalLacunarity(0f);
            caveNoise.SetFractalWeightedStrength(0.5f);
            caveNoise.SetCellularDistanceFunction(FastNoise.CellularDistanceFunction.Hybrid);
            caveNoise.SetCellularReturnType(FastNoise.CellularReturnType.CellValue);
            caveNoise.SetCellularJitter(1.4f);
            //caveNoise.SetFractalPingPongStrength(4.9f);

            caveNoise.SetFractalType(FastNoise.FractalType.FBm);
            caveNoise.SetNoiseType(FastNoise.NoiseType.Cellular);
        }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            int startingPositionX = Main.spawnTileX * Main.tile.Width;
            int startingPositionY = Main.spawnTileY * Main.tile.Height;
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    float noiseValue = (float)(caveNoise.GetNoise(startingPositionX + i, startingPositionY + j) * 0.6f);
                    if (noiseValue >= 0.0095f)
                    {
                        WorldGen.KillTile(i, j, noItem: true);
                    }
                }
            }
        }
    }
}
