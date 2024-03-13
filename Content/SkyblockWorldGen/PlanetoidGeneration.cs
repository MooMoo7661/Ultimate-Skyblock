using StructureHelper;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Microsoft.Xna.Framework;
using static WorldHelpers;
using static UltimateSkyblock.UltimateSkyblock;
using static Terraria.WorldGen;

namespace UltimateSkyblock.Content.SkyblockWorldGen
{
    public partial class MainWorld : ModSystem
    {
        public enum PlanetoidSizes
        {
            Small,
            Medium,
            Large
        }

        /// <summary>
        /// Handles generating the forest planetoids. Ore is generated according to world bools.
        /// </summary>
        public static void GenForestPlanetoids()
        {
            for (int i = 0; i < 13; i++)
            {
                // Weird math to account for different world sizes
                Point placePoint = new((Main.maxTilesX / 2) - 600 + i * (75 + (int)ScaleBasedOnWorldSizeX) + WorldGen.genRand.Next(-10, 10), 100 + WorldGen.genRand.Next(-10, 10));
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
                // A somewhat sucessful attempt at making the planetoids appear less diamond-shaped.
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
                        oreRange = 9;
                        break;

                    case PlanetoidSizes.Medium:
                        oreRange = 11;
                        break;

                    case PlanetoidSizes.Large:
                        oreRange = 13;
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
                if (GenVars.silver == TileID.Silver) { ores.Add(TileID.Silver); } else { ores.Add(TileID.Tungsten); }
                if (GenVars.iron == TileID.Lead) { ores.Add(TileID.Iron); } else { ores.Add(TileID.Lead); }
                if (GenVars.gold == TileID.Gold) { ores.Add(TileID.Gold); } else { ores.Add(TileID.Platinum); }

                for (int k = 0; k < 2; k++)
                {
                    for (int j = 0; j < ores.Count; j++)
                    {
                        WorldGen.OreRunner(placePoint.X + WorldGen.genRand.Next(-9 - oreRange / 2, 9 + oreRange / 2), placePoint.Y + WorldGen.genRand.Next(-9 - oreRange / 2, 9 + oreRange / 2), oreRange, (int)(oreRange * 0.6f) + WorldGen.genRand.Next(2), (ushort)ores[j]);

                    }
                }

                WorldUtils.Gen(placePoint, new ModShapes.OuterOutline(shapeData, true), new Actions.SetTile(TileID.Grass));
                WorldUtils.Gen(placePoint, new ModShapes.OuterOutline(shapeData2, true), new Actions.SetTile(TileID.Dirt));
            }
        }

