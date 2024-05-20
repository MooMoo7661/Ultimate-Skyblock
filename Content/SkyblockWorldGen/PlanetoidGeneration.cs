using static UltimateSkyblock.Content.SkyblockWorldGen.MainWorld;
using static UltimateSkyblock.UltimateSkyblock;
using static UltimateSkyblock.Content.SkyblockWorldGen.WorldHelpers;
using UltimateSkyblock.Content.Tiles.Blocks;

namespace UltimateSkyblock.Content.SkyblockWorldGen
{
    public partial class PlanetoidGeneration : ModSystem
    {
        public enum PlanetoidSizes
        {
            Small,
            Medium,
            Large
        }

        public static int PlanetoidHeight = 125;

        public static void PlanetoidRunner()
        {
            for (int x = 50; x < Main.maxTilesX - 50; x += 130 + WorldGen.genRand.Next(-10, 25))
            {
                if (!WorldGen.InWorld(x, PlanetoidHeight))
                    continue;

                Slice slice = Slice.GetIslandsFromCoordinate(x);
                switch (slice.Name)
                {
                    case "Deepstone":
                        GenerateDeepstonePlanetoid(x);
                        break;
                    case "Mushroom" or "Dungeon":
                        GenerateMeteorPlanetoid(x);
                        break;
                    case "Forest":
                        GenerateForestPlanetoid(x);
                        break;
                    case "Jungle":
                        GenerateHivePlanetoid(x);
                        GenerateChlorophytePlanetoid(slice, x);
                        break;
                    case "Snow":
                        GenerateSnowPlanetoid(x);
                        break;
                    case "Hallow":
                        GenerateHallowedPlanetoid(x);
                        break;
                    case "Desert":
                        GenerateDesertPlanetoid(x);
                        break;
                    case "Evil":
                        GenerateEvilPlanetoid(x);
                        break;
                }
            }
        }

        public static void GenerateMeteorPlanetoid(int x)
        {
            Point genPoint = new Point(x, PlanetoidHeight + WorldGen.genRand.Next(-10, 10));
            int size = WorldGen.genRand.Next(14, 19);
            WorldUtils.Gen(genPoint, new Shapes.Circle(size, size), new Actions.SetTile(TileID.Stone));

            WorldGen.TileRunner(genPoint.X, genPoint.Y, 10f, 6, TileID.Meteorite, false, 0, 0, false);

            WorldGen.TileRunner(genPoint.X + Main.rand.Next(-2, 2), genPoint.Y + Main.rand.Next(-2, 2), 5f, 9, TileID.Meteorite, false, 0, 0, false);
            WorldGen.TileRunner(genPoint.X - Main.rand.Next(-2, 2), genPoint.Y - Main.rand.Next(-2, 2), 7f, 4, TileID.Meteorite, false, 0, 0, false);

            for (int j = 0; j < 6; j++)
            {
                WorldGen.TileRunner(genPoint.X - Main.rand.Next(-2, 2), genPoint.Y - Main.rand.Next(-2, 2), 4f, 15 + Main.rand.Next(0, 2), TileID.Meteorite, false, Main.rand.Next(-4, 4), Main.rand.Next(-4, 4), false);
                WorldGen.TileRunner(genPoint.X - Main.rand.Next(-2, 2), genPoint.Y - Main.rand.Next(-2, 2), 3f, 15 + Main.rand.Next(0, 4), TileID.Meteorite, false, Main.rand.Next(-4, 4), Main.rand.Next(-4, 4), false);
            }
        }

