using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubworldLibrary;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.IO;
using Terraria.WorldBuilding;
using UltimateSkyblock.Content.Subworlds;
using static UltimateSkyblock.Content.Subworlds.GenUtils;
using Terraria.ModLoader;
using UltimateSkyblock.Content.Items.Placeable;
using UltimateSkyblock.Content.Tiles.Blocks;

namespace UltimateSkyblock.Content.Subworlds
{
    public class MiningSubworld : Subworld
    {
        public override int Width => 1000;
        public override int Height => 1000;
        public override bool ShouldSave => false;
        public override string Name => "Mining Subworld";

        public static Point spawn = new Point(Main.spawnTileX, Main.spawnTileY);
        public static int Slate = ModContent.TileType<SlateTile>();

        public override void OnEnter()
        {
            SubworldSystem.hideUnderworld = true;
        }

        public override void OnLoad()
        {
            Main.worldSurface = 0;
            GenVars.worldSurfaceHigh = 0;
            GenVars.worldSurfaceLow = 0;
            GenVars.rockLayer = Main.maxTilesY / 2;
            GenVars.oceanWaterStartRandomMin = 0;
            GenVars.oceanWaterStartRandomMax = 0;
        }

        public override List<GenPass> Tasks => new()
        {
            new InitialEarthPass("Filling the world - Prepping for cave generation", 1),
            new BasicPerlinCaveWorldFeatureGenerator("Generating Basic Perlin Caves", 1),
            //new BasicCellularCaveWorldFeatureGenerator("Generating Cellular Perlin Caves", 1),
            new EarthPass("Generating the Earth", 1),
            new SpawnPass("Setting up Spawn", 0.5f)
        };

        public class InitialEarthPass : GenPass
        {
            public InitialEarthPass(string name, double loadWeight) : base(name, loadWeight) { }

            protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
            {
                progress.Message = "Filling the world";
                FillWorldWithStone();
                SetGenVars();

            }

            private static void SetGenVars()
            {
                SubVars.copper = Main.rand.NextBool() ? TileID.Copper : TileID.Tin;
                SubVars.iron = Main.rand.NextBool() ? TileID.Iron : TileID.Lead;
                SubVars.silver = Main.rand.NextBool() ? TileID.Silver : TileID.Tungsten;
                SubVars.gold = Main.rand.NextBool() ? TileID.Gold : TileID.Platinum;

                SubVars.cobalt = Main.rand.NextBool() ? TileID.Cobalt : TileID.Palladium;
                SubVars.mythril = Main.rand.NextBool() ? TileID.Mythril : TileID.Orichalcum;
                SubVars.adamantite = Main.rand.NextBool() ? TileID.Adamantite : TileID.Titanium;
            }

            private static void FillWorldWithStone()
            {
                for (int x = 0; x < Main.maxTilesX; x++)
                {
                    for (int y = 0; y < Main.maxTilesY; y++)
                    {
                        if (WorldGen.InWorld(x, y))
                            WorldGen.PlaceTile(x, y, TileID.Stone, true);
                    }
                }
            }
        }

        public class SpawnPass : GenPass
        {
            public SpawnPass(string name, double loadWeight) : base(name, loadWeight) { }

            protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
            {
                progress.Message = "Setting up spawn";
                Main.spawnTileY = Main.maxTilesY / 4;
                GenerateSpawnBubble();
                TextureStonyWalls();
                LightUpSpawn();
                PlaceTents();
            }

            private static void TextureStonyWalls()
            {
                for (int x = 0; x < Main.maxTilesX; x++)
                {
                    for (int y = 0; y < Main.maxTilesY; y++)
                    {
                        if (Framing.GetTileSafely(x, y).WallType == WallID.Stone)
                        {
                            if (WorldGen.genRand.NextBool(5))
                            {
                                WorldGen.PlaceWall(x, y, WallID.Rocks1Echo);
                                if (Main.rand.NextBool(2))
                                {
                                    SpreadToNearbyWall(x, y, WallID.Rocks1Echo, 10);
                                }
                            }
                        }
                    }
                }
            }

            private static void GenerateSpawnBubble()
            {
                ShapeData shapeData1 = new ShapeData();
                ShapeData shapeData2 = new ShapeData();
                Point placePoint = new Point(Main.spawnTileX, Main.spawnTileY);

                WorldUtils.Gen(new(placePoint.X, placePoint.Y), new Shapes.Circle(40), new Actions.Blank().Output(shapeData1));
                WorldUtils.Gen(new(placePoint.X, placePoint.Y), new ModShapes.All(shapeData1), new Actions.SetTile(TileID.Stone));

                WorldUtils.Gen(placePoint, new Shapes.HalfCircle(28), new Actions.ClearTile());
                WorldUtils.Gen(placePoint, new Shapes.HalfCircle(28), new Actions.PlaceWall(WallID.Stone));
                WorldUtils.Gen(placePoint, new Shapes.HalfCircle(28), new Actions.Blank().Output(shapeData2));
                WorldUtils.Gen(placePoint, new ModShapes.OuterOutline(shapeData2, true), new Actions.SetTile(TileID.GreenMoss));
            }

