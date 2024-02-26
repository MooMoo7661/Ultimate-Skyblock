using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static WorldHelpers;
using static UltimateSkyblock.UltimateSkyblock;
using static Terraria.WorldGen;
using StructureHelper;
using UltimateSkyblock.Content.Utils;

namespace UltimateSkyblock.SkyblockWorldGen
{
    public partial class MainWorld : ModSystem
    {
        public static bool TempleGen_ = true;
        public static bool dungeonLeft;

        public override void PostWorldGen()
        {
            Main.dungeonX = dungeonLeft ? Main.maxTilesX / 20 : Main.maxTilesX - (Main.maxTilesX / 20);
            Main.dungeonY = Main.maxTilesY / 2 - Main.maxTilesY / 5 + Main.rand.Next(-20, 20);

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

            Mod.Logger.Info("Successfully cleared world");

            Main.spawnTileX = Main.maxTilesX / 2;
            Main.spawnTileY = Main.maxTilesY / 2 - Main.maxTilesY / 5; // Re-adjusting the spawn point to a constant value.

            dungeonLeft = (Main.dungeonX < Main.maxTilesX / 2) ? true : false;

            GenStartingPlatform();
            GenDungeonPlatform();
            GenUnderworldIslands();
            GenSnowIslands();
            GenChlorophytePlanetoids();
            GenHivePlanetoids();
            GenForestPlanetoids();
            GenSnowPlanetoids();
            GenEvilPlanetoids();
            GenJungleIslands();
            GenMushroomIsland();
            GenMeteorites();

            //PlaceTile(Main.dungeonX, Main.dungeonY, TileID.Adamantite, true, true); // Places tile at the spawn point for the Old Man and the Lunatic Cultists. For testing purposes.
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
                case 2:
                    chestType = 1; break;

                case 3:
                    chestType = 13; break;
            }

            x = Main.maxTilesX / 2 - 38; // Recenters the X coordinate due to islands spawning from the top left of the build
            y = Main.maxTilesY / 2 - Main.maxTilesY / 5 - 25; // Recenters the Y coordinate due to islands spawning from the top left of the build

            Generator.GenerateStructure(WorldHelpers.forestPath + "Main", new Point16(x, y), Instance); // Generate the spawn island
            GenForestIslands(new Point16(x, y + 50));

            if (config.StarterChestStyle != 4) // Setting loot for starter chest
            {
                int starterChest = PlaceChest(Main.maxTilesX / 2 - 22, Main.maxTilesY / 2 - Main.maxTilesY / 5 - 5, 21, false, chestType);
                Chest chest = Main.chest[starterChest];
                ChestType style = (ChestType)config.StarterChestStyle;

                switch (style)
                {
                    case ChestType.Classic:
                        chest.Add(new Item(ItemID.LavaBucket));
                        chest.Add(new Item(ItemID.WaterBucket));
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
            int offset = WorldSize switch
            {
                WorldSizes.Large => 50,
                _ => 20
            };

            Generator.GenerateStructure(WorldHelpers.snowPath + "Castle", Snow, Instance);
            Point16 center = Snow;

            List<string> structures = new List<string>
            {
                snowPath + "Cluster",
                snowPath + "Igloo",
                snowPath + "Pointy",
                snowPath + "Tiny"
            };

            for (int i = 0; i < 4; i++)
            {
                Point16 genPoint = i switch
                {
                    0 => center,
                    1 => Point16.Zero,
                    2 => Point16.Zero,
                    _ => Point16.Zero
                };

                int index = WorldGen.genRand.Next(structures.Count);
                string islandToGenerate = structures[index];
                structures.RemoveAt(index);
                Generator.GenerateStructure(islandToGenerate, genPoint, Instance);
            }
        }

        /// <summary>
        /// Handles generating the Jungle Temple. Called upon killing Plantera, and sets a bool preventing it from being generated again.
        /// </summary>
        /// <param name="executeRegardlessOfRestrictions">Can be used to re-generate the jungle temple without having to reset the bool associated with it. If unsure, pass false.</param>
        public static void GenTemple(bool executeRegardlessOfRestrictions)
        {
            if (!TempleGen_ && !executeRegardlessOfRestrictions) { return; }

            Generator.GenerateStructure(templePath, new Point16(Jungle.X, Jungle.Y + 100 + (int)(ScaleBasedOnWorldSizeY * 3)), Instance);
            TempleGen_ = false;
        }

        public static void GenJungleIslands()
        {
            //17
            //42

            Point16 genPoint = new Point16(Jungle.X + 200 + (int)(ScaleBasedOnWorldSizeX * 1.85f), Jungle.Y - (int)(ScaleBasedOnWorldSizeY * 10f) + Main.rand.Next(-10, 30));
            Generator.GenerateStructure(junglePath + "Main", genPoint, Instance);
            //PlaceTile(genPoint.X, genPoint.Y, TileID.Adamantite, true, true);
            Generator.GenerateStructure("Content/SkyblockWorldGen/Structures/LockedJungleChest", new Point16(genPoint.X + 13, genPoint.Y + 42), Instance);
        }

        public static void GenMushroomIsland()
        {
            Point16 genPos = new(Mushroom.X, Mushroom.Y);

            Generator.GenerateStructure("Content/SkyblockWorldGen/Structures/MushroomIsland", genPos, Instance);
        }
    }
        
    public class PlanteraTempleGeneration : GlobalNPC
    {
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => entity.type == NPCID.Plantera;

        public override void OnKill(NPC npc)
        {
            if (!NPC.downedPlantBoss)
            {
                MainWorld.GenTemple(false);
                Main.NewText("A mysterious temple has emerged from the depths of the Jungle...");
            }
        }
    }
}
    