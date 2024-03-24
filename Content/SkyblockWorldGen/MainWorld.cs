using SubworldLibrary;
using UltimateSkyblock.Content.Configs;

namespace UltimateSkyblock.Content.SkyblockWorldGen
{
    public partial class MainWorld : ModSystem
    {
        //This class is mainly for modifying the world's data, and not actually generating islands.
        //Island generation can be found in SkyblockWorldGen/IslandsGeneration.cs
        //Planetoids generation can be found in SkyblockWorldGen/PlanetoidGeneration.cs
        //Points for islands & planetoids are in SkyblockWorldGen/WorldHelpers.cs
        
        //Subworlds & subworld structures are found in Content/Subworlds
        //All of the subworld generation is split into a bunch of passes for better organization in Subworlds/Passes

        /// <summary> Used with RollHellIslands to pick an island that is not the same as the previous one.</summary>
        public static string previousStructure;

        public static float ScaleBasedOnWorldSizeX;
        public static float ScaleBasedOnWorldSizeY;
        public static WorldSizes WorldSize;

        public static SkyblockModConfig config = ModContent.GetInstance<SkyblockModConfig>();

        public enum WorldSizes
        {
            Small,
            Medium,
            Large,
            Invalid
        }
        public enum ChestType
        {
            Classic,
            Simple,
            Luxury,
            None
        }

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            //Removing a few tasks to try and speed up the worldgen process.
            //The reason I don't just remove every pass is because many things, like the world layers, backgrounds, etc. rely on those passes being called.
            //An example of that is for some reason, Jungle Temple bricks become very messed up when the task isn't run.

            tasks.RemoveAll(task => task.Name == "Full Desert" || task.Name == "Buried Chests" || task.Name == "Mushroom Patches" ||
            task.Name == "Micro Biomes" || task.Name == "Moss" || task.Name == "Guide");
        }

        public override void OnWorldLoad()
        {
            SetWorldSizeVars();
            LogInfo();
            if (!SubworldSystem.AnyActive())
            SetWorldLayerHeights();
            SetExtractionTypes();

            // Angler can move in at any time
            NPC.savedAngler = true;
            Main.townNPCCanSpawn[NPCID.Angler] = true;
        }

        public override void PreWorldGen() => SetWorldSizeVars();

        private void SetWorldSizeVars()
        {
            WorldSize = WorldGen.GetWorldSize() switch
            {
                0 => WorldSizes.Small,
                1 => WorldSizes.Medium,
                _ => WorldSizes.Large,
            };

            ScaleBasedOnWorldSizeX = WorldGen.GetWorldSize() switch
            {
                0 => 1,
                1 => 30,
                2 => 60,
                _ => 80,

            };

            ScaleBasedOnWorldSizeY = WorldGen.GetWorldSize() switch
            {
                0 => 20,
                1 => 30,
                2 => 40,
                _ => 50,
            };
        }

        private void LogInfo()
        {
            Mod.Logger.Info("World Size : " + WorldSize);
            Mod.Logger.Info("World Size Scale X : " + ScaleBasedOnWorldSizeX);
            Mod.Logger.Info("World Size Scale Y : " + ScaleBasedOnWorldSizeY);
            Mod.Logger.Info("Dungeon Side : " + (dungeonLeft ? "Left" : "Right"));
        }

        private void SetWorldLayerHeights()
        {
            Main.worldSurface = Main.maxTilesY / 2;
            GenVars.worldSurfaceHigh = Main.maxTilesY / 2 - 200;
            GenVars.worldSurfaceLow = Main.maxTilesY / 2 + 200;
            Main.rockLayer = GenVars.worldSurfaceLow;
            GenVars.rockLayerHigh = GenVars.worldSurfaceLow;
            GenVars.rockLayerLow = GenVars.worldSurfaceLow + 200;
        }

        private void SetExtractionTypes()
        {
            if (ModContent.GetInstance<SkyblockModConfig>().DirtAndSandCanBeExtracted)
            {
                ItemID.Sets.ExtractinatorMode[ItemID.DirtBlock] = 0;
                ItemID.Sets.ExtractinatorMode[ItemID.SandBlock] = 0;
            }
        }

        /// Overridden to prevent the "spreading evil" tasks, as they can completely ruin islands with stone on them. The hallow is manually generated in this as well.
        /// </summary>
        public override void ModifyHardmodeTasks(List<GenPass> list)
        {
            list.RemoveAll(task => task.Name != "Hardmode Announcement");

            GenHallowedIslands();
        }
    }
}