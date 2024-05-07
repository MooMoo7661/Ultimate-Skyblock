using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using UltimateSkyblock.Content.Items.Placeable;

namespace UltimateSkyblock.Content.Tiles.Blocks
{
    public class DeepstoneBrickTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileMerge[Type][Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileMerge[Type][TileID.Stone] = true;
            Main.tileMerge[TileID.Stone][Type] = true;
            Main.tileLighted[Type] = false;
            Main.tileNoSunLight[Type] = false;
            Main.tileBlockLight[Type] = true;
            TileID.Sets.Stone[Type] = true;
            DustType = DustID.Stone;
            MinPick = 70;

            HitSound = SoundID.Tink;
            RegisterItemDrop(ModContent.ItemType<DeepstoneBrick>());

            AddMapEntry(new Color(27, 27, 27));
        }
    }
}
