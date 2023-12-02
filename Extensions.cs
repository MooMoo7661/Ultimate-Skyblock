using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace OneBlock
{
    public static class Extensions
    {
        public static int ToSeconds(this int seconds) => seconds * 60;
        public static float ToSeconds(this float seconds) => seconds * 60;

        public static Vector2 ToVector2(this Tile tile, int x, int y) => new Vector2(x, y);
    }
}
