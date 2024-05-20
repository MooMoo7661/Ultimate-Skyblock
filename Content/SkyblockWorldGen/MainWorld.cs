using SubworldLibrary;
using UltimateSkyblock.Content.Configs;
using static UltimateSkyblock.Content.SkyblockWorldGen.IslandHandler;

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

            List<string> tasksToRemove = new List<string>
            {
                "Terrain",
                "Reset",
                "Jungle",
                "Jungle Temple",
                "Temple",
                "Dungeon",
                "Settle Liquids Again",

            };

            tasks.RemoveAll(task => !tasksToRemove.Contains(task.Name));
        }

        public override void OnWorldLoad()
        {
            if (!UltimateSkyblock.IsSkyblock())
                return;

            SetWorldSizeVars();
            LogInfo();
            if (!SubworldSystem.AnyActive())
            SetWorldLayerHeights();
            SetExtractionTypes();

            // Angler can move in at any time
            NPC.savedAngler = true;
            Main.townNPCCanSpawn[NPCID.Angler] = true;
        }

        public override void PreWorldGen()
        {
            SetWorldSizeVars();
        }

        private void SetWorldSizeVars()
        {
            WorldSize = WorldGen.GetWorldSize() switch
            {
                0 => WorldSizes.Small,
                1 => WorldSizes.Medium,
                _ => WorldSizes.Large,
            };
        }

        private void LogInfo()
        {
            Mod.Logger.Info("World Size : " + WorldSize);
            Mod.Logger.Info("Dungeon Side : " + (DungeonLeft ? "Left" : "Right"));
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
            if (UltimateSkyblock.IsSkyblock())
            list.RemoveAll(task => task.Name != "Hardmode Announcement");
        }
    }
}