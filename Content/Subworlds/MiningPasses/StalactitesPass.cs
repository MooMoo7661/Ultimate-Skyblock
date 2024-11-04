﻿using System;
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

namespace UltimateSkyblock.Content.Subworlds.MiningPasses
{
    public class StalactitesPass : GenPass
    {
        public StalactitesPass(string name, double loadWeight) : base(name, loadWeight) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            UltimateSkyblock.Instance.Logger.Info("Placing Stalactites");
            progress.Message = "Placing Stalactites";
            for (int x = 100; x < Main.maxTilesX - 100; x++)
            {
                for (int y = 100; y < Main.maxTilesY - 100; y++)
                {
                    Tile tile = Framing.GetTileSafely(x, y);
                    if (!tile.HasTile && tile.Slope == SlopeType.Solid && Framing.GetTileSafely(x, y - 1).HasTile && WorldGen.genRand.NextBool(5))
                    {
                        WorldGen.PlaceUncheckedStalactite(x, y, WorldGen.genRand.NextBool(), WorldGen.genRand.Next(3), false);
                    }

                    if (tile.HasTile && !Framing.GetTileSafely(x, y - 1).HasTile && !Framing.GetTileSafely(x, y - 2).HasTile && WorldGen.genRand.NextBool(8) && tile.Slope == SlopeType.Solid)
                    {
                        WorldGen.PlaceUncheckedStalactite(x, y - 1, WorldGen.genRand.NextBool(), WorldGen.genRand.Next(3), false);
                    }

                    progress.Set((y + x * Main.maxTilesY) / (float)(Main.maxTilesX * Main.maxTilesY));
                }
            }
        }
    }
}
