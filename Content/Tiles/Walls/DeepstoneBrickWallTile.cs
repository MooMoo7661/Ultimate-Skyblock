﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateSkyblock.Content.Tiles.Walls
{
    public class DeepstoneBrickWallTile : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            TileID.Sets.WallsMergeWith[Type] = true;
            Main.wallLargeFrames[Type] = 1;
            AddMapEntry(new Color(38, 38, 38));
        }
    }
}
