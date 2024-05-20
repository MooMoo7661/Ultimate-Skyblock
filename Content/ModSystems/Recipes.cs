using UltimateSkyblock.Content.Configs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace UltimateSkyblock.Content.ModSystems
{
    public class Recipes : ModSystem
    {
        public static RecipeGroup tombstoneRecipeGroup;
        public static RecipeGroup goldenTombstoneRecipeGroup;
        public static RecipeGroup evilBars;
        public static RecipeGroup silverOrTungsten;
        public static RecipeGroup goldOrPlatinum;
        public override void AddRecipes()
        {
            var config = ModContent.GetInstance<RecipesConfig>();

            if (config.DecraftTombstones)
            {
                Recipe.Create(ItemID.StoneBlock, 3)
                    .AddRecipeGroup(tombstoneRecipeGroup)
                    .AddTile(TileID.WorkBenches)
                    .Register();

                Recipe.Create(ItemID.GoldBar)
                   .AddRecipeGroup(goldenTombstoneRecipeGroup)
                   .AddTile(TileID.Furnaces)
                   .Register();

                Recipe.Create(ItemID.Wood, 2)
                   .AddIngredient(ItemID.CrossGraveMarker)
                   .AddTile(TileID.WorkBenches)
                   .Register();

                Recipe.Create(ItemID.Wood, 2)
                   .AddIngredient(ItemID.GraveMarker)
                   .AddTile(TileID.WorkBenches)
                   .Register();
            }

            if (config.SmeltTinCans)
            {
                Recipe.Create(ItemID.TinBar)
                    .AddIngredient(ItemID.TinCan, 2)
                    .AddTile(TileID.Furnaces)
                    .Register();
            }

            if (config.CraftGelatinCrystals)
            {
                Recipe.Create(ItemID.QueenSlimeCrystal)
                    .AddIngredient(ItemID.CrystalShard, 5)
                    .AddIngredient(ItemID.Gel, 15)
                    .AddIngredient(ItemID.SoulofLight, 5)
                    .AddTile(TileID.WorkBenches)
                    .Register();
            }

            if (config.CraftExtractinator)
            {
                Recipe.Create(ItemID.Extractinator)
                    .AddRecipeGroup(RecipeGroupID.IronBar, 5)
                    .AddIngredient(ItemID.StoneBlock, 25)
                    .AddIngredient(ItemID.Chain, 10)
                    .AddIngredient(ItemID.Gel, 20)
                    .AddTile(TileID.WorkBenches)
                    .Register();
            }

            if (config.CraftRegrowthStaff)
            {
                Recipe.Create(ItemID.StaffofRegrowth)
                    .AddRecipeGroup(RecipeGroupID.Wood, 15)
                    .AddIngredient(ItemID.Vine, 3)
                    .AddTile(TileID.WorkBenches)
                    .Register();
            }

            if (config.CraftIceSkates)
            {
                Recipe.Create(ItemID.IceSkates)
                    .AddIngredient(ItemID.IceBlock, 15)
                    .AddIngredient(ItemID.Silk, 10)
                    .AddRecipeGroup(silverOrTungsten, 5)
                    .AddTile(TileID.Loom)
                    .Register();
            }

            if (config.CraftHermesBoots)
            {
                Recipe.Create(ItemID.HermesBoots)
                    .AddRecipeGroup(goldOrPlatinum, 10)
                    .AddIngredient(ItemID.Silk, 10)
                    .AddIngredient(ItemID.FallenStar, 10)
                    .AddTile(TileID.Loom)
                    .Register();
            }

            if (config.CraftIceMirror)
            {
                Recipe.Create(ItemID.IceMirror)
                    .AddRecipeGroup(goldOrPlatinum, 8)
                    .AddIngredient(ItemID.IceBlock, 15)
                    .AddIngredient(ItemID.Glass, 10)
                    .AddIngredient(ItemID.Diamond, 3)
                    .AddTile(TileID.Furnaces)
                    .Register();
            }

            if (config.CraftRegenerationBand)
            {
                Recipe.Create(ItemID.BandofRegeneration)
                    .AddRecipeGroup(silverOrTungsten, 10)
                    .AddIngredient(ItemID.LifeCrystal, 1)
                    .AddIngredient(ItemID.RegenerationPotion, 3)
                    .AddTile(TileID.WorkBenches)
                    .Register();
            }

            if (config.CraftStepStool)
            {
                Recipe.Create(ItemID.PortableStool)
                    .AddRecipeGroup(RecipeGroupID.Wood, 25)
                    .AddIngredient(ItemID.LifeCrystal, 1)
                    .AddTile(TileID.WorkBenches)
                    .Register();
            }

            if (config.CraftFlurryBoots)
            {
                Recipe.Create(ItemID.FlurryBoots)
                    .AddRecipeGroup(goldOrPlatinum, 10)
                    .AddIngredient(ItemID.Silk, 10)
                    .AddIngredient(ItemID.IceBlock, 25)
                    .AddIngredient(ItemID.SnowBlock, 15)
                    .AddTile(TileID.Loom)
                    .Register();
            }

            if (config.CraftDuneriderBoots)
            {
                Recipe.Create(ItemID.SandBoots)
                    .AddRecipeGroup(goldOrPlatinum, 10)
                    .AddIngredient(ItemID.Silk, 10)
                    .AddIngredient(ItemID.Sandstone, 25)
                    .AddIngredient(ItemID.SandBlock, 15)
                    .AddTile(TileID.Loom)
                    .Register();
            }

            if (config.CraftAglet)
            {
                Recipe.Create(ItemID.SandBoots)
                   .AddRecipeGroup(RecipeGroupID.IronBar, 5)
                   .AddIngredient(ItemID.SwiftnessPotion)
                   .AddTile(TileID.WorkBenches)
                   .Register();
            }

            if (config.CraftLifeCrystals)
            {
                Recipe.Create(ItemID.LifeCrystal)
                   .AddIngredient(ItemID.Ruby, 3)
                   .AddIngredient(ItemID.LesserHealingPotion, 1)
                   .AddTile(TileID.WorkBenches)
                   .Register();
            }

            if (config.PotionPlantDecrafting)
            {
                Recipe.Create(ItemID.DaybloomSeeds)
                    .AddIngredient(ItemID.Daybloom)
                    .Register();

                Recipe.Create(ItemID.BlinkrootSeeds)
                   .AddIngredient(ItemID.Blinkroot)
                   .Register();

                Recipe.Create(ItemID.DeathweedSeeds)
                   .AddIngredient(ItemID.Deathweed)
                   .Register();

                Recipe.Create(ItemID.FireblossomSeeds)
                   .AddIngredient(ItemID.Fireblossom)
                   .Register();

                Recipe.Create(ItemID.MoonglowSeeds)
                   .AddIngredient(ItemID.Moonglow)
                   .Register();

                Recipe.Create(ItemID.ShiverthornSeeds)
                   .AddIngredient(ItemID.Shiverthorn)
                   .Register();

                Recipe.Create(ItemID.WaterleafSeeds)
                   .AddIngredient(ItemID.Waterleaf)
                   .Register();
            }
        }

        public override void AddRecipeGroups()
        {
            tombstoneRecipeGroup = new RecipeGroup(() => "Any Stone Gravestone", ItemID.Tombstone, ItemID.Headstone, ItemID.Gravestone, ItemID.Obelisk);
            RecipeGroup.RegisterGroup($"{nameof(UltimateSkyblock)}:GravestonesDecraftToStone", tombstoneRecipeGroup);

            goldenTombstoneRecipeGroup = new RecipeGroup(() => "Any Golden Gravestone", ItemID.RichGravestone1, ItemID.RichGravestone2, ItemID.RichGravestone3, ItemID.RichGravestone4, ItemID.RichGravestone5);
            RecipeGroup.RegisterGroup($"{nameof(UltimateSkyblock)}:GoldenGravestonesDecraftToGold", goldenTombstoneRecipeGroup);

            evilBars = new RecipeGroup(() => "Any Evil Bar", ItemID.CrimtaneBar, ItemID.DemoniteBar);
            RecipeGroup.RegisterGroup($"{nameof(UltimateSkyblock)}:AnyEvilBar", evilBars);

            silverOrTungsten = new RecipeGroup(() => "Silver or Tungsten", ItemID.SilverBar, ItemID.TungstenBar);
            RecipeGroup.RegisterGroup($"{nameof(UltimateSkyblock)}:SilverOrTungsten", silverOrTungsten);

            goldOrPlatinum = new RecipeGroup(() => "Gold or Platinum", ItemID.GoldBar, ItemID.PlatinumBar);
            RecipeGroup.RegisterGroup($"{nameof(UltimateSkyblock)}", goldOrPlatinum);
        }
    }
}