using System.Diagnostics;
using SubworldLibrary;
using Terraria.GameContent.UI.States;
using UltimateSkyblock.Content.Configs;
using UltimateSkyblock.Content.Subworlds.DungeonPasses;
using UltimateSkyblock.Content.Subworlds.MiningPasses;
using UltimateSkyblock.Content.Tiles.Blocks;
using static UltimateSkyblock.Content.Subworlds.GenUtils;

namespace UltimateSkyblock.Content.Subworlds
{
    public class MiningSubworld : Subworld
    {
        public override int Width => 1000;
        public override int Height => 1000;
        public override bool ShouldSave => ModContent.GetInstance<SubworldConfig>().SubworldSaving;
        public override string Name => "Mining Subworld";

        public static int Slate { get { return ModContent.TileType<SlateTile>(); } }
        public static int Deepstone { get { return ModContent.TileType<DeepstoneTile>(); } }
        public static int DeepstoneLayer { get { return Main.UnderworldLayer - 100; } }

        public override void DrawMenu(GameTime gameTime)
        {
           new UIWorldLoad().Draw(Main.spriteBatch);
           base.DrawMenu(gameTime);
        }

        public override void OnEnter()
        {
            SubworldSystem.hideUnderworld = false;

            UltimateSkyblock.Instance.Logger.Info("Logging tag \"Main.hardMode:\"" + " - " + Main.hardMode);
        }

        public override void OnLoad()
        {
            //Fixing offset between backgrounds.
            //Underground backgrounds are dependant on Main.worldSurface,
            //so setting it to a value that doesn't let backgrounds repeat correctly will cause black backgrounds between transitions.
            Main.worldSurface = 2;
            GenVars.worldSurfaceHigh = 0;
            GenVars.worldSurfaceLow = 0;
            GenVars.rockLayer = Main.maxTilesY / 2;
            GenVars.oceanWaterStartRandomMin = 0;
            GenVars.oceanWaterStartRandomMax = 0;
        }

        public override bool ChangeAudio()
        {
            if (Main.gameMenu && ModContent.GetInstance<SubworldClientConfig>().SubworldLoadingMusic)
            {
                Main.newMusic = MusicLoader.GetMusicSlot("UltimateSkyblock/Content/Sounds/Music/BubbleBobble");
                return true;
            }

            return false;
        }

        public override List<GenPass> Tasks => new List<GenPass>()
        {
            //Basic worldgen
            new RandGenpass("SetRandomness", 5),
            new InitialEarthPass("FillWorld", 210f),
            new BasicPerlinCaveWorldFeatureGenerator("Perlin", 177.4298f),

            //Detailed worldgen
            new DeepstonePass("Deepstone", 90),
            new DeepstoneCaveFeaturesPass("DeepstoneCaveFeatures", 40f),
            new DeepstoneOresPass("DeepstoneOres", 15f),
            new HellBarrierPass("HellBarrier", 30),
            new CaveFeaturesPass("CaveFeatures", 20),
            new SlatePass("Slate", 30),
            new HellPass("Hell", 130),
            new WaterPass("WaterPockets", 50),
            new OreGenerationPass("OreGen", 40),
            new SmoothPass("Smoothing", 15),
            new DeepstoneFoliagePass("Foliage", 20),
            new TrapsPass("Traps", 30),
            new GeodePass("Geodes", 25),
            new MiningHousesPass("MiningHouses", 20),
            new DropletsPass("Droplets", 10),
            new StalactitesPass("Stalactites", 30),
            new DeepstoneBunkerPass("DeepstoneBunker", 40),
            new SpiderPass("SpiderCaves", 10),
            new CaveWallsPass("CaveWalls", 15),
            new CaveDecorationsPass("CaveDecorations", 20),
            new MiningPasses.CleanupPass("Cleanup", 80),

            new SpawnPass("Setting up Spawn", 0.5f)
        };

        public class RandGenpass : GenPass
        {
            public RandGenpass(string name, double loadWeight) : base(name, loadWeight) { }

            protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
            {
                Main.ActiveWorldFileData.SetSeedToRandom();
                WorldGen.genRand.SetSeed(Main.ActiveWorldFileData.Seed);
            }
        }

        public class InitialEarthPass : GenPass
        {
            public InitialEarthPass(string name, double loadWeight) : base(name, loadWeight) { }

            protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
            {
                progress.Message = "Filling the world";

                FillWorldWithStone(ref progress);
                SetGenVars();
            }

            private static void SetGenVars()
            {
                SubVars.copper = WorldGen.genRand.NextBool() ? TileID.Copper : TileID.Tin;
                SubVars.iron = WorldGen.genRand.NextBool() ? TileID.Iron : TileID.Lead;
                SubVars.silver = WorldGen.genRand.NextBool() ? TileID.Silver : TileID.Tungsten;
                SubVars.gold = WorldGen.genRand.NextBool() ? TileID.Gold : TileID.Platinum;

                SubVars.cobalt = WorldGen.genRand.NextBool() ? TileID.Cobalt : TileID.Palladium;
                SubVars.mythril = WorldGen.genRand.NextBool() ? TileID.Mythril : TileID.Orichalcum;
                SubVars.adamantite = WorldGen.genRand.NextBool() ? TileID.Adamantite : TileID.Titanium;

                WorldGen.SavedOreTiers.Cobalt = WorldGen.genRand.NextBool() ? TileID.Cobalt : TileID.Palladium;
                WorldGen.SavedOreTiers.Mythril = WorldGen.genRand.NextBool() ? TileID.Mythril : TileID.Orichalcum;
                WorldGen.SavedOreTiers.Adamantite = WorldGen.genRand.NextBool() ? TileID.Adamantite : TileID.Titanium;
            }

            private static void FillWorldWithStone(ref GenerationProgress progress)
            {
                for (int x = 0; x < Main.maxTilesX; x++)
                {
                    for (int y = 0; y < Main.maxTilesY; y++)
                    {
                        Tile tile = Framing.GetTileSafely(x, y);
                        tile.HasTile = true;
                        Main.tile[x, y].TileType = TileID.Stone;
                        progress.Set((y + x * Main.maxTilesY) / (float)(Main.maxTilesX * Main.maxTilesY));
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
                                if (WorldGen.genRand.NextBool(2))
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
                        WorldGen.PlaceTile(Main.spawnTileX - i, Main.spawnTileY - 3, TileID.Torches, true);
                }
            }
        }
    }
}
