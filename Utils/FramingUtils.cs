using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace UltimateSkyblock.Utils
{
    public static class FramingUtils
    {
        public static void SetupTileMerge(this ModTile tile, int tile2)
        {
            Main.tileMerge[tile.Type][tile2] = true;
            Main.tileMerge[tile2][tile.Type] = true;
        }

        public static void SetupTileMerge(this ModTile tile, ModTile tile2)
        {
            Main.tileMerge[tile.Type][tile2.Type] = true;
            Main.tileMerge[tile2.Type][tile.Type] = true;
        }

        public static void SetupTileMerge(int tile1, int tile2)
        {
            Main.tileMerge[tile1][tile2] = true;
            Main.tileMerge[tile2][tile1] = true;
        }
    }
}
