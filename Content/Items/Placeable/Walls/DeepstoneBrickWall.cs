using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using UltimateSkyblock.Content.Tiles.Blocks;
using UltimateSkyblock.Content.Tiles.Walls;
using UltimateSkyblock.Content.Items.Placeable.Tiles;

namespace UltimateSkyblock.Content.Items.Placeable.Walls
{
    public class DeepstoneBrickWall : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 30;
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
            Item.createWall = ModContent.WallType<DeepstoneBrickWallTile>();
            Item.rare = ItemRarityID.White;
            Item.useTime = 10;
            Item.useTurn = true;
            Item.useAnimation = 15;
            ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.StoneSlabWall;
        }

        public override void AddRecipes()
        {
            CreateRecipe(4)
                .AddIngredient(ModContent.ItemType<DeepstoneBrick>())
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}
