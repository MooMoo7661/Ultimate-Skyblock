using SubworldLibrary;
using Terraria.GameContent.UI.States;
using UltimateSkyblock.Content.Subworlds.Passes;
using UltimateSkyblock.Content.Tiles.Blocks;
using static UltimateSkyblock.Content.Subworlds.GenUtils;

namespace UltimateSkyblock.Content.Subworlds
{
    public class MiningSubworld : Subworld
    {
        public override int Width => 1000;
        public override int Height => 1000;
        public override bool ShouldSave => true;
        public override string Name => "Mining Subworld";

        private UIWorldLoad _menu;

        public static int DeepstoneLayer = Main.UnderworldLayer - 300;

        public static int Slate = ModContent.TileType<SlateTile>();
        public static int Deepstone = ModContent.TileType<DeepstoneTile>();

        public override void DrawMenu(GameTime gameTime)
        {
            if (WorldGenerator.CurrentGenerationProgress != null)
                (_menu ??= new UIWorldLoad()).Draw(Main.spriteBatch);
            else
                base.DrawMenu(gameTime);
        }

        public override void OnEnter()
        {
            SubworldSystem.hideUnderworld = false;
        }

        public override void OnLoad()
        {
            Main.worldSurface = 0;
            GenVars.worldSurfaceHigh = 0;
            GenVars.worldSurfaceLow = 0;
            GenVars.rockLayer = Main.maxTilesY / 2;
            GenVars.oceanWaterStartRandomMin = 0;
            GenVars.oceanWaterStartRandomMax = 0;
            DeepstoneLayer = Main.UnderworldLayer - 300;
        }

        public override List<GenPass> Tasks => new()
        {
            //Basic worldgen
            new InitialEarthPass("FillWorld", 200f),
            new BasicPerlinCaveWorldFeatureGenerator("Perlin", 237.4298f),

            //Detailed worldgen
            new DeepstonePass("Deepslate", 50),
            new HellBarrierPass("HellBarrier", 30),
            new SlatePass("Slate", 30),
            new HellPass("Hell", 100),
            new OreGenerationPass("OreGen", 80),
            new DeepstoneFoliagePass("Foliage", 20),
            new TrapsPass("Traps", 30),
            new GeodePass("Geodes", 25),
            new DropletsPass("Droplets", 15),
            new SmoothPass("Smoothing", 15),
            new StalactitesPass("Stalactites", 30),
            new DeepstoneBunkerPass("DeepstoneBunker", 40),
            new CleanupPass("Cleanup", 80),

            new SpawnPass("Setting up Spawn", 0.5f)
        };

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
                DeepstoneLayer = Main.UnderworldLayer - 100;

                SubVars.copper = Main.rand.NextBool() ? TileID.Copper : TileID.Tin;
                SubVars.iron = Main.rand.NextBool() ? TileID.Iron : TileID.Lead;
                SubVars.silver = Main.rand.NextBool() ? TileID.Silver : TileID.Tungsten;
                SubVars.gold = Main.rand.NextBool() ? TileID.Gold : TileID.Platinum;

                SubVars.cobalt = Main.rand.NextBool() ? TileID.Cobalt : TileID.Palladium;
                SubVars.mythril = Main.rand.NextBool() ? TileID.Mythril : TileID.Orichalcum;
                SubVars.adamantite = Main.rand.NextBool() ? TileID.Adamantite : TileID.Titanium;
            }

            private static void FillWorldWithStone(ref GenerationProgress progress)
            {
                for (int x = 0; x < Main.maxTilesX; x++)
                {
                    for (int y = 0; y < Main.maxTilesY; y++)
                    {
                        if (WorldGen.InWorld(x, y))
                            WorldGen.PlaceTile(x, y, TileID.Stone, true);
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
                        WorldGen.PlaceTile(Main.spawnTileX - i, Main.spawnTileY - 3, TileID.Torches);
                }
            }
        }
    }
}
