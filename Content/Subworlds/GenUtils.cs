using Terraria.ID;
using Terraria;
using System.Security.Cryptography;
using Terraria.ModLoader;
using UltimateSkyblock.Content.Items.Placeable;
using UltimateSkyblock.Content.Tiles.Blocks;
using System.Collections.Generic;
using Terraria.WorldBuilding;
using System;
using UltimateSkyblock.Content.Utils;

namespace UltimateSkyblock.Content.Subworlds
{
    public static class GenUtils
    {
        public static Point16 ToPoint16(this Point point16) => new Point16(point16.X, point16.Y);
        public static Point ToPoint(this Point16 point16) => new Point(point16.X, point16.Y);

        public static void PlaceDungeonChest(int x, int y, string chest, bool platform, int chance = 1)
        {
            if (!platform)
                Main.tile[x, y].TileType = TileID.BlueDungeonBrick;
            else
                WorldGen.PlaceTile(x, y, TileID.Platforms, true, true, style: 11);

            if (WorldGen.genRand.NextBool(chance))
                Generator.GenerateStructure("Content/Subworlds/DungeonStructures/" + chest, new Point16(x, y - 2), UltimateSkyblock.Instance);
        }

        public static void ShelfRunner(int x, int y, int shelfType, int dir = 0, int failChance = 5)
        {
            if (shelfType == TileID.TeamBlockPinkPlatform)
            {
                if (WorldGen.genRand.NextBool(5) && failChance != 0)
                {
                    Main.tile[x, y].Clear(TileDataType.Tile);
                    return;
                }

                WorldGen.PlaceTile(x, y, TileID.Platforms, true, forced: true, style: 6);

                for (int i = 1; i < 20; i++)
                {
                    Tile tile = Framing.GetTileSafely(x + i * dir, y);
                    if (!tile.HasTile)
                    {
                        WorldGen.PlaceTile(x + i * dir, y, TileID.Platforms, true, style: 6);
                    }
                    else
                        break;
                }
            }
            else if (shelfType == TileID.TeamBlockRedPlatform)
            {
                if (WorldGen.genRand.NextBool(failChance) && failChance != 0)
                {
                    Main.tile[x, y].Clear(TileDataType.Tile);
                    return;
                }

                int left = WorldGen.genRand.Next(2, 4);
                int right = WorldGen.genRand.Next(2, 5);

                WorldGen.PlaceTile(x, y, TileID.Platforms, true, forced: true, style: 6);

                for (int i = -1; i > -left; i--)
                {
                    Tile tile = Framing.GetTileSafely(x + i, y);
                    if (!tile.HasTile)
                        WorldGen.PlaceTile(x + i, y, TileID.Platforms, true, style: 6);
                    else
                        break;
                }

                for (int i = 1; i < right; i++)
                {
                    Tile tile = Framing.GetTileSafely(x + i, y);
                    if (!tile.HasTile)
                        WorldGen.PlaceTile(x + i, y, TileID.Platforms, true, style: 6);
                    else
                        break;
                }
            }
            else if (shelfType == TileID.TeamBlockRed)
            {
                if (WorldGen.genRand.NextBool(failChance) && failChance != 0)
                {
                    Main.tile[x, y].Clear(TileDataType.Tile);
                    return;
                }

                int left = 20;
                int right = 20;

                WorldGen.PlaceTile(x, y, TileID.Platforms, true, forced: true, style: 6);

                for (int i = -1; i > -left; i--)
                {
                    Tile tile = Framing.GetTileSafely(x + i, y);
                    if (!tile.HasTile)
                        WorldGen.PlaceTile(x + i, y, TileID.Platforms, true, style: 6);
                    else
                        break;
                }

                for (int i = 1; i < right; i++)
                {
                    Tile tile = Framing.GetTileSafely(x + i, y);
                    if (!tile.HasTile)
                        WorldGen.PlaceTile(x + i, y, TileID.Platforms, true, style: 6);
                    else
                        break;
                }
            }
        }

