using Microsoft.Xna.Framework;
using Terraria;

namespace OneBlock.Utils
{
    public static class SimpleExtensions
    {
        public static int ToSeconds(this int seconds) => seconds * 60;
        public static float ToSeconds(this float seconds) => seconds * 60;
        public static Vector2 ToVector2(this Tile tile, int x, int y) => new Vector2(x, y);
        public static void Add(this Chest chest, Item item)
        {
            for (int i = 0; i < chest.item.Length; i++)
            {
                if (chest.item[i].IsAir)
                {
                    chest.item[i] = item;
                    return;
                }
            }
        }
    }
}
