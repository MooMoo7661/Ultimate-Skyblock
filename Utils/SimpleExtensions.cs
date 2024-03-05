using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
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

        public static void AddItemList(this List<Item> list, List<Item> input)
        {
            foreach(Item obj in input)
                list.Add(obj);
        }

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
    }
}