            private static void LightUpSpawn()
            {
                for (int i = -28; i < 28; i++)
                {
                    if (i % 5 == 0)
                        WorldGen.PlaceTile(spawn.X - i, spawn.Y - 3, TileID.Torches);
                }
            }

            private static void PlaceTents()
            {
                
            }
        }

        public class EarthPass : GenPass
        {
            public EarthPass(string name, double loadWeight) : base(name, loadWeight) { }

            protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
            {
                progress.Message = "Generating the Earth";

                CreateHellBarrier();
                GenerateDeepstone();
                GenerateHell();
                RunOreGen();
                GenerateGeodes();
                CreateSlateDeposits();
                SmoothWorld();
                PlaceStalactites();
                FinalCleanup();
            }

            public static void GenerateDeepstone()
            {

            }

            public static void FinalCleanup()
            {
                for (int x = 0; x < Main.maxTilesX; x++)
                {
                    for (int y = 0; y < Main.maxTilesY; y++)
                    {
                        Tile tile = Framing.GetTileSafely(x, y);

                        //Removing any extra water droplets that may have generated on sloped tiles.
                        if (tile.HasTile && tile.Slope != SlopeType.Solid && Framing.GetTileSafely(x, y + 1).TileType == TileID.WaterDrip)
                        {
                            Framing.GetTileSafely(x, y + 1).ClearTile();  
                        }
                    }
                }
            }

            public static void CreateSlateDeposits()
            {
                LoopWorldAndGenerateTilesWithDepthModifiers(6, Main.rand.Next(17, 24), Main.rand.Next(16, 29), ModContent.TileType<SlateTile>(), (int)Main.rockLayer + 100, true);

                for (int x = 0; x < Main.maxTilesX; x++)
                {
                    for (int y = 0; y < Main.maxTilesY; y++)
                    {
                        Tile tile = Framing.GetTileSafely(x, y);
                        if (tile.HasTile && tile.TileType == ModContent.TileType<SlateTile>())
                        {
                            Tile tileDown = Framing.GetTileSafely(x, y + 1);
                            if (!tileDown.HasTile && Main.rand.NextBool(8))
                            {
                                WorldGen.PlaceTile(x, y + 1, TileID.WaterDrip, true);
                            }
                        }
                    }
                }
            }

            public static void GenerateGeodes()
            {
                for (int q = 0; q < 10; q++)
                {
                    int x = Main.rand.Next(100, Main.maxTilesX - 100);
                    int y = Main.rand.Next(100, Main.UnderworldLayer - 100);

                    Tile tile = Framing.GetTileSafely(x, y);

                    if (tile.HasTile && tile.TileType == TileID.Stone && WorldGen.InWorld(x, y))
                    {
                        Point placePoint = new Point(x, y);

                        for (int i = 0; i < 5; i++)
                        {
                            int width = Main.rand.Next(12, 14);
                            int height = Main.rand.Next(12, 14);

                            WorldUtils.Gen(placePoint, new Shapes.Circle(width, height), new Actions.SetTile(TileID.Marble));

                        }

                        for (int i = 0; i < 5; i++)
                        {
                            int width = Main.rand.Next(6, 12);
                            int height = Main.rand.Next(6, 12);

                            WorldUtils.Gen(placePoint, new Shapes.Circle(width, height), new Actions.ClearTile());
                        }

                        WorldGen.gemCave(x, y);
                    }
                }
            }

            private static void CreateHellBarrier()
            {
                // This is to help prevent gem caves from leaking into hell. Generates a long strip of stone to seperate hell from the caverns.
                for (int x = 0; x < Main.maxTilesX; x++)
                {
                    for (int y = (int)Main.rockLayer + 200; y < Main.rockLayer + 205; y++)
                    {
                        Tile tile = Framing.GetTileSafely(x, y);
                        if (!tile.HasTile)
                        {
                            WorldGen.TileRunner(x, y + Main.rand.Next(-6, 6), 12, 4, TileID.Stone, true);
                        }
                    }
                }
            }

