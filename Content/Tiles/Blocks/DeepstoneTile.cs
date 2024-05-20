using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using UltimateSkyblock.Content.Subworlds;
using System.Diagnostics.CodeAnalysis;
using UltimateSkyblock.Utils;
using UltimateSkyblock.Content.Items.Placeable.Tiles;

namespace UltimateSkyblock.Content.Tiles.Blocks
{
    public class DeepstoneTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
 
            this.SetupTileMerge(Type);
            this.SetupTileMerge(TileID.Stone);
            this.SetupTileMerge(TileID.Ash);
            this.SetupTileMerge(ModContent.GetInstance<HardenedDeepstoneTile>());
            this.SetupTileMerge(ModContent.GetInstance<DeepsoilTile>());

            Main.tileLighted[Type] = false;
            Main.tileNoSunLight[Type] = false;
            Main.tileBlockLight[Type] = true;

            TileID.Sets.Stone[Type] = true;

            DustType = DustID.Stone;
            MinPick = 70;   
            MineResist = 1.3f;
            HitSound = SoundID.Tink;
            RegisterItemDrop(ModContent.ItemType<Deepstone>());

            AddMapEntry(new Color(33, 33, 33));
        }
    }
}
