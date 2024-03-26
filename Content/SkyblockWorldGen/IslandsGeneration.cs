using Terraria.WorldBuilding;
using UltimateSkyblock.Content.Items.Bombs;
using UltimateSkyblock.Content.Items.Placeable;
using UltimateSkyblock.Content.Tiles.Blocks;
using UltimateSkyblock.Content.Utils;
using static Terraria.WorldGen;
using static UltimateSkyblock.Content.SkyblockWorldGen.ChestLootHelpers;
using static UltimateSkyblock.UltimateSkyblock;
using static WorldHelpers;

namespace UltimateSkyblock.Content.SkyblockWorldGen
{
    public partial class MainWorld : ModSystem
    {
        public static bool TempleGen_ = true;
        public static bool dungeonLeft;

        public override void PostWorldGen()
        {
            Main.dungeonX = dungeonLeft ? Main.maxTilesX / 20 : Main.maxTilesX - (Main.maxTilesX / 20);
            Main.dungeonY = Main.maxTilesY / 2 - Main.maxTilesY / 5 + Main.rand.Next(-20, 20);

            Mod.Logger.Info("Successfully cleared world");

            Main.spawnTileX = Main.maxTilesX / 2;
            Main.spawnTileY = Main.maxTilesY / 2 - Main.maxTilesY / 5; // Re-adjusting the spawn point to a constant value.

            dungeonLeft = (Main.dungeonX < Main.maxTilesX / 2) ? true : false;

            ClearWorld();

            GenStartingPlatform();
            GenDungeonPlatform();
            GenUnderworldIslands();
            GenSnowIslands();
            GenJungleIslands();
            GenMushroomIsland();
            GenPlanteraArena();
            GenDesertIslands();

            GenChlorophytePlanetoids();
            GenHivePlanetoids();
            GenForestPlanetoids();
            GenSnowPlanetoids();
            GenEvilPlanetoids();
            GenMeteorites();
            GenDesertPlanetoids();

            GenTemple();

            //PlaceTile(Main.dungeonX, Main.dungeonY, TileID.Adamantite, true, true); // Places tile at the spawn point for the Old Man and the Lunatic Cultists. For testing purposes.
        }

