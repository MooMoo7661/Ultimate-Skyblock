using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using UltimateSkyblock.Content.Tiles.Blocks;

namespace UltimateSkyblock.Content.Items.Placeable
{
    public class DeepstoneGravel : ModItem
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
            Item.createTile = ModContent.TileType<DeepstoneGravelTile>();
            Item.rare = ItemRarityID.White;
            Item.useTime = 10;
            Item.useTurn = true;
            Item.useAnimation = 15;
            ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.SiltBlock;
        }
    }
}
