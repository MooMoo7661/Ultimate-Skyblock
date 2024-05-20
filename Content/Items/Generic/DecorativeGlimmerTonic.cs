using UltimateSkyblock.Content.Buffs;
using UltimateSkyblock.Content.Tiles.Extractinators;
using UltimateSkyblock.Content.Tiles.Furniture;
namespace UltimateSkyblock.Content.Items.Generic
{
    public class DecorativeGlimmerTonic : ModItem
    {

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 5;
        }
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;

            Item.placeStyle = 0;
            Item.consumable = true;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.maxStack = Item.CommonMaxStack;
            Item.createTile = ModContent.TileType<GlimmerTonicDecoration>();
            Item.rare = ItemRarityID.Blue;
            Item.useTime = 10;
            Item.useTurn = true;
            Item.useAnimation = 15;
        }


        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<GlimmerTonic>())
                .Register();
        }
    }
}