        /// <summary>
        /// Clears the world.
        /// </summary>
        public new static void ClearWorld()
        {
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    Tile tile = Main.tile[i, j];
                    if (tile == null) continue;

                    tile.ClearTile();
                    tile.WallType = WallID.None;
                    tile.LiquidAmount = 0;
                }
            }
        }

        /// <summary>
        /// Handles generating the spawn island, as well as the starting chest.
        /// </summary>
        public static void GenStartingPlatform()
        {
            // X + 38 - Offsets to spawn the starting chest
            // Y + 25

            int x = Main.maxTilesX / 2; // Get the X coordinate to spawn the island
            int y = Main.maxTilesY / 2 - Main.maxTilesY / 5; // Get the Y coordinate to spawn the island

            NPC.NewNPC(Entity.GetSource_NaturalSpawn(), x * 16, y * 16, NPCID.Guide); // Spawn the guide on the island

            int chestType = Main.rand.Next(3) switch // Randomize the starting chest. Type is set later on to accomodate for different chest config options.
            {
                0 => 0,
                1 => 12,
                _ => 8,
            };


            switch (config.StarterChestStyle)
            {
                case ChestType.Simple:
                    chestType = 1; break;

                case ChestType.Luxury:
                    chestType = 13; break;
            }

            x = Main.maxTilesX / 2 - 38; // Re-centers the X coordinate due to islands spawning from the top left of the build
            y = Main.maxTilesY / 2 - Main.maxTilesY / 5 - 25; // Re-centers the Y coordinate due to islands spawning from the top left of the build

            Generator.GenerateStructure(WorldHelpers.forestPath + "Main", new Point16(x, y), Instance); // Generate the spawn island
            GenForestIslands(new Point16(x, y + 50));

            var logger = Instance.Logger;
            logger.Info("Starter chest style: " + config.StarterChestStyle);

            if (config.StarterChestStyle != ChestType.None) // Setting loot for starter chest
            {
                int starterChest = PlaceChest(Main.maxTilesX / 2 - 22, Main.maxTilesY / 2 - Main.maxTilesY / 5 - 5, 21, false, chestType);
                Chest chest = Main.chest[starterChest];
                ChestType style = config.StarterChestStyle;

                List<Item> seeds = new List<Item>
                {
                    new Item(ItemID.BlinkrootSeeds, 3),
                    new Item(ItemID.DaybloomSeeds, 3),
                    new Item(ItemID.WaterleafSeeds, 3),
                    new Item(ItemID.ShiverthornSeeds, 3),
                    new Item(ItemID.DeathweedSeeds, 3),
                };
                List<Item> planters = new List<Item>
                {
                    new Item(ItemID.BlinkrootPlanterBox, 10),
                    new Item(ItemID.CorruptPlanterBox, 10),
                    new Item(ItemID.CrimsonPlanterBox, 10),
                    new Item(ItemID.DayBloomPlanterBox, 10),
                    new Item(ItemID.FireBlossomPlanterBox, 10),
                    new Item(ItemID.MoonglowPlanterBox, 10),
                };

                switch (style)
                {
                    case ChestType.Classic:
                        chest.Add(new Item(ItemID.LavaBucket));
                        chest.Add(new Item(ItemID.WaterBucket));
                        chest.Add(new Item(ItemID.DirtStickyBomb, 5));
                        chest.Add(new Item(ModContent.ItemType<StickyStoneBomb>(), 5));
                        chest.Add(new Rule().GetItem(seeds));
                        chest.Add(new Rule().GetItem(planters));
                        chest.Add(new Item(ItemID.Acorn, Main.rand.Next(4, 9)));
                        chest.Add(new Item(ItemID.SandBlock, 25));
                        break;

                    case ChestType.Simple:
                        chest.Add(ItemID.EmptyBucket);
                        chest.Add(ItemID.SandBlock, 15);
                        chest.Add(ItemID.Acorn, 2);
                        break;

                    case ChestType.Luxury:
                        List<Item> potions = new List<Item> 
                        {
                            new Item(ItemID.IronskinPotion, Main.rand.Next(2, 4)), 
                            new Item(ItemID.ShinePotion, Main.rand.Next(2, 4)),
                            new Item(ItemID.NightOwlPotion, Main.rand.Next(2, 4)),
                            new Item(ItemID.SwiftnessPotion, Main.rand.Next(2, 4)),
                            new Item(ItemID.MiningPotion, Main.rand.Next(2, 4)),
                            new Item(ItemID.BuilderPotion, Main.rand.Next(2, 4)),
                        };

                        chest.Add(new Item(ItemID.LavaBucket));
                        chest.Add(new Item(ItemID.WaterBucket));
                        chest.Add(new Item(ItemID.DirtStickyBomb, 10));
                        chest.Add(new Item(ItemID.CloudinaBottle));
                        chest.Add(new Item(ModContent.ItemType<StickyStoneBomb>(), 10));
                        chest.Add(new Item(ModContent.ItemType<AutoExtractor>()));
                        chest.Add(new Rule().GetItem(seeds));
                        chest.Add(new Rule().GetItem(potions));
                        chest.Add(new Rule().GetItem(planters));
                        chest.Add(new Item(ItemID.Acorn, Main.rand.Next(9, 15)));
                        chest.Add(new Item(ItemID.SandBlock, 50));
                        chest.Add(new Item(ItemID.DirtBlock, 50));
                        chest.Add(new Item(ItemID.StoneBlock, 50));
                        break;

                }
            }
        }

        /// <summary>
        /// Handles generating all of the forest islands.
        /// </summary>
        /// <remarks>
        /// Creates a list of 4 string paths to the structure files, then randomly chooses one to generate and removes it from the list.
        /// </remarks>
        /// <param name="center">Center from where to generate the other islands at</param>
        public static void GenForestIslands(Point16 center)
        {
            List<string> originalIslands = new List<string>
            {
                WorldHelpers.forestPath + "Dual",
                WorldHelpers.forestPath + "House",
                WorldHelpers.forestPath + "Statue",
                WorldHelpers.forestPath + "Chest",
            };

            List<string> islandsToGenerate = originalIslands;

            for (int i = 0; i <= originalIslands.Count + 2; i++)
            {
                int index = WorldGen.genRand.Next(originalIslands.Count);
                string selectedIsland = islandsToGenerate[index];

                Point16 genPoint = i switch
                {
                    0 => new Point16(center.X - WorldGen.genRand.Next(400, 450), center.Y),
                    1 => new Point16(center.X + WorldGen.genRand.Next(400, 450), center.Y - WorldGen.genRand.Next(10, 30)),
                    2 => new Point16(center.X - WorldGen.genRand.Next(180, 200), center.Y - WorldGen.genRand.Next(60, 70)),
                    3 => new Point16(center.X + WorldGen.genRand.Next(180, 200), center.Y - WorldGen.genRand.Next(60, 75)),
                    _ => new Point16(center.X + WorldGen.genRand.Next(160, 210), center.Y - WorldGen.genRand.Next(70, 80)),
                };

                Generator.GenerateStructure(selectedIsland, genPoint, Instance);
                islandsToGenerate.RemoveAt(index);
            }
        }

        /// <summary>
        /// Handles generating the dungeon platform. <br>
        /// Very simple, just takes a certain offset and places the island such that the center lines up exactly with the dungeonX and dungeonY.</br>
        /// </summary>
        public static void GenDungeonPlatform()
        {
            // x - 29
            // y - 10

            Generator.GenerateStructure("Content/SkyblockWorldGen/Structures/DungeonIsland", new Point16(Main.dungeonX - 29, Main.dungeonY - 10), Instance);
        }
        
        /// <summary>
        /// Handles generating the hallow islands.
        /// </summary>
        public static void GenHallowedIslands()
        {
            Hallow = new Point16(Main.maxTilesX / 3, Main.maxTilesY / 2 - Main.maxTilesY / 3 - 40);

            Generator.GenerateStructure("Content/SkyblockWorldGen/Structures/HallowIslandMain", new Point16(WorldHelpers.Hallow.X, WorldHelpers.Hallow.Y), Instance);

            PlaceTile(WorldHelpers.Hallow.X, WorldHelpers.Hallow.Y, TileID.Adamantite);

            // X + 60
            // Y + 6

            int HallowedChest = PlaceChest(WorldHelpers.Hallow.X + 60, WorldHelpers.Hallow.Y + 6, 21, false, 26);

            Chest hallowChest = Main.chest[HallowedChest];


            hallowChest.item[0].SetDefaults(ItemID.RainbowGun);
            hallowChest.item[1].SetDefaults(ItemID.GreaterHealingPotion);
            hallowChest.item[1].stack = 15;
            hallowChest.item[2].SetDefaults(ItemID.GreaterManaPotion);
            hallowChest.item[2].stack = 25;
            hallowChest.item[3].SetDefaults(ItemID.HallowedBar);
            hallowChest.item[3].stack = 30;
            hallowChest.item[4].SetDefaults(ItemID.CrystalBullet);
            hallowChest.item[4].stack = 250;
            hallowChest.item[5].SetDefaults(ItemID.PearlsandBlock);
            hallowChest.item[5].stack = 100;
            hallowChest.item[6].SetDefaults(ItemID.HallowedSeeds);
            hallowChest.item[6].stack = 50;
            hallowChest.item[7].SetDefaults(ItemID.HeartreachPotion);
            hallowChest.item[7].stack = 5;
            hallowChest.item[8].SetDefaults(ItemID.MusicBoxShimmer);
            hallowChest.item[9].SetDefaults(ItemID.WormholePotion);
            hallowChest.item[9].stack = 10;
            hallowChest.item[10].SetDefaults(ItemID.PotionOfReturn);
            hallowChest.item[10].stack = 5;
            hallowChest.item[11].SetDefaults(ItemID.PearlstoneBlock);
            hallowChest.item[11].stack = 200;
            hallowChest.item[12].SetDefaults(ItemID.MusicBoxOWUndergroundHallow);
            hallowChest.item[13].SetDefaults(ItemID.Sextant);
            hallowChest.item[14].SetDefaults(ItemID.ThornsPotion);
            hallowChest.item[14].stack = 5;


            Generator.GenerateStructure("Content/SkyblockWorldGen/Structures/HallowIslandCave", new Point16(WorldHelpers.Hallow.X - 100, WorldHelpers.Hallow.Y + WorldGen.genRand.Next(-20, 10)), Instance);

            Generator.GenerateStructure("Content/SkyblockWorldGen/Structures/HallowIslandBasic1", new Point16(WorldHelpers.Hallow.X + WorldGen.genRand.Next(-50, -30), WorldHelpers.Hallow.Y + WorldGen.genRand.Next(70, 80)), Instance);

            Generator.GenerateStructure("Content/SkyblockWorldGen/Structures/HallowIslandBasic2", new Point16(WorldHelpers.Hallow.X + WorldGen.genRand.Next(10, 15), WorldHelpers.Hallow.Y + WorldGen.genRand.Next(-55, -50)), Instance);

        }

        /// <summary>
        /// Periodically places hell islands throughout the bottom of the world. Re-rolls each island before creation to avoid generating the same island twice in a row. 
        /// </summary>
        public static void GenUnderworldIslands()
        {
            // + 14 X
            // + 18 Y

            Point16 HellPlacePoint = new Point16(Main.maxTilesX / 2 - 140, Main.UnderworldLayer + 50);

            for (int i = 2; i < 20; i++)
            {
                Point16 IslandPlacePoint = new(Main.maxTilesX / 2 - i * (200 + (int)(ScaleBasedOnWorldSizeX * 2.5f)), WorldHelpers.Hell.Y);
                string structureToGenerate = RollHellIsland(IslandPlacePoint.X);
                Point PlanetoidPoint = new(IslandPlacePoint.X + 120, IslandPlacePoint.Y + 60);
                if (InWorld(PlanetoidPoint.X, PlanetoidPoint.Y))
                {
                    GenHellPlanetoids(PlanetoidPoint.X, PlanetoidPoint.Y);
                }

                if (structureToGenerate != null)
                    Generator.GenerateStructure(structureToGenerate, IslandPlacePoint, Instance);
            }

            for (int i = 2; i < 20; i++)
            {
                Point16 IslandPlacePoint = new(Main.maxTilesX / 2 + i * (200 + (int)(ScaleBasedOnWorldSizeX * 2.5f)), WorldHelpers.Hell.Y);
                Point PlanetoidPoint = new(IslandPlacePoint.X - 70, IslandPlacePoint.Y + 60);
                if (InWorld(PlanetoidPoint.X, PlanetoidPoint.Y))
                {
                    GenHellPlanetoids(PlanetoidPoint.X, PlanetoidPoint.Y);
                }

                string structureToGenerate = RollHellIsland(IslandPlacePoint.X);

                if (structureToGenerate != null)
                    Generator.GenerateStructure(structureToGenerate, IslandPlacePoint, Instance);
            }

            static string RollHellIsland(int i)
            {
                if (i > 0)
                {
                    string structureToGenerate = WorldHelpers.hellPath;

                    switch (Main.rand.Next(11))
                    {
                        case 0 or 1:
                            structureToGenerate += "Large1"; break;

                        case 2 or 3:
                            structureToGenerate += "Medium1"; break;

                        case 4 or 5:
                            structureToGenerate += "Medium2"; break;

                        case 6 or 7:
                            structureToGenerate += "Small1"; break;

                        case 8 or 9:
                            structureToGenerate += "Small2"; break;

                        case 10:
                            structureToGenerate += "Small3"; break;

                    }

                    if (structureToGenerate == previousStructure)
                    {
                        return RollHellIsland(i);
                    }
                    else
                    {

                        previousStructure = structureToGenerate;

                        return structureToGenerate;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Handles generating the snow islands.
        /// </summary>
        public static void GenSnowIslands()
        {
            //int offset = WorldSize switch
            //{
            //    WorldSizes.Large => 50,
            //    _ => 20
            //};
            Point16 center = new Point16(Snow.X + (int)(200 + ScaleBasedOnWorldSizeX * 2), Snow.Y);
            Generator.GenerateStructure(WorldHelpers.snowPath + "Castle", center, Instance);

            List<string> structures = new()
            {
                snowPath + "Cluster",
                snowPath + "Igloo",
                snowPath + "Pointy",
                snowPath + "Tiny",
                snowPath + "BiomeChest"
            };

            for (int i = 0; i < 5; i++)
            {
                Point16 genPoint = i switch
                {
                    0 => new(center.X - WorldGen.genRand.Next(180, 215), center.Y + WorldGen.genRand.Next(-10, 10)), // left
                    1 => new(center.X - WorldGen.genRand.Next(-90, 10), center.Y - WorldGen.genRand.Next(120, 150)), // top left
                    2 => new(center.X + WorldGen.genRand.Next(90, 130), center.Y - WorldGen.genRand.Next(120, 150)), // top right
                    3 => new(center.X + WorldGen.genRand.Next(220, 245), center.Y + WorldGen.genRand.Next(-15, 15)), // right
                    4 => new(center.X + WorldGen.genRand.Next(220, 245), center.Y + WorldGen.genRand.Next(120, 150)) // right
                };

                int index = WorldGen.genRand.Next(structures.Count);
                string islandToGenerate = structures[index];
                structures.RemoveAt(index);
                Generator.GenerateStructure(islandToGenerate, genPoint, Instance);
            }
        }

        /// <summary>
        /// Handles generating the Jungle Temple.
        /// </summary>
        public static void GenTemple()
        {
            Generator.GenerateStructure(templePath, new Point16(Jungle.X, Main.maxTilesY / 2 + Main.maxTilesY / 5), Instance);
        }

        public static void GenJungleIslands()
        {
            //17
            //42

            Point16 mainPoint = new Point16(Jungle.X + 200 + (int)(ScaleBasedOnWorldSizeX * 1.85f), Jungle.Y - 50 + Main.rand.Next(-5, 5));
            Point16 bridgePoint = new Point16(Jungle.X + genRand.Next(-20, 20) + (int)(ScaleBasedOnWorldSizeX * 1.85f), Jungle.Y - 50 - Main.rand.Next(-5, 20));
            Point16 smallPoint = new Point16(Jungle.X + 150 + genRand.Next(-5, 5) + (int)(ScaleBasedOnWorldSizeX * 1.85f), Jungle.Y - 150 - Main.rand.Next(-5, 20));

            Generator.GenerateStructure(junglePath + "Main", mainPoint, Instance);
            Generator.GenerateStructure(junglePath + "Bridge", bridgePoint, Instance);
            Generator.GenerateStructure(junglePath + "Small", smallPoint, Instance);

            Generator.GenerateStructure("Content/SkyblockWorldGen/Structures/LockedJungleChest", new Point16(mainPoint.X + 13, mainPoint.Y + 42), Instance);
        }

        public static void GenMushroomIsland()
        {
            Point16 genPos = new(Mushroom.X, Mushroom.Y);

            Generator.GenerateStructure("Content/SkyblockWorldGen/Structures/MushroomIsland", genPos, Instance);
        }

        public static void GenDesertIslands()
        {
            List<string> islandsToGenerate = new List<string>
            {
                desertPath + "Camp",
                desertPath + "House",
                desertPath + "Tiny",
                desertPath + "Ruins",
            };

            Generator.GenerateStructure(desertPath + "Pyramid", Desert, Instance);
            Point16 center = Desert;

            for (int i = 0; i < 4; i++)
            {
                Point16 genPoint = i switch
                {
                    0 => new(center.X - WorldGen.genRand.Next(100, 120), center.Y - WorldGen.genRand.Next(70, 90)),
                    1 => new(center.X + WorldGen.genRand.Next(100, 120), center.Y - WorldGen.genRand.Next(70, 90)),
                    2 => new(center.X - WorldGen.genRand.Next(100, 120), center.Y + WorldGen.genRand.Next(70, 90)),
                    _ => new(center.X + WorldGen.genRand.Next(100, 120), center.Y + WorldGen.genRand.Next(70, 90))
                };

                int index = Main.rand.Next(islandsToGenerate.Count);
                string island = islandsToGenerate[index];
                islandsToGenerate.RemoveAt(index);

                Generator.GenerateStructure(island, genPoint, Instance);
            }
        }

        public static void GenPlanteraArena()
        {
            Point genPos = new(Jungle.X + Main.maxTilesX / 20, Main.maxTilesY / 2 + Main.maxTilesY / 5);

            ShapeData shapeData = new ShapeData();
            ShapeData shapeData2 = new ShapeData();
            ShapeData shapeData3 = new ShapeData();
            WorldUtils.Gen(genPos, new Shapes.Circle(95, 95), new Actions.SetTile(TileID.Mud));
            WorldUtils.Gen(genPos, new Shapes.Circle(95, 95), new Actions.Blank().Output(shapeData));
            WorldUtils.Gen(genPos, new Shapes.Circle(70, 70), new Actions.Clear());
            WorldUtils.Gen(genPos, new Shapes.Circle(70, 70), new Actions.Blank().Output(shapeData2));
            shapeData3 = shapeData2;
            WorldUtils.Gen(genPos, new ModShapes.OuterOutline(shapeData), new Actions.SetTile(TileID.JungleGrass));
            WorldUtils.Gen(genPos, new ModShapes.OuterOutline(shapeData2), new Actions.SetTile(TileID.JungleGrass));
            WorldUtils.Gen(genPos, new ModShapes.OuterOutline(shapeData2), new Actions.Smooth(true));
            WorldUtils.Gen(genPos, new ModShapes.OuterOutline(shapeData), new Actions.Smooth(true));

            PlaceAltar(genPos.X, genPos.Y);
            genPos.Y -= 70;
            genPos.X -= 70;

            PlaceTorchesAndPlatforms(genPos, new(genPos.X + 70, genPos.Y + 70), shapeData3);
        }

        public static void PlaceAltar(int x, int y)
        {
            y += 1;

            ShapeData shapeData = new ShapeData();
            WorldUtils.Gen(new(x, y), new Shapes.Circle(6, 6), new Actions.SetTile(TileID.Mud));
            WorldUtils.Gen(new(x, y), new Shapes.Circle(6, 6), new Actions.Blank().Output(shapeData));
            WorldUtils.Gen(new(x, y), new ModShapes.OuterOutline(shapeData), new Actions.SetTile(TileID.JungleGrass));
            // I use structure helper here instead of WorldGen.PlaceObject due to that method not also placing linked tile entities.
            // It's needed for it to place the TE hooked to it due to it having a special map draw effect.
            Generator.GenerateStructure(path + "PlanteraAltarStructure", new(x - 1, y - 12), Instance);

            y -= 9;
            for (int i = 0; i < 5; i++)
            {
                WorldGen.PlaceTile(x + 2 - i, y + 2, TileID.Mudstone, true, true);
                if (i == 0 || i == 4)
                    WorldGen.PlaceTile(x + 2 - i, y + 1, TileID.Torches, true, false);
            }

            WorldGen.PlaceTile(x, y - 6, TileID.Torches);
        }

        public static void PlaceTorchesAndPlatforms(Point genPos, Point wallPos, ShapeData wallData)
        {   
            WorldUtils.Gen(wallPos, new ModShapes.All(wallData), new Actions.PlaceWall(WallID.MudUnsafe));

            for (int i = genPos.X - 1; i < genPos.X + 141; i++)
            {
                int torchCounter = 0;
                for (int j = genPos.Y - 1; j < genPos.Y + 141; j++)
                {
                    Tile tile = Framing.GetTileSafely(i, j);
                    if (!tile.HasTile)
                    {
                        if (i % 13 == 0)
                        {
                            torchCounter++;
                            if (torchCounter == 10)
                            {
                                WorldGen.PlaceTile(i, j, TileID.Torches);
                                torchCounter = 0;
                            }
                        }

                        if (j % 13 == 0)
                        {
                            bool forced = false;
                            if (Framing.GetTileSafely(i, j).TileType == TileID.Torches)
                            {
                                forced = true;
                            }

                            WorldGen.PlaceTile(i, j, TileID.Platforms, true, forced);
                        }
                    }
                }
            }
        }
    }
}
    