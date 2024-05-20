using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ObjectData;
using Microsoft.Xna.Framework.Graphics;
using UltimateSkyblock.Content.Configs;
using UltimateSkyblock.Content.Items.Placeable.Tiles;

namespace UltimateSkyblock.Content.Tiles.Blocks
{
    public class SlateTile : ModTile
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
            MinPick = 45;

            HitSound = SoundID.Tink;
            RegisterItemDrop(ModContent.ItemType<Slate>());

            AddMapEntry(new Color(101, 93, 83));
        }
    }
}