        /// <summary>
        /// Handles generating the forest planetoids. Ore is generated according to world bools.
        /// </summary>
        public static void GenerateForestPlanetoid(int x)
        {
            // Weird math to account for different world sizes
            Point placePoint = new(x, PlanetoidHeight + WorldGen.genRand.Next(-10, 10));
            PlanetoidSizes planetoidSize = PlanetoidSizes.Small;
            int oreRange = 6;
            int size = WorldGen.genRand.Next(15, 25);
            ShapeData shapeData = new ShapeData();
            ShapeData shapeData2 = new ShapeData();
            WorldUtils.Gen(placePoint, new Shapes.Circle(size, size), new Actions.SetTile(TileID.Dirt));
            WorldUtils.Gen(placePoint, new Shapes.Circle(size, size), new Actions.Blank().Output(shapeData));
            WorldUtils.Gen(placePoint, new Shapes.Circle(size - 1, size - 1), new Actions.Blank().Output(shapeData2));

            if (size <= 18) { planetoidSize = PlanetoidSizes.Small; }
            if (size > 18 && size < 21) { planetoidSize = PlanetoidSizes.Medium; }
            if (size >= 21) { planetoidSize = PlanetoidSizes.Large; }

            // Causes larger planetoids to generate 2 more circles, both with a longer width and height to create different blob shapes.
            // A somewhat successful attempt at making the planetoids appear less diamond-shaped.
            if (size >= 16)
            {
                WorldUtils.Gen(placePoint, new Shapes.Circle(size + 1, size - 1), new Actions.SetTile(TileID.Dirt));
                WorldUtils.Gen(placePoint, new Shapes.Circle(size - 1, size + 1), new Actions.SetTile(TileID.Dirt));
                WorldUtils.Gen(placePoint, new Shapes.Circle(size, size), new Actions.Blank().Output(shapeData));
                WorldUtils.Gen(placePoint, new Shapes.Circle(size + 1, size - 1), new Actions.Blank().Output(shapeData));
                WorldUtils.Gen(placePoint, new Shapes.Circle(size - 1, size + 1), new Actions.Blank().Output(shapeData));
            }

            switch (planetoidSize)
            {
                case PlanetoidSizes.Small:
                    oreRange = 6;
                    break;

                case PlanetoidSizes.Medium:
                    oreRange = 8;
                    break;

                case PlanetoidSizes.Large:
                    oreRange = 10;
                    break;
            }

            for (int z = 0; z < 10; z++)
            {
                WorldGen.OreRunner(placePoint.X + WorldGen.genRand.Next(-3 - oreRange, 3 + oreRange), placePoint.Y + WorldGen.genRand.Next(3 - oreRange, 3 + oreRange), oreRange + WorldGen.genRand.Next(2), (int)(oreRange * 1.3f) + WorldGen.genRand.Next(2), TileID.Stone);
            }

            List<int> ores = new List<int>
            {
                    TileID.ClayBlock
            };

            if (GenVars.copper == TileID.Copper) { ores.Add(TileID.Copper); } else { ores.Add(TileID.Tin); }
            if (GenVars.iron == TileID.Lead) { ores.Add(TileID.Iron); } else { ores.Add(TileID.Lead); }

            for (int k = 0; k < 2; k++)
            {
                for (int j = 0; j < ores.Count; j++)
                {
                    if (!WorldGen.genRand.NextBool(3))
                    WorldGen.OreRunner(placePoint.X + WorldGen.genRand.Next(-9 - oreRange / 2, 9 + oreRange / 2), placePoint.Y + WorldGen.genRand.Next(-9 - oreRange / 2, 9 + oreRange / 2), oreRange, (int)(oreRange * 0.6f) + WorldGen.genRand.Next(2), (ushort)ores[j]);
                }
            }

            WorldUtils.Gen(placePoint, new ModShapes.OuterOutline(shapeData, true), new Actions.SetTile(TileID.Grass));
            WorldUtils.Gen(placePoint, new ModShapes.OuterOutline(shapeData2, true), new Actions.SetTile(TileID.Dirt));

        }

