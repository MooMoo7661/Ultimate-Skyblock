using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using UltimateSkyblock.Content.Subworlds;
using UltimateSkyblock.Content.Tiles.Environment.Foliage;
using UltimateSkyblock.Content.Items.Placeable.Tiles;

namespace UltimateSkyblock.Content.Tiles.Blocks
{
    public class DeepstoneBrickBeamTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            TileID.Sets.IsBeam[Type] = true;
            
            AddMapEntry(Color.SlateGray);
            DustType = DustID.Stone;
        }
    }
}
