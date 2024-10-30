using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateSkyblock.Content.CheesePrevention
{
    public class TileConversion : GlobalTile
    {
        public static bool[] HardmodeOres = TileID.Sets.Factory.CreateBoolSet(
            TileID.Cobalt,
            TileID.Mythril,
            TileID.Adamantite,
            TileID.Palladium,
            TileID.Orichalcum,
            TileID.Titanium);

        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (!UltimateSkyblock.IsSkyblock())
                return;

            if (HardmodeOres[type] && !Condition.Hardmode.IsMet())
                fail = true;

            if (type == TileID.Meteorite && !NPC.downedBoss2)
                fail = true;
        }


    }
}
