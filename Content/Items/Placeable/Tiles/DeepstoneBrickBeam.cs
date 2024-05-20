using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using UltimateSkyblock.Content.Tiles.Blocks;

namespace UltimateSkyblock.Content.Items.Placeable.Tiles
{
    public class DeepstoneBrickBeam : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.WoodenBeam);
            Item.ResearchUnlockCount = 50;
            Item.createTile = ModContent.TileType<DeepstoneBrickBeamTile>();
        }
    }
}