            private void GenerateHell()
            {
                LoopWorldAndGenerateTiles(1, Main.rand.Next(40, 69), Main.rand.Next(30, 45), TileID.Ash, TileID.Stone, Main.UnderworldLayer + 40, Main.maxTilesY);

                //This is handling generating a "platform" right below the bottom of the world, so lava doesn't fall out.
                // TileRunner is run in a straight line, to provide a better variation and blending.
                for (int x = 0; x < Main.maxTilesX;  x++)
                {
                    for (int y = Main.maxTilesY - 20; y < Main.maxTilesY; y++)
                    {
                        Tile tile = Framing.GetTileSafely(x, y);
                        if (!tile.HasTile)
                        {
                            WorldGen.TileRunner(x, y, 8, 4, TileID.Ash, true);
                        }
                    }
                }       

                for (int x = 0; x < Main.maxTilesX; x++)
                {
                    for (int y = Main.UnderworldLayer + 70; y < Main.maxTilesY; y++)
                    {
                        Tile tile = Framing.GetTileSafely(x, y);

                        //Smooths the line between stone and ash generation
                        if (tile.HasTile && tile.TileType == TileID.Stone)
                        {
                            WorldGen.TileRunner(x, y, 9, 14, TileID.Ash);
                        }

                        //Places lava everywhere from the bottom of the world up by 100
                        //Also replaces sandstone with ash in the rare case it generates.
                        if (y >= Main.maxTilesY - 100 && !tile.HasTile)
                        {
                            WorldGen.PlaceLiquid(x, y, (byte)LiquidID.Lava, 255);
                        }
                    }
                }

                // Hellstone generation
                LoopWorldAndGenerateTiles(2, Main.rand.Next(6, 10), Main.rand.Next(6, 10), TileID.Hellstone, TileID.Ash, Main.UnderworldLayer + 60, Main.maxTilesY);
            }

            private static void RunOreGen()
            {
                UltimateSkyblock.Instance.Logger.Info("Generating Sandstone");
                LoopWorldAndGenerateTilesWithDepthModifiers(8, Main.rand.Next(9, 18), Main.rand.Next(8, 22), TileID.Sandstone, Main.UnderworldLayer, false);

                UltimateSkyblock.Instance.Logger.Info("Generating Dirt");
                LoopWorldAndGenerateTilesWithDepthModifiers(3, Main.rand.Next(8, 13), Main.rand.Next(20, 38), TileID.Dirt, Main.maxTilesY - (Main.maxTilesY / 7), false, 200);

                UltimateSkyblock.Instance.Logger.Info("Generating Copper or Tin");
                LoopWorldAndGenerateTilesWithDepthModifiers(8, Main.rand.Next(6, 10), Main.rand.Next(50, 70), SubVars.copper, Main.UnderworldLayer - 40, false, 100);

                UltimateSkyblock.Instance.Logger.Info("Generating Silver or Tungsten");
                LoopWorldAndGenerateTilesWithDepthModifiers(10, Main.rand.Next(4, 8), Main.rand.Next(40, 60), SubVars.silver, Main.UnderworldLayer - 30, false, 100);

                UltimateSkyblock.Instance.Logger.Info("Generating Iron or Lead");
                LoopWorldAndGenerateTilesWithDepthModifiers(9, Main.rand.Next(7, 13), Main.rand.Next(4, 8), SubVars.iron, Main.UnderworldLayer, false, 100);

                UltimateSkyblock.Instance.Logger.Info("Generating Gold or Platinum");
                LoopWorldAndGenerateTilesWithDepthModifiers(11, Main.rand.Next(4, 8), Main.rand.Next(5, 8), SubVars.gold, Main.UnderworldLayer, false, 100);
            }   

            private static void SmoothWorld()
            {
                for (int x = 50; x < Main.maxTilesX - 50; x++)
                {
                    for (int y = 50; y < Main.maxTilesY - 50; y++)
                    {
                        if (Main.rand.NextBool(7) && WorldGen.InWorld(x, y) && Framing.GetTileSafely(x, y + 1).TileType != TileID.WaterDrip)
                        Tile.SmoothSlope(x, y);
                    }
                }

                // Removes blocks with no neighboring tiles, to avoid floating singular tiles.
                for (int x = 0; x < Main.maxTilesX; x++)
                {
                    for (int y = 0; y < Main.maxTilesY; y++)
                    {
                        Tile tile = Framing.GetTileSafely(x, y);
                        if (tile.HasTile)
                        {
                            Tile tileUp = Framing.GetTileSafely(x, y - 1);
                            Tile tileDown = Framing.GetTileSafely(x, y + 1);
                            Tile tileLeft = Framing.GetTileSafely(x - 1, y);
                            Tile tileRight = Framing.GetTileSafely(x + 1, y);

                            if (!tileUp.HasTile && !tileDown.HasTile && !tileLeft.HasTile && !tileRight.HasTile)
                            {
                                tile.ClearTile();
                            } 
                        }
                    }
                }
            }

            private static void PlaceStalactites()
            {
                UltimateSkyblock.Instance.Logger.Info("Placing Stalactites");
                for (int x = 100; x < Main.maxTilesX - 100; x++)
                {
                    for (int y = 100; y < Main.maxTilesY - 100; y++)
                    {
                        Tile tile = Framing.GetTileSafely(x, y);
                        if (!tile.HasTile && tile.Slope == SlopeType.Solid && Framing.GetTileSafely(x, y - 1).HasTile && Main.rand.NextBool(5))
                        {
                            WorldGen.PlaceUncheckedStalactite(x, y, Main.rand.NextBool(), Main.rand.Next(3), false);
                        }
                    }
                }
            }
        }
    }
}
