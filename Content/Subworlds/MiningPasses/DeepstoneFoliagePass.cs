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
    public class DeepstoneFoliagePass : GenPass
    {
        public DeepstoneFoliagePass(string name, double loadWeight) : base(name, loadWeight) { }

        public enum PlantType
        {
            Glowcap,
            SplitGlowcap
        }

        public int DeepstoneTile = ModContent.TileType<DeepstoneTile>();

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = Main.UnderworldLayer - 250; y < Main.maxTilesY; y++)
                {
                    int type = GetPlantToPlace(x, y);
                    if (type != -1 && Main.rand.NextBool(20))
                    {
                        WorldGen.PlaceTile(x, y - 1, type);
                        progress.Set((y + x * Main.maxTilesY) / (float)(Main.maxTilesX * Main.maxTilesY));
                    }
                }
            }
        }

        public int GetPlantToPlace(int x, int y)
        {
            Tile tile = Framing.GetTileSafely(x, y);
            Tile up = Framing.GetTileSafely(x, y - 1);
            Tile right = Framing.GetTileSafely(x + 1, y);
            Tile upRight = Framing.GetTileSafely(x + 1, y - 1);

            PlantType type = (PlantType)Main.rand.Next(2);

            if (type == PlantType.Glowcap)
            {
                if (!up.HasTile && tile.TileType == DeepstoneTile)
                {
                    return ModContent.TileType<Tiles.Environment.Foliage.GlimmercapTile>();
                }
            }
            else if (type == PlantType.SplitGlowcap)
            {
                if (!up.HasTile && !upRight.HasTile && tile.TileType == DeepstoneTile && right.TileType == DeepstoneTile)
                {
                    return ModContent.TileType<Tiles.Environment.Foliage.SplitGlimmercapTile>();
                }
            }

            return -1;
        }


    }
}
