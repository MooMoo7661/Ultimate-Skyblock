using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Humanizer;

namespace UltimateSkyblock.Content.CheesePrevention
{
    public class ItemConversion : GlobalItem
    {
        public override void UpdateInventory(Item item, Player player)
        {
            if (!Condition.Hardmode.IsMet() && UltimateSkyblock.IsSkyblock())
            {

            }
        }
    }
}