        /// <summary>
        /// Handles generating the Hive Planetoids. Randomly picks from a string file path list and generates the planetoid from that path.
        /// </summary>
        public static void GenerateHivePlanetoid(int x)
        {
            Point16 placePoint = new(x, PlanetoidHeight - 20 + WorldGen.genRand.Next(-10, 10));
            List<string> medium = new List<string> { hivePath + "Medium", hivePath + "Medium2", hivePath + "Medium3", hivePath + "Medium4" };
            List<string> small = new List<string> { hivePath + "Small1", hivePath + "Small2", hivePath + "Small3", hivePath + "SmallOpen", hivePath + "SmallOpen2" };

            List<string> planetoids = Main.rand.Next(3) switch
            {
                0 => medium,
                1 => small,
                _ => new List<string> { hivePath + "Large1" }
            };

            int index = Main.rand.Next(planetoids.Count);
            string planetoidToGenerate = planetoids[index];
            Generator.GenerateStructure(planetoidToGenerate, placePoint, Instance);
        }

        /// <summary>
        /// Handles generating the Winter Planetoids. Randomly generates them with ice, snow, slush, and platinum/gold ore. Whether gold or platinum is generated is chosen based on world bools.
        /// </summary>
        public static void GenerateSnowPlanetoid(int x)
        {
            // Weird math to account for different world sizes
            Point placePoint = new(x, PlanetoidHeight + WorldGen.genRand.Next(-10, 10));
            PlanetoidSizes planetoidSize = PlanetoidSizes.Small;
            int oreRange = 6;
            int size = WorldGen.genRand.Next(15, 25);
            ShapeData shapeData = new ShapeData();
            ShapeData shapeData2 = new ShapeData();
            WorldUtils.Gen(placePoint, new Shapes.Circle(size, size), new Actions.SetTile(TileID.SnowBlock));
            WorldUtils.Gen(placePoint, new Shapes.Circle(size, size), new Actions.Blank().Output(shapeData));
            WorldUtils.Gen(placePoint, new Shapes.Circle(size - 1, size - 1), new Actions.Blank().Output(shapeData2));

            if (size <= 18) { planetoidSize = PlanetoidSizes.Small; }
            if (size > 18 && size < 21) { planetoidSize = PlanetoidSizes.Medium; }
            if (size >= 21) { planetoidSize = PlanetoidSizes.Large; }


            // Causes larger planetoids to generate 2 more circles, both with a longer width and height to create different blob shapes.
            // A somewhat successful attempt at making the planetoids appear less diamond-shaped.
            if (size >= 16)
            {
                WorldUtils.Gen(placePoint, new Shapes.Circle(size + 1, size - 1), new Actions.SetTile(TileID.SnowBlock));
                WorldUtils.Gen(placePoint, new Shapes.Circle(size - 1, size + 1), new Actions.SetTile(TileID.SnowBlock));
                WorldUtils.Gen(placePoint, new Shapes.Circle(size, size), new Actions.Blank().Output(shapeData));
                WorldUtils.Gen(placePoint, new Shapes.Circle(size + 1, size - 1), new Actions.Blank().Output(shapeData));
                WorldUtils.Gen(placePoint, new Shapes.Circle(size - 1, size + 1), new Actions.Blank().Output(shapeData));
            }

            switch (planetoidSize)
            {
                case PlanetoidSizes.Small:
                    oreRange = 6;
                    break;

                case PlanetoidSizes.Medium:
                    oreRange = 8;
                    break;

                case PlanetoidSizes.Large:
                    oreRange = 10;
                    break;
            }

            for (int z = 0; z < 10; z++)
            {
                WorldGen.OreRunner(placePoint.X + WorldGen.genRand.Next(-3 - oreRange, 3 + oreRange), placePoint.Y + WorldGen.genRand.Next(3 - oreRange, 3 + oreRange), oreRange + WorldGen.genRand.Next(2), (int)(oreRange * 1.3f) + WorldGen.genRand.Next(2), TileID.IceBlock);
            }

            List<int> ores = new List<int>
            {
                TileID.IceBlock
            };

            if (GenVars.gold == TileID.Gold) { ores.Add(TileID.Gold); } else { ores.Add(TileID.Platinum); }
            if (GenVars.silver == TileID.Silver) { ores.Add(TileID.Silver); } else { ores.Add(TileID.Tungsten); }

            for (int k = 0; k < 2; k++)
            {
                for (int ligma = 0; ligma < 2; ligma++)
                {
                    if (!WorldGen.genRand.NextBool(3))
                        WorldGen.OreRunner(placePoint.X + WorldGen.genRand.Next(-3 - oreRange, 3 + oreRange), placePoint.Y + WorldGen.genRand.Next(-3 - oreRange, 3 + oreRange), oreRange, (int)(oreRange * 1.1f) + WorldGen.genRand.Next(2), TileID.IceBlock);
                }

                for (int balls = 0; balls < ores.Count; balls++)
                {
                    if (!WorldGen.genRand.NextBool(3))
                        WorldGen.OreRunner(placePoint.X + WorldGen.genRand.Next(-9 - oreRange / 2, 9 + oreRange / 2), placePoint.Y + WorldGen.genRand.Next(-9 - oreRange / 2, 9 + oreRange / 2), oreRange, (int)(oreRange * 0.6f) + WorldGen.genRand.Next(2), (ushort)ores[balls]);
                }
            }

            WorldUtils.Gen(placePoint, new ModShapes.OuterOutline(shapeData, true), new Actions.SetTile(TileID.SnowBlock));
            WorldUtils.Gen(placePoint, new ModShapes.OuterOutline(shapeData2, true), new Actions.SetTile(TileID.SnowBlock));
        }