        public static bool HasTileType(int x, int y, int type)
        {
            Tile up = Framing.GetTileSafely(x, y - 1);
            Tile down = Framing.GetTileSafely(x, y + 1);
            Tile left = Framing.GetTileSafely(x - 1, y);
            Tile right = Framing.GetTileSafely(x + 1, y);

            return up.TileType == type || down.TileType == type ||
                left.TileType == type || right.TileType == type;
        }

        public static bool TileHasAir(int x, int y)
        {
            Tile up = Framing.GetTileSafely(x, y - 1);
            Tile down = Framing.GetTileSafely(x, y + 1);
            Tile left = Framing.GetTileSafely(x - 1, y);
            Tile right = Framing.GetTileSafely(x + 1, y);

            if (!up.HasTile || !down.HasTile || !left.HasTile || !right.HasTile)
                return true;

            return false;
        }

        public static bool SuitableFor2x2(int x, int y)  => 
            Framing.GetTileSafely(x, y).Valid() && Framing.GetTileSafely(x + 1, y).Valid() &&
            !Framing.GetTileSafely(x, y - 1).HasTile && !Framing.GetTileSafely(x + 1, y - 1).HasTile &&
            !Framing.GetTileSafely(x + 1, y - 2).HasTile && !Framing.GetTileSafely(x + 2, y - 2).HasTile;


        public static void GetSurroundingTiles(int x, int y, out Tile tileLeft, out Tile tileRight, out Tile tileTop, out Tile tileBottom)
        {
            tileLeft = Framing.GetTileSafely(x - 1, y);
            tileRight = Framing.GetTileSafely(x + 1, y);
            tileTop = Framing.GetTileSafely(x, y - 1);
            tileBottom = Framing.GetTileSafely(x, y + 1);
        }

