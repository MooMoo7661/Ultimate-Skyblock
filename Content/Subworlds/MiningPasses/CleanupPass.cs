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
using UltimateSkyblock.Content.Tiles.Walls;

namespace UltimateSkyblock.Content.Subworlds.MiningPasses
{
    public class CleanupPass : GenPass
    {
        public CleanupPass(string name, double loadWeight) : base(name, loadWeight) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Final Cleanup";
            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = 0; y < Main.maxTilesY; y++)
                {
                    Tile tile = Main.tile[x, y];

                    //Removing any extra water or lava droplets that may have generated on sloped tiles.
                    if (tile.HasTile && tile.Slope != SlopeType.Solid && (Framing.GetTileSafely(x, y + 1).TileType == TileID.WaterDrip || Framing.GetTileSafely(x, y + 1).TileType == TileID.LavaDrip))
                    {
                        Framing.GetTileSafely(x, y + 1).ClearTile();
                    }

                    //Temporary replacement of obsidian brick walls until I rework the deepstone castles
                    if (tile.WallType == WallID.AncientObsidianBrickWall || tile.WallType == WallID.ObsidianBackEcho)
                    {
                        Main.tile[x, y].WallType = (ushort)ModContent.WallType<DeepstoneBrickWallTile>();
                    }

                    if (tile.TileType == TileID.AshGrass)
                    {
                        GenUtils.GetSurroundingTiles(x, y, out Tile left, out Tile right, out Tile top, out Tile bottom);
                        if (!left.HasTile && !right.HasTile && !top.HasTile && !bottom.HasTile)
                        {
                            Main.tile[x, y].Clear(TileDataType.Tile);
                        }
                    }

                    progress.Set((y + x * Main.maxTilesY) / (float)(Main.maxTilesX * Main.maxTilesY));
                }
            }

            WaterPass.SettleLiquids(ref progress);
        }
    }
}
