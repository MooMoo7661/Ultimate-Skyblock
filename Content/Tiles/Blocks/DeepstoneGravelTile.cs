using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using UltimateSkyblock.Content.Items.Placeable;

namespace UltimateSkyblock.Content.Tiles.Blocks
{
    public class DeepstoneGravelTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileMerge[Type][Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileMerge[Type][TileID.Stone] = true;
            Main.tileMerge[Type][ModContent.TileType<DeepstoneTile>()] = true;
            Main.tileMerge[ModContent.TileType<DeepstoneTile>()][Type] = true;
            Main.tileMerge[TileID.Stone][Type] = true;
            Main.tileMerge[Type][TileID.Ash] = true;
            Main.tileMerge[TileID.Ash][Type] = true;
            Main.tileLighted[Type] = false;
            Main.tileNoSunLight[Type] = false;
            Main.tileBlockLight[Type] = true;
            DustType = DustID.Silt;
            MinPick = 45;

            HitSound = SoundID.Dig;
            RegisterItemDrop(ModContent.ItemType<DeepstoneGravel>());

            AddMapEntry(new Color(80, 80, 80));
            
        }
    }
}
