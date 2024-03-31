using UltimateSkyblock.Content.Tiles.Environment;

using static UltimateSkyblock.Content.SkyblockWorldGen.Slice;
using static UltimateSkyblock.Content.SkyblockWorldGen.SliceGenerationTasks;
using static UltimateSkyblock.Content.SkyblockWorldGen.PlanetoidGeneration;

namespace UltimateSkyblock.Content.SkyblockWorldGen
{
    public class IslandHandler : ModSystem
    {
        public static bool DungeonLeft;
        public static string previousStructure;

        public static Point16 Jungle;

        public static List<Slice> Slices = new List<Slice>();
        public static int SliceCount = 10;

        public static List<SliceGenerationInfo> OutsideIslands = new List<SliceGenerationInfo>();
        public static List<SliceGenerationInfo> IslandsLeft = new List<SliceGenerationInfo>();
        public static List<SliceGenerationInfo> IslandsRight = new List<SliceGenerationInfo>();

        public override void PostWorldGen()
        {
            DungeonLeft = (Main.dungeonX < Main.maxTilesX / 2) ? true : false;
            Main.dungeonX = DungeonLeft ? Main.maxTilesX / 20 : Main.maxTilesX - (Main.maxTilesX / 20);
            Main.dungeonY = Main.maxTilesY / 2 - Main.maxTilesY / 5 + WorldGen.genRand.Next(-20, 20);

            Main.spawnTileX = Main.maxTilesX / 2;
            Main.spawnTileY = Main.maxTilesY / 2 - Main.maxTilesY / 5; // Re-adjusting the spawn point to a constant value.

            SliceGenerationTasks.ClearWorld();

            CreateSlices();
            AddSliceInfo();
            InvokeSliceGeneration();
            PlanetoidRunner();

            AddHellSlices();

            GenerateStartingPlatform();

            Cleanup();
        }

        /// <summary>
        /// Divides the world into 10 equal sections, then assigns "Slices" to each section.
        /// <br>Slices store info such as width, center, name, and more.</br>
        /// </summary>
        public void CreateSlices()
        {
            Slices = new List<Slice>();
            int sliceSize = Main.maxTilesX / SliceCount;

            for (int i = 0; i < SliceCount; i++)
            {
                Slice slice = new Slice(sliceSize * i, sliceSize * i + sliceSize);
                slice.Index = i;
                ModifySliceFromIndex(ref slice);
                Slices.Add(slice);
            }
        }

        public virtual void AddSliceInfo()
        {
            OutsideIslands.Add(new SliceGenerationInfo("Mushroom", new IslandGenerationEvent(GenerateMushroom)));

            List<SliceGenerationInfo> dungeonSlices = new List<SliceGenerationInfo>
            {
                    new SliceGenerationInfo("Snow", new IslandGenerationEvent(GenerateSnowIslands)),
                    new SliceGenerationInfo("Evil", new IslandGenerationEvent(GenerateEvilIslands)),
                    new SliceGenerationInfo("Hallow", new IslandGenerationEvent(GenerateHallowIslands)),
            };

            List<SliceGenerationInfo> notDungeonSlices = new List<SliceGenerationInfo>
            {
                    new SliceGenerationInfo("Jungle", new IslandGenerationEvent(GenerateJungleIslands)),
                    new SliceGenerationInfo("Desert", new IslandGenerationEvent(GenerateDesertIslands)),
                    new SliceGenerationInfo("Pending", new IslandGenerationEvent(GenerateEvilIslands)),
            };

            // Randomly orders the lists
            for (int i = 0; i < 3; i++)
            {
                int index = WorldGen.genRand.Next(dungeonSlices.Count);
                var slice = dungeonSlices[index];
                IslandsLeft.Add(slice);
                dungeonSlices.RemoveAt(index);

                index = WorldGen.genRand.Next(notDungeonSlices.Count);
                slice = notDungeonSlices[index];
                IslandsRight.Add(slice);
                notDungeonSlices.RemoveAt(index);
            }


            if (DungeonLeft)
            {
                Slices[9].Name = "Mushroom";
                Slices[9].IslandGeneration += new IslandGenerationEvent(GenerateMushroom);

                Slices[0].Name = "Dungeon";
                Slices[0].IslandGeneration += new IslandGenerationEvent(GenerateDungeon);

                for (int i = 0; i < 3; i++)
                {
                    Slices[1 + i].Name = IslandsLeft[i].Name;
                    Slices[1 + i].IslandGeneration += IslandsLeft[i].Event;
                }

                for (int i = 0; i < 3; i++)
                {
                    Slices[6 + i].Name = IslandsRight[i].Name;
                    Slices[6 + i].IslandGeneration += IslandsRight[i].Event;
                }
            }
            else
            {
                Slices[0].Name = "Mushroom";
                Slices[0].IslandGeneration += new IslandGenerationEvent(GenerateMushroom);

                Slices[9].Name = "Dungeon";
                Slices[9].IslandGeneration += new IslandGenerationEvent(GenerateDungeon);

                for (int i = 0; i < 3; i++)
                {
                    Slices[1 + i].Name = IslandsRight[i].Name;
                    Slices[1 + i].IslandGeneration += IslandsRight[i].Event;
                }

                for (int i = 0; i < 3; i++)
                {
                    Slices[6 + i].Name = IslandsLeft[i].Name;
                    Slices[6 + i].IslandGeneration += IslandsLeft[i].Event;
                }
            }
        }

        public static void ModifySliceFromIndex(ref Slice slice)
        {
            if (slice.Index == 4 || slice.Index == 5)
            {
                slice.Name = "Forest";
            }
        }

        /// <summary>
        /// This is ran after all modifications to slices.
        /// <br>Handles calling the method hooked to each slice.</br>
        /// </summary>
        public void InvokeSliceGeneration()
        {
            foreach (Slice slice in Slices)
            {
                slice.InvokeIslandGeneration();
            }
        }
    }   
}
