using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using UltimateSkyblock.Content.Items.Placeable;
using UltimateSkyblock.Content.Subworlds;
using UltimateSkyblock.Content.Tiles.Environment.Foliage;

namespace UltimateSkyblock.Content.Tiles.Blocks
{
    public class DeepsoilTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileMerge[Type][Type] = true;
            Main.tileSolid[Type] = true;

            Main.tileMerge[ModContent.TileType<DeepstoneTile>()][Type] = true;
            Main.tileMerge[Type][ModContent.TileType<DeepstoneTile>()] = true;

            Main.tileLighted[Type] = false;
            Main.tileNoSunLight[Type] = false;
            Main.tileBlockLight[Type] = true;

            DustType = DustID.Mud;
            MinPick = 50;   
            MineResist = 1.4f;

            HitSound = SoundID.Dig;
            RegisterItemDrop(ModContent.ItemType<Deepsoil>());

            AddMapEntry(new Color(63, 31, 24));
        }

        public override void RandomUpdate(int x, int y)
        {
            Tile tileUp = Framing.GetTileSafely(x, y - 1);
            if (!tileUp.HasTile && Main.rand.NextBool(60))
            {
                if (Main.rand.NextBool(3))
                {
                    Tile tileUpRight = Framing.GetTileSafely(x + 1, y - 1);
                    if (!tileUpRight.HasTile)
                        WorldGen.PlaceTile(x, y - 1, ModContent.TileType<SplitGlimmercapTile>(), true);
                }
                else
                {
                    WorldGen.PlaceTile(x, y - 1, ModContent.TileType<GlimmercapTile>(), true);
                }
            }    
        }
    }
}