        /// <summary>
        /// Handles randomly generating chlorophyte planetoids.
        /// </summary>
        /// <remarks>
        /// Uses a random width between 14 and 21, and calls WorldGen.TileRunner to create random "streaks" of chlorophyte in an "X" shape.
        /// </remarks>
        public static void GenerateChlorophytePlanetoid(Slice slice, int x)
        {
            Point placePoint = new(x, SliceGenerationTasks.IslandHeight + 150 + slice.Length / 4 + WorldGen.genRand.Next(-15, 15));
            int size = WorldGen.genRand.Next(14, 21);
            ShapeData shapeData = new ShapeData();
            WorldUtils.Gen(placePoint, new Shapes.Circle(size, size), new Actions.SetTile(TileID.Mud));
            WorldUtils.Gen(placePoint, new Shapes.Circle(size, size), new Actions.Blank().Output(shapeData));
            if (size >= 18)
            {
                WorldUtils.Gen(placePoint, new Shapes.Circle(size + 1, size - 1), new Actions.SetTile(TileID.Mud));
                WorldUtils.Gen(placePoint, new Shapes.Circle(size - 1, size + 1), new Actions.SetTile(TileID.Mud));
                WorldUtils.Gen(placePoint, new Shapes.Circle(size, size), new Actions.Blank().Output(shapeData));
                WorldUtils.Gen(placePoint, new Shapes.Circle(size + 1, size - 1), new Actions.Blank().Output(shapeData));
                WorldUtils.Gen(placePoint, new Shapes.Circle(size - 1, size + 1), new Actions.Blank().Output(shapeData));
            }

            WorldUtils.Gen(placePoint, new ModShapes.OuterOutline(shapeData, true), new Actions.SetTile(TileID.JungleGrass));

            WorldGen.TileRunner(placePoint.X, placePoint.Y, 6f, 15, TileID.Chlorophyte, false, -6, 6, false);
            WorldGen.TileRunner(placePoint.X, placePoint.Y, 6f, 15, TileID.Chlorophyte, false, 6, 6, false);
            WorldGen.TileRunner(placePoint.X, placePoint.Y, 6f, 15, TileID.Chlorophyte, false, -6, -6, false);
            WorldGen.TileRunner(placePoint.X, placePoint.Y, 6f, 15, TileID.Chlorophyte, false, 6, -6, false);
        }

