using tModPorter;
using UltimateSkyblock.Content.Items.Bombs;
using UltimateSkyblock.Content.Items.Placeable;
using UltimateSkyblock.Content.Tiles.Environment;
using UltimateSkyblock.Content.Utils;
using static UltimateSkyblock.Content.SkyblockWorldGen.ChestLootHelpers;
using static UltimateSkyblock.Content.SkyblockWorldGen.IslandHandler;
using static UltimateSkyblock.Content.SkyblockWorldGen.MainWorld;
using static UltimateSkyblock.Content.SkyblockWorldGen.WorldHelpers;

namespace UltimateSkyblock.Content.SkyblockWorldGen
{
    public class SliceGenerationTasks
    {
        public static int IslandHeight { get => Main.maxTilesY / 2 - Main.maxTilesY / 5; }

        public static void ClearWorld()
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

            UltimateSkyblock.Instance.Logger.Info("Successfully cleared world");
        }

        public static void Cleanup()
        {
            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = 0; y < Main.maxTilesY; y++)
                {
                    // next 2 checks are literally only because I'm a lazy fuck.
                    // Since many of the structures already had Hellstone Brick in them, I'd have to go back and re-capture a TON.
                    // Eventually, in the future, I'll have to do that anyways, but for now this is a simple fix, albeit not the best.

                    Tile tile = Framing.GetTileSafely(x, y);
                    if (tile.HasTile && tile.TileType == TileID.HellstoneBrick && y < Main.maxTilesY / 2)
                    {
                        WorldGen.PlaceTile(x, y, ModContent.TileType<CrenelatedStoneTile>(), forced: true);
                    }

                    if (tile.WallType == WallID.HellstoneBrick && y < Main.maxTilesY / 2)
                    {
                        WorldGen.PlaceWall(x, y, ModContent.WallType<CrenelatedStoneWallTile>());
                    }

                }
            }
        }

        public static void GenerateDungeon(Slice slice)
        {
            // y - 44
            // x - 86

            // x - 169

            int yOffset = 86;
            int xOffset = 45;

            string path = "Content/SkyblockWorldGen/Structures/DungeonCastle" + (WorldGen.crimson ? "Crimson" : "Corrupt");
            if (slice.CenterInWorld < Main.maxTilesX / 2)
            {
                path += "Left";
                xOffset = 166;
            }

            Point16 placePoint = new Point16(slice.CenterInWorld, IslandHeight + WorldGen.genRand.Next(-25, 25));

            Generator.GenerateStructure(path, placePoint, UltimateSkyblock.Instance);
            Main.dungeonX = placePoint.X + xOffset;
            Main.dungeonY = placePoint.Y + yOffset;
        }

        public static void GenerateMushroom(Slice slice)
        {
            Generator.GenerateStructure("Content/SkyblockWorldGen/Structures/MushroomIsland", new Point16(WorldGen.genRand.Next(slice.LengthMin + slice.Length / 4, slice.LengthMax - slice.Length / 4), IslandHeight + WorldGen.genRand.Next(-40, 40)), UltimateSkyblock.Instance);
        }

        public static void GenerateEvilIslands(Slice slice) { }

        /// <summary>
        /// Handles generating the spawn island, as well as the starting chest.
        /// </summary>
        public static void GenerateStartingPlatform()
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

            Generator.GenerateStructure(WorldHelpers.forestPath + "Main", new Point16(x, y), UltimateSkyblock.Instance); // Generate the spawn island
            GenForestIslands(new Point16(x, y + 50));

            var logger = UltimateSkyblock.Instance.Logger;
            logger.Info("Starter chest style: " + config.StarterChestStyle);

            if (config.StarterChestStyle != ChestType.None) // Setting loot for starter chest
            {
                int starterChest = WorldGen.PlaceChest(Main.maxTilesX / 2 - 22, Main.maxTilesY / 2 - Main.maxTilesY / 5 - 5, 21, false, chestType);
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

                Generator.GenerateStructure(selectedIsland, genPoint, UltimateSkyblock.Instance);
                islandsToGenerate.RemoveAt(index);
            }
        }

        public static void GenerateHallowIslands(Slice slice)
        {
            Point16 center = new Point16(slice.CenterInWorld, IslandHeight);
            Generator.GenerateStructure(WorldHelpers.hallowPath + "Big", center, UltimateSkyblock.Instance);

            List<string> structures = new()
            {
                hallowPath + "Statue",
                hallowPath + "Lamp",
                hallowPath + "Campfire",
                //hallowPath + "Cave",
                //hallowPath + "Lamp"
            };

            for (int i = 0; i < 3; i++)
            {
                Point16 genPoint = i switch
                {
                    0 => new Point16(slice.LengthMin + slice.Length / 5 + WorldGen.genRand.Next(-20, 30), IslandHeight + slice.Length / 4 + WorldGen.genRand.Next(-30, 10)), // top left
                    1 => new Point16(slice.LengthMin + slice.Length / 6 + WorldGen.genRand.Next(-10, 30), IslandHeight + WorldGen.genRand.Next(-10, 30)), // left
                    2 => new Point16(slice.CenterInWorld + WorldGen.genRand.Next(-25, 25), IslandHeight - slice.Length / 5 + WorldGen.genRand.Next(-20, 20)), // top
                    3 => new Point16(slice.LengthMax - slice.Length / 7, IslandHeight + WorldGen.genRand.Next(-20, 20)), // right
                    4 => new Point16(slice.CenterInWorld + WorldGen.genRand.Next(-50, 50), IslandHeight + slice.Length / 6 + WorldGen.genRand.Next(40)) // bottom
                };

                int index = WorldGen.genRand.Next(structures.Count);
                string islandToGenerate = structures[index];
                structures.RemoveAt(index);
                Generator.GenerateStructure(islandToGenerate, genPoint, UltimateSkyblock.Instance);
            }
        }

        public static void GenerateJungleIslands(Slice slice)
        {
            //17
            //42
            bool left = WorldGen.genRand.NextBool();
            Point16 mainPoint = new Point16(left ? slice.LengthMin + 20 + slice.Length / 4 : slice.LengthMax - 20 - slice.Length / 4, IslandHeight + WorldGen.genRand.Next(-20, 20));
            Point16 bridgePoint = new Point16(left ? slice.LengthMax - slice.Length / 4  + WorldGen.genRand.Next(-25, 5): slice.LengthMin + slice.Length / 5 + WorldGen.genRand.Next(-25, 5), IslandHeight - WorldGen.genRand.Next(10, 20));
            Point16 smallPoint = new Point16(slice.CenterInWorld + WorldGen.genRand.Next(-20, 20), IslandHeight - 50 - WorldGen.genRand.Next(20, 50));

            Generator.GenerateStructure(junglePath + "Main", mainPoint, UltimateSkyblock.Instance);
            Generator.GenerateStructure(junglePath + "Bridge", bridgePoint, UltimateSkyblock.Instance);
            Generator.GenerateStructure(junglePath + "Small", smallPoint, UltimateSkyblock.Instance);

            Generator.GenerateStructure("Content/SkyblockWorldGen/Structures/LockedJungleChest", new Point16(mainPoint.X + 13, mainPoint.Y + 42), UltimateSkyblock.Instance);

            GenTemple(slice);
            GenPlanteraArena(slice);
        }

        public static void GenerateDesertIslands(Slice slice)
        {
            Point16 center = new(slice.CenterInWorld, IslandHeight);

            List<string> islandsToGenerate = new List<string>
            {
                desertPath + "Camp",
                desertPath + "House",
                desertPath + "Tiny",
                desertPath + "Ruins",
            };

            Generator.GenerateStructure(desertPath + "Pyramid", center, UltimateSkyblock.Instance);

            for (int i = 0; i < 4; i++)
            {
                Point16 pos1 = new Point16(slice.LengthMin + slice.Length / 5 + WorldGen.genRand.Next(-20, 30), IslandHeight - slice.Length / 4 + WorldGen.genRand.Next(-30, 10)); // top left
                Point16 pos2 = new Point16(slice.LengthMin + WorldGen.genRand.Next(slice.Length - slice.Length / 8), IslandHeight + slice.Length / 6 + WorldGen.genRand.Next(40)); // bottom

                Point16 genPoint = i switch
                {
                    0 => WorldGen.genRand.NextBool() ? pos1 : pos2,
                    1 => new Point16(slice.LengthMin + slice.Length / 6 + WorldGen.genRand.Next(-10, 30), IslandHeight + WorldGen.genRand.Next(-10, 30)), // left
                    2 => new Point16(slice.CenterInWorld + WorldGen.genRand.Next(-25, 25), IslandHeight - slice.Length / 5 + WorldGen.genRand.Next(-20, 20)), // top
                    _ => new Point16(slice.LengthMax - slice.Length / 7, IslandHeight + WorldGen.genRand.Next(-20, 20)), // right
                };

                int index = Main.rand.Next(islandsToGenerate.Count);
                string island = islandsToGenerate[index];
                islandsToGenerate.RemoveAt(index);

                Generator.GenerateStructure(island, genPoint, UltimateSkyblock.Instance);
            }
        }

        /// <summary>
        /// Handles generating the snow islands.
        /// </summary>
        public static void GenerateSnowIslands(Slice slice)
        {
            Point16 center = new Point16(slice.CenterInWorld, IslandHeight);
            Generator.GenerateStructure(WorldHelpers.snowPath + "Castle", center, UltimateSkyblock.Instance);

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
                    0 => new Point16(slice.LengthMin + slice.Length / 5 + WorldGen.genRand.Next(-20, 30), IslandHeight - slice.Length / 4 + WorldGen.genRand.Next(-30, 10)), // top left
                    1 => new Point16(slice.LengthMin + slice.Length / 6 + WorldGen.genRand.Next(-10, 30), IslandHeight + WorldGen.genRand.Next(-10, 30)), // left
                    2 => new Point16(slice.CenterInWorld + WorldGen.genRand.Next(-25, 25), IslandHeight - slice.Length / 5 + WorldGen.genRand.Next(-20, 20)), // top
                    3 => new Point16(slice.LengthMax - slice.Length / 7, IslandHeight + WorldGen.genRand.Next(-20, 20)), // right
                    4 => new Point16(slice.CenterInWorld + WorldGen.genRand.Next(-50, 50), IslandHeight + slice.Length / 6 + WorldGen.genRand.Next(40)) // bottom
                };

                int index = WorldGen.genRand.Next(structures.Count);
                string islandToGenerate = structures[index];
                structures.RemoveAt(index);
                Generator.GenerateStructure(islandToGenerate, genPoint, UltimateSkyblock.Instance);
            }
        }

        public static void AddHellSlices()
        {
            foreach (Slice slice in Slices)
            {
                // Generates an island at the start, middle, and end of each slice.
                Generator.GenerateStructure(RollHellIsland(), new Point16(slice.LengthMin + slice.Length / 5, Main.UnderworldLayer + 40), UltimateSkyblock.Instance);
                Generator.GenerateStructure(RollHellIsland(), new Point16(slice.LengthMin + slice.Length / 2, Main.UnderworldLayer + 40), UltimateSkyblock.Instance);
                Generator.GenerateStructure(RollHellIsland(), new Point16(slice.LengthMax - slice.Length / 5, Main.UnderworldLayer + 40), UltimateSkyblock.Instance);

                GenHellPlanetoid(slice.CenterInWorld + WorldGen.genRand.Next(-slice.Length / 5, slice.Length / 5), Main.UnderworldLayer + 125);
            }
        }

        static string RollHellIsland()
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
                return RollHellIsland();
            }
            else
            {

                previousStructure = structureToGenerate;

                return structureToGenerate;
            }
        }

        public static void GenHellPlanetoid(int x, int y)
        {
            // Weird math to account for different world sizes
            Point placePoint = new(x, y);
            int size = WorldGen.genRand.Next(14, 19);
            ShapeData shapeData = new ShapeData();
            ShapeData shapeData2 = new ShapeData();
            WorldUtils.Gen(placePoint, new Shapes.Circle(size, size), new Actions.SetTile(TileID.Ash));
            WorldUtils.Gen(placePoint, new Shapes.Circle(size, size), new Actions.Blank().Output(shapeData));
            WorldUtils.Gen(placePoint, new Shapes.Circle(size - 1, size - 1), new Actions.Blank().Output(shapeData2));

            int mode = WorldGen.genRand.Next(2);


            if (mode == 0)
            {
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
            }
            else if (mode == 1)
            {
                WorldUtils.Gen(placePoint, new Shapes.Circle(WorldGen.genRand.Next(8, 12), WorldGen.genRand.Next(8, 12)), Actions.Chain(new GenAction[]
                {
                    new Modifiers.Dither(.4),
                    new Actions.SetTile(TileID.Hellstone),
                }));
            }

            WorldUtils.Gen(placePoint, new ModShapes.OuterOutline(shapeData, true), new Actions.SetTile(TileID.AshGrass));
            WorldUtils.Gen(placePoint, new ModShapes.OuterOutline(shapeData2, true), new Actions.SetTile(TileID.Ash));
        }

        /// <summary>
        /// Handles generating the Jungle Temple.
        /// </summary>
        public static void GenTemple(Slice slice)
        {
            Generator.GenerateStructure(templePath, new Point16(slice.LengthMin + slice.Length / 5, Main.maxTilesY / 2 + Main.maxTilesY / 5), UltimateSkyblock.Instance);
        }

        public static void GenPlanteraArena(Slice slice)
        {
            Point genPos = new(slice.LengthMax - slice.Length / 6, Main.maxTilesY / 2 + Main.maxTilesY / 5);

            ShapeData shapeData = new ShapeData();
            ShapeData shapeData2 = new ShapeData();
            WorldUtils.Gen(genPos, new Shapes.Circle(95, 95), new Actions.SetTile(TileID.Mud));
            WorldUtils.Gen(genPos, new Shapes.Circle(95, 95), new Actions.Blank().Output(shapeData));
            WorldUtils.Gen(genPos, new Shapes.Circle(70, 70), new Actions.Clear());
            WorldUtils.Gen(genPos, new Shapes.Circle(70, 70), new Actions.Blank().Output(shapeData2));
            ShapeData shapeData3 = shapeData2;
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
            Generator.GenerateStructure(path + "PlanteraAltarStructure", new Point16(x - 1, y - 12), UltimateSkyblock.Instance);

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
