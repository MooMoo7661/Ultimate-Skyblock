using UltimateSkyblock.Content.Configs;
using static Terraria.WorldGen;

namespace UltimateSkyblock.Content.GlobalClasses
{
    public class GlobalSapling : GlobalTile
    {
        public override void RandomUpdate(int i, int j, int type)
        {
            var config = ModContent.GetInstance<SkyblockModConfig>();

            if ((type == TileID.Saplings || type == TileID.GemSaplings) && config.FastTrees)
            {
                if (Main.rand.NextBool(4))
                {
                    GrowTree(i, j);
                }
            }
        }

        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            Tile tile = Main.tile[i, j];

            if (type == TileID.Saplings && !fail && tile.TileFrameY != 0 && ModContent.GetInstance<SkyblockModConfig>().SaplingsDropAcorns)
            {
                Item.NewItem(GetItemSource_FromTileBreak(i, j), new Vector2(i * 16, j * 16), Vector2.Zero, ItemID.Acorn);
            }

            if ((type == TileID.Crystals || type == TileID.Titanstone) && !Main.hardMode)
            {
                noItem = true;
            }
        }
    }
}