        public static void GenerateEvilPlanetoid(int x)
        {
            string crimsonString = path + "Crimson" + "Planetoid";
            string corruptString = path + "Corrupt" + "Planetoid";

            List<string> planetoids;
            List<string> crimsonPlanetoids = new List<string>
            {
                crimsonString + "Medium1",
                crimsonString + "Medium2",
                crimsonString + "Medium3",
                crimsonString + "Medium4",
                crimsonString + "Medium5",
                crimsonString + "Small1"
            };
            List<string> corruptPlanetoids = new List<string>
            {
                corruptString + "Large1",
                corruptString + "Large2",
                corruptString + "Medium1",
                corruptString + "Medium2",
                corruptString + "Medium3",
                corruptString + "Medium4"
            };

            if (WorldGen.crimson)
                planetoids = crimsonPlanetoids;
            else
                planetoids = corruptPlanetoids;

            Point16 placePoint = new(x, PlanetoidHeight + WorldGen.genRand.Next(-10, 10));
            int index = Main.rand.Next(planetoids.Count);
            string planetoidToGenerate = planetoids[index];
            Generator.GenerateStructure(planetoidToGenerate, placePoint, Instance);
            planetoids.RemoveAt(index);
        }

        public static void GenerateHallowedPlanetoid(int x)
        {
            Point genPoint = new Point(x, PlanetoidHeight + WorldGen.genRand.Next(-10, 10));
            if (WorldGen.InWorld(genPoint.X, genPoint.Y))
            {
                int size = WorldGen.genRand.Next(14, 19);
                ShapeData data = new ShapeData();
                WorldUtils.Gen(genPoint, new Shapes.Circle(size + 3, size + 3), new Actions.SetTile(TileID.HardenedSand));
                WorldUtils.Gen(genPoint, new Shapes.Circle(size + 3, size + 3), new Actions.Blank().Output(data));
                WorldUtils.Gen(genPoint, new Shapes.Circle(size, size), new Actions.SetTile(TileID.Dirt));

                for (int j = 0; j < 6; j++)
                {
                    WorldGen.TileRunner(genPoint.X - Main.rand.Next(-2, 2), genPoint.Y - Main.rand.Next(-2, 2), 5f, 15 + Main.rand.Next(0, 4), GenVars.iron, false, Main.rand.Next(-4, 4), Main.rand.Next(-4, 4), false);
                    WorldGen.TileRunner(genPoint.X - Main.rand.Next(-2, 2), genPoint.Y - Main.rand.Next(-2, 2), 5f, 15 + Main.rand.Next(0, 4), TileID.Pearlstone, false, Main.rand.Next(-4, 4), Main.rand.Next(-4, 4), false);
                }

                for (int Q = genPoint.X - size - 5; Q < genPoint.X + size + 5; Q++)
                {
                    for (int R = genPoint.Y - size - 5; R < genPoint.Y + size + 5; R++)
                    {
                        Tile tile = Framing.GetTileSafely(Q, R);
                        if (tile.HasTile && tile.TileType == TileID.HardenedSand)
                        {
                            WorldGen.PlaceTile(Q, R, TileID.Dirt, true, forced: true);
                        }
                    }
                }

                WorldUtils.Gen(genPoint, new ModShapes.OuterOutline(data), new Actions.SetTile(TileID.HallowedGrass));
            }
        }