        /// <summary>
        /// Handles generating the Hive Planetoids. Randomly picks from a string filepath list and generates the planetoid from that path.
        /// </summary>
        public static void GenHivePlanetoids()
        {
            for (int i = 0; i < 8; i++)
            {
                Point16 placePoint = new(Jungle.X - 150 + i * (75 + (int)ScaleBasedOnWorldSizeX) + WorldGen.genRand.Next(-10, 10), 85);
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
        }

        /// <summary>
        /// Handles generating the Winter Planetoids. Randomly generates them with ice, snow, slush, and platinum/gold ore. Whether gold or platinum is generated is chosen based on world bools.
        /// </summary>
        public static void GenSnowPlanetoids()
        {
            for (int i = 0; i < 6; i++)
            {
                // Weird math to account for different world sizes
                Point placePoint = new(Snow.X + 100 + i * (75 + (int)ScaleBasedOnWorldSizeX) + WorldGen.genRand.Next(-10, 10), 100 + WorldGen.genRand.Next(-10, 10));
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
                // A somewhat sucessful attempt at making the planetoids appear less diamond-shaped.
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
                        oreRange = 9;
                        break;

                    case PlanetoidSizes.Medium:
                        oreRange = 11;
                        break;

                    case PlanetoidSizes.Large:
                        oreRange = 13;
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

                for (int k = 0; k < 2; k++)
                {
                    for (int ligma = 0; ligma < 2; ligma++)
                    {
                        WorldGen.OreRunner(placePoint.X + WorldGen.genRand.Next(-3 - oreRange, 3 + oreRange), placePoint.Y + WorldGen.genRand.Next(-3 - oreRange, 3 + oreRange), oreRange, (int)(oreRange * 1.1f) + WorldGen.genRand.Next(2), TileID.Slush);
                    }

                    for (int balls = 0; balls < ores.Count; balls++)
                    {
                        WorldGen.OreRunner(placePoint.X + WorldGen.genRand.Next(-9 - oreRange / 2, 9 + oreRange / 2), placePoint.Y + WorldGen.genRand.Next(-9 - oreRange / 2, 9 + oreRange / 2), oreRange, (int)(oreRange * 0.6f) + WorldGen.genRand.Next(2), (ushort)ores[balls]);

                    }
                }

                WorldUtils.Gen(placePoint, new ModShapes.OuterOutline(shapeData, true), new Actions.SetTile(TileID.SnowBlock));
                WorldUtils.Gen(placePoint, new ModShapes.OuterOutline(shapeData2, true), new Actions.SetTile(TileID.SnowBlock));
            }
        }

        /// <summary>
        /// Handles randomly generating chlorophyte planetoids.
        /// </summary>
        /// <remarks>
        /// Uses a random width between 14 and 21, and calls WorldGen.TileRunner to create random "streaks" of chlorophyte in an "X" shape.
        /// </remarks>
        public static void GenChlorophytePlanetoids()
        {
            for (int i = 0; i < 5; i++)
            {
                Point placePoint = new(Jungle.X + i * (75 + (int)ScaleBasedOnWorldSizeX) + WorldGen.genRand.Next(-10, 10), Main.maxTilesY / 2 - Main.maxTilesY / 5 + Main.rand.Next(100, 120));
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

                WorldGen.TileRunner(placePoint.X, placePoint.Y, 9f, 20, TileID.Chlorophyte, false, -6, 6, false);
                WorldGen.TileRunner(placePoint.X, placePoint.Y, 9f, 20, TileID.Chlorophyte, false, 6, 6, false);
                WorldGen.TileRunner(placePoint.X, placePoint.Y, 9f, 20, TileID.Chlorophyte, false, -6, -6, false);
                WorldGen.TileRunner(placePoint.X, placePoint.Y, 9f, 20, TileID.Chlorophyte, false, 6, -6, false);
            }
        }

        public static void GenEvilPlanetoids()
        {
            string crimsonString = crimsonPath + "Planetoid";
            string corruptString = corruptPath + "Planetoid";

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
            {
                planetoids = crimsonPlanetoids;
            }
            else
            {
                planetoids = corruptPlanetoids;
            }

            for (int i = 0; i < 6; i++)
            {
                int fuckWorldSizes = 30;
                if (WorldSize == WorldSizes.Large) { fuckWorldSizes = 90; }
                Point16 placePoint = new((Evil.X + 60 + fuckWorldSizes * 3) - i * (80 + (int)ScaleBasedOnWorldSizeX) + WorldGen.genRand.Next(-10, 10), 70 + WorldGen.genRand.Next(-10, 10));
                int index = Main.rand.Next(planetoids.Count);
                string planetoidToGenerate = planetoids[index];
                Generator.GenerateStructure(planetoidToGenerate, placePoint, Instance);
                planetoids.RemoveAt(index);
            }           
        }

        public static void GenHellPlanetoids(int x, int y)
        {
            // Weird math to account for different world sizes
            Point placePoint = new(x, y);
            int size = WorldGen.genRand.Next(14, 19);
            ShapeData shapeData = new ShapeData();
            ShapeData shapeData2 = new ShapeData();
            WorldUtils.Gen(placePoint, new Shapes.Circle(size, size), new Actions.SetTile(TileID.Ash));
            WorldUtils.Gen(placePoint, new Shapes.Circle(size, size), new Actions.Blank().Output(shapeData));
            WorldUtils.Gen(placePoint, new Shapes.Circle(size - 1, size - 1), new Actions.Blank().Output(shapeData2));

            if (Main.rand.NextBool())
            {
                WorldGen.TileRunner(placePoint.X, placePoint.Y, 9f, 20, TileID.Hellstone, false, -6, -6, false);
                WorldGen.TileRunner(placePoint.X, placePoint.Y, 9f, 20, TileID.Hellstone, false, 6, 6, false);
            }
            else
            {
                WorldGen.TileRunner(placePoint.X, placePoint.Y, 9f, 20, TileID.Hellstone, false, 6, -6, false);
                WorldGen.TileRunner(placePoint.X, placePoint.Y, 9f, 20, TileID.Hellstone, false, -6, 6, false);
            }

            WorldUtils.Gen(placePoint, new ModShapes.OuterOutline(shapeData, true), new Actions.SetTile(TileID.AshGrass));
            WorldUtils.Gen(placePoint, new ModShapes.OuterOutline(shapeData2, true), new Actions.SetTile(TileID.Ash));
        }

        public static void GenMeteorites()
        {
            for (int i = 0; i < 10; i++)
            {
                Point genPoint = new Point(Snow.X + (635 + (int)(ScaleBasedOnWorldSizeX * 4f)) + (i * 120), 100 + Main.rand.Next(-10, 15));
                if (InWorld(genPoint.X, genPoint.Y))
                {
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
            }
        }
    }
}
