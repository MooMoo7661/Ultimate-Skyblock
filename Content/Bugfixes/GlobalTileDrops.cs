using static Terraria.WorldGen;

namespace UltimateSkyblock.Content.Bugfixes
{
    public class GlobalTileDrops : GlobalTile
    {
        //Bandaid fix to some dumb fucking bug
        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            base.KillTile(i, j, type, ref fail, ref effectOnly, ref noItem);

            if (type == TileID.Heart)
            {
                if (Main.tile[i, j].TileFrameX == 0 && Main.tile[i, j].TileFrameY == 0)
                {
                    Item.NewItem(GetItemSource_FromTileBreak(i, j), new Vector2(i * 16, j * 16), Vector2.Zero, ItemID.LifeCrystal);
                }

                noItem = true;
            }
        }
    }
}