        public static void GenerateDeepstonePlanetoid(int x)
        {
            Point genPoint = new Point(x, PlanetoidHeight + WorldGen.genRand.Next(-10, 10));
            if (WorldGen.InWorld(genPoint.X, genPoint.Y))
            {
                int size = WorldGen.genRand.Next(14, 19);
                WorldUtils.Gen(genPoint, new Shapes.Circle(size + 3, size + 3), new Actions.SetTile((ushort)ModContent.TileType<DeepstoneTile>()));
                WorldUtils.Gen(genPoint, new Shapes.Circle(size, size), new Actions.SetTile(TileID.Dirt));

                WorldGen.TileRunner(genPoint.X + Main.rand.Next(-2, 2), genPoint.Y + Main.rand.Next(-2, 2), 8f, 9, ModContent.TileType<DeepsoilTile>(), false, 0, 0, false);
                WorldGen.TileRunner(genPoint.X - Main.rand.Next(-2, 2), genPoint.Y - Main.rand.Next(-2, 2), 12f, 8, ModContent.TileType<DeepsoilTile>(), false, 0, 0, false);

                for (int j = 0; j < 6; j++)
                {
                    WorldGen.TileRunner(genPoint.X - Main.rand.Next(-2, 2), genPoint.Y - Main.rand.Next(-2, 2), 6f, 10 + Main.rand.Next(0, 2), ModContent.TileType<DeepsoilTile>(), false, Main.rand.Next(-4, 4), Main.rand.Next(-4, 4), false);
                    WorldGen.TileRunner(genPoint.X - Main.rand.Next(-2, 2), genPoint.Y - Main.rand.Next(-2, 2), 5f, 10 + Main.rand.Next(0, 4), ModContent.TileType<DeepsoilTile>(), false, Main.rand.Next(-4, 4), Main.rand.Next(-4, 4), false);
                }

                for (int Q = genPoint.X - size - 1; Q < genPoint.X + size + 1; Q++)
                {
                    for (int R = genPoint.Y - size - 1; R < genPoint.Y + size + 1; R++)
                    {
                        Tile tile = Framing.GetTileSafely(Q, R);
                        if (tile.HasTile && tile.TileType == TileID.Dirt)
                        {
                            WorldGen.PlaceTile(Q, R, (ushort)ModContent.TileType<HardenedDeepstoneTile>(), true, forced: true);
                        }
                    }
                }
            }
        }

        public static void GenerateDesertPlanetoid(int x)
        {
            Point genPoint = new Point(x, PlanetoidHeight + WorldGen.genRand.Next(-10, 10));
            if (WorldGen.InWorld(genPoint.X, genPoint.Y))
            {
                int size = WorldGen.genRand.Next(14, 19);
                WorldUtils.Gen(genPoint, new Shapes.Circle(size + 3, size + 3), new Actions.SetTile(TileID.HardenedSand));
                WorldUtils.Gen(genPoint, new Shapes.Circle(size, size), new Actions.SetTile(TileID.Dirt));

                WorldGen.TileRunner(genPoint.X + Main.rand.Next(-2, 2), genPoint.Y + Main.rand.Next(-2, 2), 8f, 9, TileID.DesertFossil, false, 0, 0, false);
                WorldGen.TileRunner(genPoint.X - Main.rand.Next(-2, 2), genPoint.Y - Main.rand.Next(-2, 2), 12f, 8, TileID.HardenedSand, false, 0, 0, false);

                for (int j = 0; j < 6; j++)
                {
                    WorldGen.TileRunner(genPoint.X - Main.rand.Next(-2, 2), genPoint.Y - Main.rand.Next(-2, 2), 6f, 15 + Main.rand.Next(0, 2), TileID.DesertFossil, false, Main.rand.Next(-4, 4), Main.rand.Next(-4, 4), false);
                    WorldGen.TileRunner(genPoint.X - Main.rand.Next(-2, 2), genPoint.Y - Main.rand.Next(-2, 2), 5f, 15 + Main.rand.Next(0, 4), TileID.HardenedSand, false, Main.rand.Next(-4, 4), Main.rand.Next(-4, 4), false);
                }

                for (int Q = genPoint.X - size - 1; Q < genPoint.X + size + 1; Q++)
                {
                    for (int R = genPoint.Y - size - 1; R < genPoint.Y + size + 1; R++)
                    {
                        Tile tile = Framing.GetTileSafely(Q, R);
                        if (tile.HasTile && tile.TileType == TileID.Dirt)
                        {
                            WorldGen.PlaceTile(Q, R, TileID.Sand, true, forced: true);
                        }
                    }
                }
            }
        }
    }
}
