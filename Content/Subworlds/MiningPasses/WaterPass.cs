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
using static tModPorter.ProgressUpdate;

namespace UltimateSkyblock.Content.Subworlds.MiningPasses
{
    public class WaterPass : GenPass
    {
        public WaterPass(string name, double loadWeight) : base(name, loadWeight) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = 0; y < Main.maxTilesY - 500; y++)
                {
                    Tile tile = Framing.GetTileSafely(x, y);
                    if (!tile.HasTile && WorldGen.genRand.NextBool(500) && !GenUtils.MostlyAir(22, 22, x, y))
                    {
                        int xOffset = WorldGen.genRand.Next(12, 16);
                        int yOffset = WorldGen.genRand.Next(12, 16);

                        for (int x2 = x - xOffset; x2 < x + xOffset; x2++)
                        {
                            for (int y2 = y - yOffset; y2 < y + yOffset; y2++)
                            {
                                if (WorldGen.InWorld(x2, y2) && !Framing.GetTileSafely(x2, y2).HasTile)
                                    WorldGen.PlaceLiquid(x2, y2, (byte)LiquidID.Water, (byte)WorldGen.genRand.Next(150, 255));
                            }
                        }
                    }
                }
            }

            SettleLiquids(ref progress);
        }

        public void SettleLiquids(ref GenerationProgress progress)
        {
            Liquid.worldGenTilesIgnoreWater(ignoreSolids: true);
            Liquid.QuickWater(3);
            WorldGen.WaterCheck();
            int num538 = 0;
            Liquid.quickSettle = true;
            while (num538 < 10)
            {
                int num539 = Liquid.numLiquid + LiquidBuffer.numLiquidBuffer;
                num538++;
                float num540 = 0f;
                while (Liquid.numLiquid > 0)
                {
                    float num541 = (float)(num539 - (Liquid.numLiquid + LiquidBuffer.numLiquidBuffer)) / (float)num539;
                    if (Liquid.numLiquid + LiquidBuffer.numLiquidBuffer > num539)
                    {
                        num539 = Liquid.numLiquid + LiquidBuffer.numLiquidBuffer;
                    }
                    if (num541 > num540)
                    {
                        num540 = num541;
                    }
                    else
                    {
                        num541 = num540;
                    }
                    if (num538 == 1)
                    {
                        progress.Set(num541 / 3f + 0.33f);
                    }
                    int num542 = 10;
                    if (num538 > num542)
                    {
                        num542 = num538;
                    }
                    Liquid.UpdateLiquid();
                }
                WorldGen.WaterCheck();
            }
            Liquid.quickSettle = false;
            Liquid.worldGenTilesIgnoreWater(ignoreSolids: false);
            Main.tileSolid[484] = false;
        }
    }
}
