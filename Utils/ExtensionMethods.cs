using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace UltimateSkyblock.Content.Utils
{
    public static class ExtensionMethods
    {
        public static void PercentFull(this IEnumerable<Item> inv, out float stackPercentFull, out float slotsPercentFull)
        {
            slotsPercentFull = 0;
            float total = 0f;
            foreach (Item item in inv.Reverse())
            {
                if (item.NullOrAir())
                    continue;

                total += item.stack / (float)item.maxStack;
                slotsPercentFull++;
            }

            int count = inv.Count();
            if (count <= 0)
            {
                slotsPercentFull = 1f;
                stackPercentFull = 1f;
                return;
            }

            slotsPercentFull /= count;
            stackPercentFull = total / count;
        }
        public static bool NullOrAir(this Item item) => item?.IsAir ?? true;
        public static bool Deposit(this Item[] inv, ref Item item, out int index)
        {
            if (item.NullOrAir())
            {
                index = inv.Length;
                return false;
            }

            if (item.favorited)
            {
                index = inv.Length;
                return false;
            }

            if (Restock(inv, ref item, out index))
                return true;

            index = 0;
            while (index < inv.Length && !inv[index].IsAir)
            {
                index++;
            }

            if (index == inv.Length)
                return false;

            inv[index] = item.Clone();
            if (item.stack == item.maxStack)
                inv.DoCoins(index);

            item.TurnToAir();

            return true;
        }
        public static bool Restock(Item[] inv, ref Item item, out int index)
        {
            for (int i = 0; i < inv.Length; i++)
            {
                Item bagItem = inv[i];
                if (!bagItem.NullOrAir() && bagItem.type == item.type && bagItem.stack < bagItem.maxStack)
                {
                    if (ItemLoader.TryStackItems(bagItem, item, out _))
                    {
                        if (item.stack < 1)
                        {
                            item.TurnToAir();
                            index = i;
                            if (bagItem.stack == bagItem.maxStack)
                                inv.DoCoins(i);

                            return true;
                        }
                        else
                        {
                            inv.DoCoins(i);
                        }
                    }
                }
            }

            index = inv.Length;
            return false;
        }

        public static void DoCoins(this Item[] inv, int slot)
        {
            Item item = inv[slot];
            if (item.type < ItemID.CopperCoin || item.type > ItemID.GoldCoin)
                return;

            if (item.stack != 100)
                return;

            item.SetDefaults(item.type + 1);
            for (int i = 0; i < inv.Length; i++)
            {
                Item coin = inv[i];
                if (item.IsTheSameAs(coin) && i != slot && coin.stack < coin.maxStack)
                {
                    coin.stack++;
                    item.TurnToAir(true);
                    item.active = false;
                    inv.DoCoins(i);

                    break;
                }
            }
        }
        private static bool IsTheSameAs(this Item item, Item compareItem)
        {
            if (item.netID == compareItem.netID)
                return item.type == compareItem.type;

            return false;
        }

    }
}
