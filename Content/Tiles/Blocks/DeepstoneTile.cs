using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using UltimateSkyblock.Content.Items.Placeable;
using UltimateSkyblock.Content.Subworlds;
using System.Diagnostics.CodeAnalysis;

namespace UltimateSkyblock.Content.Tiles.Blocks
{
    public class DeepstoneTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileMerge[Type][Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileMerge[Type][TileID.Stone] = true;
            Main.tileMerge[TileID.Stone][Type] = true;
            Main.tileMerge[Type][TileID.Ash] = true;
            Main.tileMerge[TileID.Ash][Type] = true;

            Main.tileMerge[ModContent.TileType<DeepstoneBrickTile>()][Type] = true;
            Main.tileMerge[Type][ModContent.TileType<DeepstoneBrickTile>()] = true;

            Main.tileLighted[Type] = false;
            Main.tileNoSunLight[Type] = false;
            Main.tileBlockLight[Type] = true;
            TileID.Sets.Stone[Type] = true;
            DustType = DustID.Stone;
            MinPick = 70;   
            MineResist = 2f;

            HitSound = SoundID.Tink;
            RegisterItemDrop(ModContent.ItemType<Deepstone>());

            AddMapEntry(new Color(27, 27, 27));
        }
    }
}
