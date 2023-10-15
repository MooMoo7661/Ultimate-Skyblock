using Terraria.Enums;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace OneBlock
{
	public class OneBlock : Mod
	{
        public static OneBlock Instance;
        public override void Load()
        {
            On_WorldGen.ShakeTree += On_WorldGen_ShakeTree;
            Instance = this;
            OB_Liquid.Load();
        }

        public override void Unload()
        {
            On_WorldGen.ShakeTree -= On_WorldGen_ShakeTree;
        }
        private void On_WorldGen_ShakeTree(On_WorldGen.orig_ShakeTree orig, int i, int j)
        {
            orig(i, j);

            var config = ModContent.GetInstance<OneBlockModConfig>();

            if (!config.TreesDropMoreAcorns)
            {
                return;
            }

            WorldGen.GetTreeBottom(i, j, out int x, out int y);

            TreeTypes treeType = WorldGen.GetTreeType((int)Main.tile[x, y].TileType);

            if (treeType == TreeTypes.None)
            {
                return;
            }

            if (!Main.dedServ)
            {
                y--;
                while (y > 10 && Main.tile[x, y].HasTile && TileID.Sets.IsShakeable[(int)Main.tile[x, y].TileType]) { y--; }

                y++;

                if (!WorldGen.IsTileALeafyTreeTop(x, y) || Collision.SolidTiles(x - 2, x + 2, y - 2, y + 2))
                {
                    return;
                }

                if (Main.rand.NextBool(8))
                {
                   Item.NewItem(WorldGen.GetItemSource_FromTileBreak(x, y), new Vector2(x * 16, y * 16), Vector2.Zero, ItemID.Acorn, Stack: 1);
                }
            }
        }
    }
}