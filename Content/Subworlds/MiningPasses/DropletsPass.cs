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
using Terraria.ModLoader;
using UltimateSkyblock.Content.Tiles.Blocks;

namespace UltimateSkyblock.Content.Subworlds.MiningPasses
{
    public class DropletsPass : GenPass
    {
        public DropletsPass(string name, double loadWeight) : base(name, loadWeight) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            for (int x = 45; x < Main.maxTilesX - 45; x++)
            {
                for (int y = 45; y < Main.maxTilesY - 45; y++)
                {
                    Tile tile = Framing.GetTileSafely(x, y);
                    if (tile.HasTile && !Framing.GetTileSafely(x, y + 1).HasTile)
                    {
                        if (tile.TileType == ModContent.TileType<SlateTile>() && Main.rand.NextBool(30))
                        {
                            WorldGen.PlaceTile(x, y + 1, TileID.WaterDrip, true);
                        }
                        else if (tile.TileType == ModContent.TileType<DeepstoneTile>() && Main.rand.NextBool(17))
                        {
                            WorldGen.PlaceTile(x, y + 1, y >= Main.UnderworldLayer ? TileID.LavaDrip : (Main.rand.NextBool() ? TileID.WaterDrip : TileID.LavaDrip), true);
                        }
                    }
                }
            }
        }
    }
}