        public static void ScanAreaForAir(int width, int height, int x, int y, out int numAir, out int numSolid)
        {
            numAir = 0;
            numSolid = 0;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++) 
                {
                    Tile tile = Framing.GetTileSafely(x + i, y + j);
                    if (!tile.HasTile)
                    {
                        numAir++;
                    }
                    else
                    {
                        numSolid++;
                    }
                }
            }
        }

        public static bool MostlyAir(int width, int height, int i, int j)
        {
            int numAir = 0;
            int numSolid = 0;

            for (int x = i - width; x < i + width; x++)
            {
                for (int y = j - width; y < j + height; y++)
                {
                    if (WorldGen.InWorld(x, y))
                    {
                        if (Framing.GetTileSafely(x, y).HasTile)
                            numSolid++;
                        else
                            numAir++;
                    }
                }
            }

            return numAir > numSolid;
        }

        public static bool AreaContainsSensitiveTiles(List<int> tiles, int i, int j, int radiusWidth, int radiusHeight)
        {
            for (int x = i - radiusWidth; x < i + radiusWidth; x++)
            {
                for (int y = j - radiusHeight; y < j + radiusHeight; y++)
                {
                    if (WorldGen.InWorld(x, y))
                    {
                        Tile tile = Framing.GetTileSafely(x, y);
                        if (tiles.Contains(tile.TileType))
                            return true;
                    }
                }
            }

            return false;
        }

        public static bool AreaContainsSensitiveTile(int tile, int i, int j, int radiusWidth, int radiusHeight)
        {
            for (int x = i - radiusWidth; x < i + radiusWidth; x++)
            {
                for (int y = j - radiusHeight; y < j + radiusHeight; y++)
                {
                    if (WorldGen.InWorld(x, y))
                    {
                        Tile tileCheck = Framing.GetTileSafely(x, y);
                        if (tile == tileCheck.TileType)
                            return true;
                    }
                }
            }

            return false;
        }


        /// <summary>
        /// Loops through the world and generates the specified tile type with the given size/steps.
        /// </summary>
        /// <param name="genChance">Chance to generate tile patch. Returns 1 in X</param>
        /// <param name="type">Tile type to generate</param>
        /// <param name="minHeightRequirement">Minimum height at which tiles can be generated at</param>
        /// <param name="maxHeightRequirement">Maximum height at which tilec can be generated at. If  alone, defaults to Main.MaxTilesY</param>
        /// <remarks>Use to quickly generate ore and other patches of tiles.</remarks>
        public static void LoopWorldAndGenerateTiles(int genChance, int strength, int steps, int type, List<int> tilesThatCanBeGeneratedOn, int minHeightRequirement = 0, int maxHeightRequirement = 0)
        {
            for (int k = 0; k < (int)(Main.maxTilesX * Main.maxTilesY * 0.002); k++)
            {
                int x = Main.rand.Next(0, Main.maxTilesX);

                int y = Main.rand.Next(0, Main.maxTilesY);

                if (maxHeightRequirement == 0)
                {
                    maxHeightRequirement = Main.maxTilesY;
                }

                Tile tile = Framing.GetTileSafely(x, y);
                if (tile.HasTile && Main.rand.NextBool(genChance) && (y >= minHeightRequirement && y <= maxHeightRequirement) && tilesThatCanBeGeneratedOn.Contains(tile.TileType))
                {
                    WorldGen.TileRunner(x, y, strength, steps, type);
                }
            }
        }

        /// <summary>
        /// Simple function that takes in parameters for highly customizable "splotches" of a specific tile type. Allows for better depth calculations for thinning tile generation.
        /// </summary>
        /// <remarks></remarks>
        /// <param name="genChance"></param>
        /// <param name="type">Type of tile to generate</param>
        /// <param name="levelToDisperse">Y level where the generation starts thinning out. Also allows deepType to take over as the tile being generated.</param>
        /// <param name="thinningChance">Chance of the generation to thin out past the given levelToDisperse.</param>
        /// <param name="tilesThatCanBeGeneratedOn">Self explanatory. If all you want is stone, just pass in TileID.Stone</param>
        /// <param name="deepType">Type of tile to generate when deep enough. Defaults to the same tile as the given type.</param>
        /// <param name="tilePercent">% to multiply Main.maxTilesX * Main.maxTilesY by. Higher number = more iterations, lower = less.</param>
        public static void LoopWorldAndGenerateTilesWithDepthModifiers(int genChance, int strength, int steps, int type, List<int> tilesThatCanBeGeneratedOn, int levelToDisperse, bool canGenerateAfterLevel, int thinningChance = 100, int deepType = -1, float tilePercent = 0.002f)
        {
            for (int k = 0; k < (int)(Main.maxTilesX * Main.maxTilesY * tilePercent); k++)
            {
                int x = Main.rand.Next(0, Main.maxTilesX);

                int y = Main.rand.Next(0, Main.maxTilesY);

                Tile tile = Framing.GetTileSafely(x, y);
                int genChanceAtDepth = genChance;
                int finalGenType = type;

                if (y >= levelToDisperse)
                {
                    if (!canGenerateAfterLevel)
                        continue;

                    genChanceAtDepth += y / thinningChance;

                    if (deepType == -1)
                    {
                        finalGenType = type;
                    }
                    else
                    {
                        finalGenType = deepType;
                    }
                }

                // UltimateSkyblock.Instance.Logger.Info(genChanceAtDepth);

                if (tile.HasTile && tilesThatCanBeGeneratedOn.Contains(tile.TileType) && Main.rand.NextBool(genChanceAtDepth))
                {
                        WorldGen.TileRunner(x, y, strength, steps, finalGenType);
                }
            }
        }

        public static void SpreadToNearbyWall(int x, int y, int type, int failChance)
        {
            if (Main.rand.NextBool())
            {
                if (Main.rand.NextBool())
                {
                    x--;
                    y--;
                }
                else
                {
                    x++;
                    y--;
                }
            }
            else
            {
                if (Main.rand.NextBool())
                {
                    x++;
                    y++;
                }
                else
                {
                    x--;
                    y++;
                }
            }

            if (Main.rand.NextBool(failChance))
            {
                return;
            }
            else
            {
                failChance--;
                Tile tile = Framing.GetTileSafely(x, y);
                if (tile.WallType == WallID.Stone)
                {
                    tile.WallType = WallID.None;
                    WorldGen.PlaceWall(x, y, type, true);
                }
                SpreadToNearbyWall(x, y, WallID.Rocks1Echo, failChance);
            }
        }
    }
}
