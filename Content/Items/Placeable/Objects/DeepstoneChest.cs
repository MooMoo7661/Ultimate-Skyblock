using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using UltimateSkyblock.Content.Tiles.Blocks;
using UltimateSkyblock.Content.Tiles.Furniture;

namespace UltimateSkyblock.Content.Items.Placeable.Objects
{
    public class DeepstoneChest : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<DeepstoneChestTile>());
            Item.placeStyle = 0; // Use this to place the chest in its locked style
            Item.width = 26;
            Item.height = 22;
            Item.value = 500;
        }
    }
    
    public class DeepstoneChestKey : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 5; // Biome keys usually take 1 item to research instead.
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.GoldenKey);
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(gold: 3, silver: 42);
            Item.scale = 1.4f;
        }
    }
}
