using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltimateSkyblock.Content.Configs;
using static Terraria.WorldGen;

namespace UltimateSkyblock.Content.Bugfixes
{
    public class GlobalTileDrops : GlobalTile
    {

        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (type == TileID.Heart && Main.tile[i, j].TileFrameX == 0 && Main.tile[i, j].TileFrameY == 0 && ModContent.GetInstance<BugsConfig>().HeartCrystalDropFix)
            {
                noItem = true;
                Item.NewItem(GetItemSource_FromTileBreak(i, j), new Vector2(i * 16, j * 16), Vector2.Zero, ItemID.LifeCrystal);
            }
        }
    }
}
