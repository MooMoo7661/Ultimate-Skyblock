using OneBlock.Items.Placeable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace OneBlock.Tiles.Blocks
{
    public class VolcanicStoneTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileMerge[Type][Type] = true;
            Main.tileMerge[Type][TileID.Ash] = true;
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileMerge[Type][TileID.Stone] = true;
            Main.tileLighted[Type] = false;
            Main.tileMerge[Type][TileID.LavaMoss] = true;
            Main.tileNoSunLight[Type] = false;
            Main.tileBlockLight[Type] = true;
            Main.tileMerge[Type][TileID.LavaMoss] = true;
            Main.tileMerge[Type][TileID.AshGrass] = true;
            TileID.Sets.Stone[Type] = true;
            DustType = DustID.Ash;

            HitSound = SoundID.Tink;
            RegisterItemDrop(ModContent.ItemType<VolcanicStone>());

            AddMapEntry(Color.Black);
        }
    }
}
