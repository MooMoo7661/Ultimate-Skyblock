using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using UltimateSkyblock.Content.Tiles.Extractinators;

namespace UltimateSkyblock.Content.Items.Placeable
{
    public class AutoExtractorTier4 : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
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
            Item.createTile = ModContent.TileType<AutoExtractorTier4Tile>();
            Item.rare = ItemRarityID.Yellow;
            Item.useTime = 10;
            Item.useTurn = true;
            Item.useAnimation = 15;
            ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.Extractinator;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<AutoExtractorTier3>())
                .AddIngredient(ItemID.SpectreBar, 5)
                .AddIngredient(ItemID.SoulofLight, 15)
                .AddIngredient(ItemID.Diamond, 20)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
