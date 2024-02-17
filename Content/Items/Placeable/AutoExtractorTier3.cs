using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using UltimateSkyblock.Content.Tiles.Extractinators;
using UltimateSkyblock.Content.Items.Placeable;

namespace UltimateSkyblock.Content.Items.Placeable
{
    public class AutoExtractorTier3 : ModItem
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
            Item.createTile = ModContent.TileType<AutoExtractorTier3Tile>();
            Item.rare = ItemRarityID.Pink;
            Item.useTime = 10;
            Item.useTurn = true;
            Item.useAnimation = 15;
            ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.Extractinator;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<AutoExtractorTier2>())
                .AddIngredient(ItemID.HallowedBar, 5)
                .AddIngredient(ItemID.SoulofLight, 5)
                .AddIngredient(ItemID.Diamond, 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
