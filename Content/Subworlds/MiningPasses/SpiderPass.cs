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
    public class SpiderPass : GenPass
    {
        public SpiderPass(string name, double loadWeight) : base(name, loadWeight) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
           for (int i = 0; i < WorldGen.genRand.Next(3, 6); i++)
           {
                GenSpiderCave(0);
           }

           for (int j = 0; j < (Main.maxTilesX * Main.maxTilesY) * 0.06; j++)
           {
                int x = WorldGen.genRand.Next(20, Main.maxTilesX - 20);
                int y = WorldGen.genRand.Next(20, Main.maxTilesY - 300);

                if (!Main.tile[x, y].HasTile && Main.tile[x, y].WallType == WallID.SpiderUnsafe && GenUtils.AreaContainsSensitiveTiles(new List<int> { TileID.Stone, TileID.Sandstone, ModContent.TileType<SlateTile>() }, x, y, 4, 4))
                {
                    WorldGen.TileRunner(x, y, WorldGen.genRand.Next(4, 11), WorldGen.genRand.Next(2, 4), TileID.Cobweb, true, -1f, noYChange: false, overRide: false);
                }
           }
        }
        
        bool GenSpiderCave(byte iterations)
        {
            if (iterations >= 200)
                return false;

            int x = WorldGen.genRand.Next(100, Main.maxTilesX - 100);

            int y = WorldGen.genRand.Next(100, Main.maxTilesY - 500);

            if (new Vector2(x, y).Distance(new (Main.spawnTileX, Main.spawnTileY)) < 150 || Main.tile[x, y].HasTile || Main.tile[x, y].WallType == WallID.SpiderUnsafe)
            {
                iterations++;
                return GenSpiderCave(iterations);
            }

            WorldGen.Spread.Spider(x, y);
            return true;
        }

        void SpreadSpider(int x, int y)
        {
            if (!WorldGen.InWorld(x, y))
            {
                return;
            }
            byte wall = 62;
            List<Point> list = new List<Point>();
            List<Point> list2 = new List<Point>();
            HashSet<Point> hashSet = new HashSet<Point>();
            list2.Add(new Point(x, y));
            while (list2.Count > 0)
            {
                list.Clear();
                list.AddRange(list2);
                list2.Clear();
                while (list.Count > 0)
                {
                    Point item = list[0];
                    if (!WorldGen.InWorld(item.X, item.Y, 1))
                    {
                        list.Remove(item);
                        continue;
                    }
                    hashSet.Add(item);
                    list.Remove(item);
                    Tile tile = Main.tile[item.X, item.Y];
                    if (WorldGen.SolidTile(item.X, item.Y) || tile.WallType != 0)
                    {
                        if (tile.HasTile && tile.WallType == 0)
                        {
                            tile.WallType = wall;
                        }
                        continue;
                    }
                    tile.WallType = wall;
                    WorldGen.SquareWallFrame(item.X, item.Y);
                    if (!tile.HasTile)
                    {
                        tile.LiquidType = 0;
                        tile.LiquidAmount = 0;
                        if (WorldGen.SolidTile(item.X, item.Y + 1) && WorldGen.genRand.NextBool(3))
                        {
                           WorldGen.PlacePot(item.X, item.Y, 28, WorldGen.genRand.Next(19, 21));
                        }
                        if (!tile.HasTile)
                        {
                            if (WorldGen.SolidTile(item.X, item.Y - 1) && WorldGen.genRand.NextBool(3))
                            {
                                WorldGen.PlaceTight(item.X, item.Y, spiders: true);
                            }
                            else if (WorldGen.SolidTile(item.X, item.Y + 1))
                            {
                                WorldGen.PlaceTile(item.X, item.Y, 187, mute: true, forced: false, -1, 9 + WorldGen.genRand.Next(5));
                                if (WorldGen.genRand.NextBool(3))
                                {
                                    if (!tile.HasTile)
                                    {
                                        WorldGen.PlaceSmallPile(item.X, item.Y, 34 + WorldGen.genRand.Next(4), 1, 185);
                                    }
                                    if (!tile.HasTile)
                                    {
                                        WorldGen.PlaceSmallPile(item.X, item.Y, 48 + WorldGen.genRand.Next(6), 0, 185);
                                    }
                                }
                            }
                        }
                    }
                    Point item2 = new Point(item.X - 1, item.Y);
                    if (!hashSet.Contains(item2))
                    {
                        list2.Add(item2);
                    }
                    item2 = new Point(item.X + 1, item.Y);
                    if (!hashSet.Contains(item2))
                    {
                        list2.Add(item2);
                    }
                    item2 = new Point(item.X, item.Y - 1);
                    if (!hashSet.Contains(item2))
                    {
                        list2.Add(item2);
                    }
                    item2 = new Point(item.X, item.Y + 1);
                    if (!hashSet.Contains(item2))
                    {
                        list2.Add(item2);
                    }
                }
            }
        }
    }
}
