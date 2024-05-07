using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.IO;
using Terraria.WorldBuilding;
using UltimateSkyblock.Content.Items.Placeable;
using static UltimateSkyblock.Content.Subworlds.MiningSubworld;
using StructureHelper;

namespace UltimateSkyblock.Content.Subworlds.DungeonPasses
{
    public class PaintingMaker : GenPass
    {
        public PaintingMaker(string name, double loadWeight) : base(name, loadWeight) { }

        public static List<int> Paintings3X3Subtypes = new List<int>
        {
            19, // The Cursed Man
            18, // Skellington J Skellingsworth
            13, // The Hanged Man
            14, // Glory of the Fire
            16, // Wall Skeleton
            17, // Hanging Skeleton
            15, // Bone Warp
            71, // Reborn
        };

        public static List<int> Paintings6X4Subtypes = new List<int>
        {
           16, // The Creation of the Guide
           8, // The Destroyer
           5, // Dryadisque
           0, // The Eye Sees the End
           13, // Facing the Cerebral Mastermind
           4, // Goblins Playing Poker
           11, // Great Wave
           6, // Impact
           9, // The Persistency of Eyes
           7, // Powered by Birds
           3, // The Screamer
           30, // Sparky
           1, // Something Evil is Watching You
           12, // Starry Night
           15, // Trio Super Heroes
           2, // The Twins Have Awoken
           10, // Unicorn Crossing the Hallows
        };

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = 0; y < Main.maxTilesY; y++)
                {
                    Tile tile = Main.tile[x, y];
                    if (WorldGen.genRand.NextBool(180) && !GenUtils.AreaContainsSensitiveTiles(new List<int> { TileID.Painting2X3, TileID.Painting3X2, TileID.Painting3X3, TileID.Painting4X3, TileID.Painting6X4 }, x, y, 20, 20) && !tile.HasTile && tile.WallType != WallID.None && tile.WallType != WallID.BlueDungeon)
                    {
                        if (WorldGen.genRand.NextBool(3))
                            Place3X3Painting(x, y);
                        else
                            Place6X4Painting(x, y);
                    }

                    progress.Set((y + x * Main.maxTilesY) / (float)(Main.maxTilesX * Main.maxTilesY));
                }
            }
        }

        public static void Place3X3Painting(int x, int y)
        {
            bool can = true;
            for (int i = x - 2; i < x + 3; i++)
            {
                for (int j = y - 2; j < y + 3; j++)
                {
                    Tile tile = Framing.GetTileSafely(i, j);
                    if (tile.HasTile && !Main.tileSolidTop[tile.TileType])
                    {
                        can = false;
                    }

                    //if (WorldGen.InWorld(i, j) && Framing.GetTileSafely(i, j).TileType != TileID.Adamantite)
                    //{
                    //    Tile tile2 = Main.tile[i, j];
                    //    tile2.WallType = WallID.Dirt;
                    //}
                }
            }

            if (can && WorldGen.InWorld(x, y))
            {
                WorldGen.PlaceObject(x, y, TileID.Painting3X3, false, Paintings3X3Subtypes[WorldGen.genRand.Next(Paintings3X3Subtypes.Count)]);
            }
        }

        public static void Place6X4Painting(int x, int y)
        {
            bool can = true;
            for (int i = x - 3; i < x + 5; i++)
            {
                for (int j = y - 3; j < y + 3; j++)
                {
                    Tile tile = Framing.GetTileSafely(i, j);
                    if (tile.HasTile && !Main.tileSolidTop[tile.TileType] && Main.tileSolid[tile.TileType])
                    {
                        can = false;
                    }

                    //if (WorldGen.InWorld(i, j) && Framing.GetTileSafely(i, j).TileType != TileID.Adamantite)
                    //{
                    //    Tile tile2 = Main.tile[i, j];
                    //    tile2.WallType = WallID.Dirt;
                    //}
                }
            }

            if (can && WorldGen.InWorld(x, y))
            {
                WorldGen.PlaceObject(x, y, TileID.Painting6X4, false, Paintings6X4Subtypes[WorldGen.genRand.Next(Paintings6X4Subtypes.Count)]);
            }
        }
    }
}
