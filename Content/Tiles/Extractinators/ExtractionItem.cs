using MonoMod.RuntimeDetour;
using System;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace UltimateSkyblock.Content.Tiles.Extractinators
{
    internal class ExtractionItem : GlobalItem
    {
        public override void ExtractinatorUse(int extractType, int extractinatorBlockType, ref int resultType, ref int resultStack)
        {
            //if (resultType >= ItemID.CopperCoin && resultType <= ItemID.GoldCoin) {
            //    if (extractinatorBlockType == ModContent.TileType<AutoExtractorTier5Tile>() && Main.rand.NextBool(40))
            //        resultType = ItemID.PlatinumCoin;
            //}
        }

        #region Detours and Reflection

        private static Hook extractinatorHook;
        public override void Load()
        {
            extractinatorHook = new Hook(ItemLoaderExtractinatorUse, ItemLoader_ExtractinatorUse_Detour);
            extractinatorHook.Apply();
            On_Player.DropItemFromExtractinator += On_Player_DropItemFromExtractinator;
            On_Player.ExtractinatorUse += On_Player_ExtractinatorUse;
        }

        private void On_Player_ExtractinatorUse(On_Player.orig_ExtractinatorUse orig, Player self, int extractType, int extractinatorBlockType)
        {
            orig(self, extractType, extractinatorBlockType);
        }

        private void On_Player_DropItemFromExtractinator(On_Player.orig_DropItemFromExtractinator orig, Player self, int itemType, int stack)
        {
            orig(self, itemType, stack);
        }

        public override void Unload()
        {
            extractinatorHook.Undo();
        }

        public static readonly MethodInfo extractinatorUse = typeof(Player).GetMethod("ExtractinatorUse", BindingFlags.NonPublic | BindingFlags.Instance);
        public delegate void ExtractinatorUseDelegate(Player player, int extractType, int extractinatorBlockType);
        public static ExtractinatorUseDelegate ExtractinatorUseMethod = (ExtractinatorUseDelegate)Delegate.CreateDelegate(typeof(ExtractinatorUseDelegate), extractinatorUse);

        /// <summary>
        /// Needs to be paired with a detour around ItemLoader.ExtractinatorUse() and set stack to zero.
        /// </summary>
        /// <param name="extractType"></param>
        /// <param name="extractinatorBlockType"></param>
        public static void AutoExtractinatorUse(int extractType, int extractinatorBlockType, out int type, out int stack)
        {
            Extracting = true;
            ExtractinatorUseMethod(null, extractType, extractinatorBlockType);
            Extracting = false;
            type = extractItemType;
            stack = extractStack;
        }

        private static int extractItemType = 0;
        private static int extractStack = 0;
        public static bool Extracting = false;
        public delegate void orig_ItemLoader_ExtractinatorUse(ref int resultType, ref int resultStack, int extractType, int extractinatorBlockType);
        public delegate void hook_ItemLoader_ExtractinatorUse(orig_ItemLoader_ExtractinatorUse orig, ref int resultType, ref int resultStack, int extractType, int extractinatorBlockType);
        public static readonly MethodInfo ItemLoaderExtractinatorUse = typeof(ItemLoader).GetMethod("ExtractinatorUse", BindingFlags.Public | BindingFlags.Static);
        public static void ItemLoader_ExtractinatorUse_Detour(orig_ItemLoader_ExtractinatorUse orig, ref int resultType, ref int resultStack, int extractType, int extractinatorBlockType)
        {
            orig(ref resultType, ref resultStack, extractType, extractinatorBlockType);
            if (Extracting)
            {
                extractItemType = resultType;
                extractStack = resultStack;
                resultType = 0;
            }
        }

        #endregion
    }
}
