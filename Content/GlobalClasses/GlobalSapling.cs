using UltimateSkyblock.Content.Configs;
using static Terraria.WorldGen;

namespace UltimateSkyblock.Content.GlobalClasses
{
    public class GlobalSapling : GlobalTile
    {
        public override void Drop(int i, int j, int type)
        {
           if (type == TileID.Saplings && UltimateSkyblock.IsSkyblock())
                Item.NewItem(GetItemSource_FromTileBreak(i, j), new Vector2(i * 16, j * 16), Vector2.Zero, ItemID.Acorn);
        }

        public override void RandomUpdate(int i, int j, int type)
        {
            var config = ModContent.GetInstance<SkyblockModConfig>();

            if ((type == TileID.Saplings || type == TileID.GemSaplings) && config.FastTrees && UltimateSkyblock.IsSkyblock())
            {
                if (Main.rand.NextBool(4))
                {
                    GrowTree(i, j);
                }
            }
        }

        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if ((type == TileID.Crystals || type == TileID.Titanstone || type == TileID.MythrilBrick || type == TileID.CobaltBrick) && !Main.hardMode && UltimateSkyblock.IsSkyblock())
            {
                noItem = true;
            }
        }
    }
}
