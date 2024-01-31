using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using StructureHelper;
using OneBlock.Content.Configs;

namespace OneBlock.SkyblockWorldGen
{
    public partial class MainWorld : ModSystem
    {
        /// <summary>
        /// Used with RollHellIslands to pick an island that is not the same as the previous one.
        /// </summary>
        public static string previousStructure;
        public static float ScaleBasedOnWorldSizeX;
        public static float ScaleBasedOnWorldSizeY;
        public static WorldSizes WorldSize;

        public static OneBlockModConfig config = ModContent.GetInstance<OneBlockModConfig>();
        public enum WorldSizes
        {
            Small,
            Medium,
            Large
        }
        public enum ChestType
        {
            Classic,
            Simple,
            Luxury
        }
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            tasks.RemoveAll(task => task.Name == "Full Desert" || task.Name == "Buried Chests" || task.Name == "Mushroom Patches" ||
            task.Name == "Micro Biomes" || task.Name == "Moss" || task.Name == "Guide");
        }

        public override void PreWorldGen()
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

        public override void OnWorldLoad()
        {
            WorldSize = WorldGen.GetWorldSize() switch
            {
                0 => WorldSizes.Small,
                1 => WorldSizes.Medium,
                _ => WorldSizes.Large,
            };

            NPC.savedAngler = true;
            Main.townNPCCanSpawn[NPCID.Angler] = true;
        }

        /// <summary>
        /// Overridden to prevent the "spreading evil" tasks, as they can completely ruin islands with stone on them. The hallow is manually generated in this as well.
        /// </summary>
        public override void ModifyHardmodeTasks(List<GenPass> list)
        {
            list.RemoveAll(task => task.Name != "Hardmode Announcement");

            GenHallowedIslands();
        }
    }
    

    public class RefinableDirt : GlobalItem
    {
        public override void SetStaticDefaults()
        {
            if (ModContent.GetInstance<OneBlockModConfig>().DirtAndSandCanBeExtracted)
            {
                ItemID.Sets.ExtractinatorMode[ItemID.DirtBlock] = 0;
                ItemID.Sets.ExtractinatorMode[ItemID.SandBlock] = 0;
            }
        }
    }
}