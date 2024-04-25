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

namespace UltimateSkyblock.Content.Subworlds.MiningPasses
{
    public class CaveDecorationsPass : GenPass
    {
        public CaveDecorationsPass(string name, double loadWeight) : base(name, loadWeight) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            for (int x = 50; x < Main.maxTilesX - 50; x++)
            {
                for (int y = 50; y < Main.maxTilesY - 50; y++)
                {
                    Tile tile = Framing.GetTileSafely(x, y);
                    Tile up = Framing.GetTileSafely(x, y - 1);
                    Tile upRight = Framing.GetTileSafely(x + 1, y - 1);
                    Tile right = Framing.GetTileSafely(x + 1, y);

                    if (WorldGen.genRand.NextBool(6))
                    {
                        // Pots & rubble
                        if (tile.HasTile && !up.HasTile && tile.Slope == SlopeType.Solid)
                        {
                            if (right.HasTile && WorldGen.genRand.NextBool(3) && GenUtils.SuitableFor2x2(x, y))
                                MediumRubbleMaker(x, y);
                            else if (!up.HasTile && tile.Slope == SlopeType.Solid)
                                SmallRubbleMaker(x, y);

                            if (right.HasTile && right.Slope == SlopeType.Solid && !upRight.HasTile && !Framing.GetTileSafely(x, y - 2).HasTile && !Framing.GetTileSafely(x + 1, y - 2).HasTile)
                                PotMaker(x, y);
                        }
                    }

                    if (y < Main.UnderworldLayer && WorldGen.genRand.NextBool(50) && GenUtils.SuitableFor2x2(x, y) && !GenUtils.AreaContainsSensitiveTiles(new List<int> { TileID.Heart }, x, y, 8, 8))
                    {
                        WorldGen.PlaceObject(x, y - 1, TileID.Heart, true);
                    }

                    progress.Set((y + x * Main.maxTilesY) / (float)(Main.maxTilesX * Main.maxTilesY));
                }
            }
        }
        public static bool PotMaker(int x, int y)
        {
            int type = WorldGen.genRand.Next(4);
            int tileType = Framing.GetTileSafely(x, y).TileType;

            if (y > Main.UnderworldLayer - 200)
                type = WorldGen.genRand.Next(13, 16);
            if (tileType == TileID.Marble)
                type = WorldGen.genRand.Next(31, 34);

            return WorldGen.PlacePot(x, y - 1, style: type);
        }

        public static bool SmallRubbleMaker(int x, int y)
        {
            int type = Framing.GetTileSafely(x, y).TileType; // This helps me type less cuz I'm lazy
            int rubbleType = -1;

            if (type == TileID.Dirt)
            {
                rubbleType = WorldGen.genRand.Next(7) switch
                {
                    1 => 6,
                    2 => 7,
                    3 => 8,
                    4 => 9,
                    5 => 10,
                    6 => 11,
                    _ => 5,
                };
            }
            else if (type == TileID.Stone || type == ModContent.TileType<SlateTile>())
            {
                rubbleType = WorldGen.genRand.Next(5) switch
                {
                    1 => 2,
                    2 => 3,
                    3 => 4,
                    4 => 5,
                    _ => 1,
                };
            }
            else if (type == TileID.Sandstone)
            {
                rubbleType = WorldGen.genRand.Next(6) switch
                {
                    1 => 55,
                    2 => 56,
                    3 => 57,
                    4 => 58,
                    5 => 59,
                    _ => 54,
                };
            }
            else if (type == TileID.Sand)
            {
                rubbleType = WorldGen.genRand.Next(9) switch
                {
                    1 => 67,
                    2 => 66,
                    3 => 69,
                    4 => 70,
                    5 => 71,
                    _ => 68,
                };
            }
            else if (type == TileID.Marble)
            {
                rubbleType = WorldGen.genRand.Next(4) switch
                {
                    1 => 74,
                    2 => 75,
                    3 => 76,
                    _ => 73,
                };
            }

            if (rubbleType == -1)
                return false;

            return WorldGen.PlaceSmallPile(x, y - 1, rubbleType, 0);
        }

        public static bool MediumRubbleMaker(int x, int y)
        {
            int type = Framing.GetTileSafely(x, y).TileType; // This helps me type less cuz I'm lazy
            int rubbleType = -1;

            if (type == TileID.Stone)
            {
                rubbleType = WorldGen.genRand.Next(6) switch
                {
                    1 => 1,
                    2 => 2,
                    3 => 3,
                    4 => 4,
                    5 => 5,
                    _ => 0,
                };

                if (WorldGen.genRand.NextBool(20))
                {
                    rubbleType = WorldGen.genRand.Next(12) switch
                    {
                        1 or 2 or 3 => 19, // Amethyst
                        4 or 5 or 6 => 20, // Topaz
                        7 or 8 => 21, // Sapphire
                        9 or 10 => 23, // Ruby
                        11 => 22, // Emerald
                        _ => 24 // Diamond
                    };
                }

                if (WorldGen.genRand.NextBool(40))
                {
                    rubbleType = WorldGen.genRand.Next(3) switch
                    {
                        1 => 17,
                        2 => 18,
                        _ => 16
                    };
                }
            }
            else if (type == TileID.Dirt)
            {
                rubbleType = WorldGen.genRand.Next(6) switch
                {
                    1 => 1,
                    2 => 2,
                    3 => 3,
                    4 => 4,
                    5 => 5,
                    _ => 0,
                };
            }

            if (rubbleType == -1)
                return false;

            return WorldGen.PlaceSmallPile(x, y - 1, rubbleType, 1);
        }
    }
}
