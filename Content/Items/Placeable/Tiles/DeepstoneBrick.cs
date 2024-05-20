using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using UltimateSkyblock.Content.Tiles.Blocks;
using UltimateSkyblock.Content.Items.Placeable.Walls;

namespace UltimateSkyblock.Content.Items.Placeable.Tiles
{
    public class DeepstoneBrick : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 50;
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
            Item.createTile = ModContent.TileType<DeepstoneBrickTile>();
            Item.rare = ItemRarityID.Blue;
            Item.useTime = 10;
            Item.useTurn = true;
            Item.useAnimation = 15;
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<Deepstone>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Deepstone>(), 2)
                .AddTile(TileID.WorkBenches)
                .Register();

            CreateRecipe()
               .AddIngredient(ModContent.ItemType<DeepstoneBrickWall>(), 4)
               .AddTile(TileID.WorkBenches)
               .Register();

        }
    }
}
