using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ObjectData;
using Microsoft.Xna.Framework.Graphics;
using UltimateSkyblock.Content.Configs;
using UltimateSkyblock.Content.Items.Placeable;

namespace UltimateSkyblock.Content.Tiles.Environment
{
    public class CrenelatedStoneWallTile : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = false;

            DustType = DustID.Stone;

            AddMapEntry(new Color(112, 112, 112));
            RegisterItemDrop(WallID.Stone);
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}