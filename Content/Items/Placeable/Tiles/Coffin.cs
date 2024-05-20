using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using UltimateSkyblock.Content.Tiles.Blocks;
using UltimateSkyblock.Content.Tiles.Furniture;

namespace UltimateSkyblock.Content.Items.Placeable.Tiles
{
    public class Coffin : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 108;
            Item.placeStyle = 0;
            Item.consumable = true;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.maxStack = Item.CommonMaxStack;
            Item.createTile = ModContent.TileType<CoffinTile>();
            Item.rare = ItemRarityID.White;
            Item.useTime = 10;
            Item.useTurn = true;
            Item.useAnimation = 15;
        }

        public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
        }
    }
}
