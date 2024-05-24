using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateSkyblock.Content.CheesePrevention
{
    public class TileConversion : GlobalTile
    {
        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (HardmodeOres[type] && !Condition.Hardmode.IsMet() && UltimateSkyblock.IsSkyblock())
                fail = true;
        }

        public static bool[] HardmodeOres = TileID.Sets.Factory.CreateBoolSet(
            TileID.Cobalt,
            TileID.Mythril,
            TileID.Adamantite,
            TileID.Palladium,
            TileID.Orichalcum,
            TileID.Titanium);
    }
}
