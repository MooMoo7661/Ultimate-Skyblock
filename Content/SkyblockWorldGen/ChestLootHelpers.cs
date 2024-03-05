using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UltimateSkyblock.Content.SkyblockWorldGen.MainWorld;
using Terraria;
using Terraria.ID;
using UltimateSkyblock.Content.Utils;

namespace UltimateSkyblock.Content.SkyblockWorldGen
{
    public class ChestLootHelpers
    {
        /// <summary>
        /// Used for getting items with specific rules. Contains PoolItem, which is just an item but has a chance contained in it.
        /// </summary>
        public class Rule
        {
            public Item GetItem(Item input, float chance = 100)
            {
                if (Main.rand.Next(101) < chance)
                {
                    return input;
                }

                return new(ItemID.None);
            }

            public Item GetItem(int type, float chance = 100, int stack = 1)
            {
                int roll = (Main.rand.Next(101));
                UltimateSkyblock.Instance.Logger.Info("Rolled " + roll + ", chance was " + chance);
                if (roll < chance)
                {
                    return new Item(type, stack);
                }

                return new(ItemID.None);
            }

            public Item GetItem(List<Item> input, float chance = 100)
            {
                if (Main.rand.Next(101) > chance)
                {
                    return new(ItemID.None);
                }

                int index = Main.rand.Next(input.Count);
                return input[index];
            }
        }
    }
}
