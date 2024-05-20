using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using UltimateSkyblock.Content.Subworlds;
using System.Diagnostics.CodeAnalysis;
using UltimateSkyblock.Content.Items.Placeable.Tiles;

namespace UltimateSkyblock.Content.Tiles.Blocks
{
    public class HardenedDeepstoneTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileMerge[Type][Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileMerge[Type][TileID.Stone] = true;
            Main.tileMerge[TileID.Stone][Type] = true;
            Main.tileMerge[Type][TileID.Ash] = true;
            Main.tileMerge[TileID.Ash][Type] = true;

            Main.tileMerge[ModContent.TileType<HardenedDeepstoneTile>()][Type] = true;
            Main.tileMerge[Type][ModContent.TileType<HardenedDeepstoneTile>()] = true;

            Main.tileLighted[Type] = false;
            Main.tileNoSunLight[Type] = false;
            Main.tileBlockLight[Type] = true;
            TileID.Sets.Stone[Type] = true;
            DustType = DustID.Stone;
            MinPick = 70;   
            MineResist = 1.7f;

            HitSound = SoundID.Tink;
            RegisterItemDrop(ModContent.ItemType<HardenedDeepstone>());

            AddMapEntry(new Color(20, 20, 20));
        }
    }
}
