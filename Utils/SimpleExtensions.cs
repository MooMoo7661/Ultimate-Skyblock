using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace UltimateSkyblock.Content.Utils
{
    public static class SimpleExtensions
    {
        public static int ToSeconds(this int seconds) => seconds * 60;
        public static float ToSeconds(this float seconds) => seconds * 60;
        public static Vector2 ToVector2(this Tile tile, int x, int y) => new Vector2(x, y);

        public static string ToHexString(this string text, Color color)
        {
            return "[c/" + color.Hex3() + ":" + text + "]";
        }

        /// <summary>
        /// Tries to add items to the chest. Will find the next open air slot.<para>Does not combine item stacks, only for simple uses.</para>
        /// </summary>
        public static void Add(this Chest chest, Item item)
        {
            for (int i = 0; i < chest.item.Length; i++)
            {
                if (chest.item[i].NullOrAir()) // Gets the closest index of a chest that's empty
                {
                    chest.item[i].SetDefaults(item.type);
                    chest.item[i].stack = item.stack;
                    return;
                }
            }
        }

        /// <summary>
        /// Tries to add items to the chest. Will find the next open air slot.<para>Does not combine item stacks, only for simple uses.</para>
        /// </summary>
        /// <returns>True, if the item was successfully added.<para>False, if the item could not be added.</para></returns>
        public static bool TryAdd(this Chest chest, Item item)
        {
            for (int i = 0; i < chest.item.Length; i++)
            {
                if (chest.item[i].NullOrAir()) // Gets the closest index of a chest that's empty
                {
                    chest.item[i].SetDefaults(item.type);
                    chest.item[i].stack = item.stack;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Tries to add items to the chest. Will find the next open air slot.<para>Does not combine item stacks, only for simple uses.</para>
        /// </summary>
        public static void Add(this Chest chest, List<Item> ItemsList)
        {
            foreach (Item item in ItemsList)
            {
                for (int i = 0; i < chest.item.Length; i++)
                {
                    if (chest.item[i].NullOrAir())
                    {
                        chest.item[i].SetDefaults(item.type);
                        chest.item[i].stack = item.stack;
                    }
                }
            }
        }

        public static bool Valid(this Tile tile) => tile.HasTile && tile.Slope == SlopeType.Solid;
    }
}
