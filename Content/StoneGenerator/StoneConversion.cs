using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using UltimateSkyblock.Content.Configs;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace UltimateSkyblock.Content.StoneGenerator
{
    public static class OB_Liquid
    {
        public static void Load()
        {
            IL_Liquid.Update += IL_Liquid_Update;
        }
        private static void IL_Liquid_Update(ILContext il)
        {
            var c = new ILCursor(il);

            if (!c.TryGotoNext(MoveType.Before,
                i => i.MatchLdcI4(1),
                i => i.MatchStloc(9),
                i => i.MatchLdcI4(1),
                i => i.MatchStloc(10),
                i => i.MatchLdcI4(1),
                i => i.MatchStloc(11),
                i => i.MatchLdcI4(1),
                i => i.MatchStloc(12)
                ))
            {
                throw new Exception("Failed to find instructions IL_Liquid_Update 1/2");
            }

            c.EmitLdarg(0);
            c.EmitLdfld(typeof(Liquid).GetField("x"));
            c.EmitLdarg(0);
            c.EmitLdfld(typeof(Liquid).GetField("y"));

            c.EmitDelegate(CheckMoveLiquids);

            var label = c.DefineLabel();
            c.Emit(OpCodes.Br, label);

            if (!c.TryGotoNext(MoveType.Before,
                i => i.MatchLdloca(4),
                i => i.MatchCall(out _),
                i => i.MatchLdindU1(),
                i => i.MatchLdloc(5),
                i => i.MatchBeq(out _)
                ))
            {
                throw new Exception("Failed to find instructions IL_Liquid_Update 2/2");
            }

            c.MarkLabel(label);
        }
        /// <summary>
        /// Part of changed Vanilla liquid movement mechanics
        /// </summary>
        private static bool MovePrevented(Tile tile) => tile.HasUnactuatedTile && Main.tileSolid[tile.TileType] && !Main.tileSolidTop[tile.TileType];
        /// <summary>
        /// Part of changed Vanilla liquid movement mechanics
        /// </summary>
        private static bool WillMerge(Tile tile) => tile.LiquidAmount > 0 && tile.LiquidType != tile.LiquidType;
        /// <summary>
        /// Replaces the vanilla liquid movement mechanics.  Probably shouldn't change anything in here besides adding hook like methods like I did with CanMove().
        /// </summary>
        private static void CheckMoveLiquids(int x, int y)
        {
            Tile tile = Main.tile[x, y];
            if (tile.LiquidAmount > 0)
            {
                float num = 0f;
                int numLeft = 0;
                int numRight = 0;
                int liquidAmount = 0;
                for (int i = 1; i <= 3; i++)
                {
                    int leftX = x - i;
                    Tile tileL = Main.tile[leftX, y];
                    bool leftCanMove = true;
                    if (MovePrevented(tileL) || WillMerge(tileL))
                    {
                        leftCanMove = false;
                    }
                    else if (i > 1 && tileL.LiquidAmount == 0)
                    {
                        leftCanMove = false;
                    }
                    else if (i == 2 && tile.LiquidAmount > 250)
                    {
                        leftCanMove = false;
                    }

                    if (CanMove(x, y, leftX, y, ref leftCanMove) && leftCanMove)
                    {
                        numLeft++;
                        liquidAmount += tileL.LiquidAmount;
                    }

                    int rightX = x + i;
                    Tile tileR = Main.tile[rightX, y];
                    bool rightCanMove = true;
                    if (MovePrevented(tileR) || WillMerge(tileR))
                    {
                        rightCanMove = false;
                    }
                    else if (i > 1 && tileR.LiquidAmount == 0)
                    {
                        rightCanMove = false;
                    }
                    else if (i == 2 && tile.LiquidAmount > 250)
                    {
                        rightCanMove = false;
                    }

                    if (CanMove(x, y, rightX, y, ref rightCanMove) && rightCanMove)
                    {
                        numRight++;
                        liquidAmount += tileR.LiquidAmount;
                    }

                    if (!leftCanMove || !rightCanMove)
                        break;
                }

                num += tile.LiquidAmount + liquidAmount;
                if (tile.LiquidAmount < 3)
                    num--;

                byte newAmount = (byte)Math.Round(num / (1 + numLeft + numRight));
                if (newAmount == byte.MaxValue - 1 && WorldGen.genRand.Next(30) == 0)
                    newAmount = byte.MaxValue;

                bool anyUpdated = false;
                int higherNum = Math.Max(numLeft, numRight);
                for (int i = 1; i <= higherNum; i++)
                {
                    if (i <= numLeft)
                    {
                        int tileX = x - i;
                        Tile tileL = Main.tile[tileX, y];
                        tileL.LiquidType = tile.LiquidType;
                        if (tileL.LiquidAmount != newAmount || tile.LiquidAmount != newAmount)
                        {
                            tileL.LiquidAmount = newAmount;
                            Liquid.AddWater(tileX, y);
                            anyUpdated = true;
                        }
                    }

                    if (i <= numRight)
                    {
                        int tileX = x + i;
                        Tile tileLR = Main.tile[tileX, y];
                        tileLR.LiquidType = tile.LiquidType;
                        if (tileLR.LiquidAmount != newAmount || tile.LiquidAmount != newAmount)
                        {
                            tileLR.LiquidAmount = newAmount;
                            Liquid.AddWater(tileX, y);
                            anyUpdated = true;
                        }
                    }
                }

                if (anyUpdated || numLeft < 2 && numRight < 2 || Main.tile[x, y - 1].LiquidAmount <= 0)
                    tile.LiquidAmount = newAmount;
            }
        }

        private static readonly int combineDelay = 30;
        /// <param name="canMoveVanilla">Passed in to allow forcing a liquid to move even if vanilla wouldn't allow it.</param>
        private static bool CanMove(int x, int y, int xMove, int yMove, ref bool canMoveVanilla)
        {
            if (!ModContent.GetInstance<SkyblockModConfig>().StoneGenerator)
                return canMoveVanilla;

            Tile tile = Main.tile[x, y];
            int oppositeX = xMove - (x - xMove);
            if (oppositeX < 0 || oppositeX >= Main.maxTilesX)
                return canMoveVanilla;

            Tile moveTile = Main.tile[xMove, yMove];
            Tile opposite = Main.tile[oppositeX, yMove];
            //Make something happen when a liquid touches a tile like turn sand into sandstone.
            if (moveTile.HasTile)
            {
                //switch (moveTile.TileType) {
                //	case TileID.Sand:
                //		if (tile.LiquidType == LiquidID.Lava || opposite.LiquidType > 0 && opposite.LiquidType == LiquidID.Lava)
                //			TryUpdateCombineInfo(xMove, yMove, convertDelay);

                //		return false;
                //}

                //return canMoveVanilla;
            }

            if (opposite.LiquidAmount > 0 && tile.LiquidType != opposite.LiquidType)
            {
                if (tile.LiquidType == LiquidID.Water && opposite.LiquidType == LiquidID.Lava || tile.LiquidType == LiquidID.Lava && opposite.LiquidType == LiquidID.Water)
                {
                    TryUpdateCombineInfo(xMove, yMove, combineDelay);

                    return false;
                }
            }

            return canMoveVanilla;
        }
        private static void TryUpdateCombineInfo(int x, int y, int delay)
        {
            Point combinePoint = new(x, y);
            int index = combinePoints.IndexOf(combinePoint);
            if (index == -1)
            {
                combinePoints.Add(combinePoint);
                combineInfos.Add(new CombineInfo(combineDelay, () =>
                {
                    CheckCombineLiquids(x, y);
                }));
            }
            else
            {
                CombineInfo combineInfo = combineInfos[index];
                if (combineInfo.InitialDelay > delay)
                {
                    combineInfo.Delay -= combineInfo.InitialDelay - delay;
                    combineInfo.InitialDelay = delay;
                }
            }
        }
        private static void CheckCombineLiquids(int x, int y)
        {
            Tile[] tiles = DirectionID.GetTiles(x, y);
            if (tiles[DirectionID.None].HasTile)
            {
                //for (int i = DirectionID.None; i < DirectionID.Count; i++) {
                //	if (tiles[i].Lava()) {
                //		PlaceBlockFromLiquidMerge(x, y, TileID.Sandstone, LiquidID.Lava, LiquidID.Water);
                //		break;
                //	}
                //}

                return;
            }

            Tile left = tiles[DirectionID.Left];
            if (left.LiquidAmount > 0)
            {
                Tile right = tiles[DirectionID.Right];
                bool water = left.LiquidType == LiquidID.Water;
                if (water || left.LiquidType == LiquidID.Lava)
                {
                    if (right.LiquidAmount > 0)
                    {
                        int requiredType = water ? LiquidID.Lava : LiquidID.Water;
                        if (right.LiquidType == requiredType)
                        {
                            int tileType = y <= Main.rockLayer ? TileID.Stone : TileID.Obsidian;
                            PlaceBlockFromLiquidMerge(x, y, tileType, left.LiquidType, right.LiquidType);
                        }
                    }
                }
            }
        }
        private static bool Lava(this Tile tile) => tile.LiquidAmount > 0 && tile.LiquidType == LiquidID.Lava;
        private static void PlaceBlockFromLiquidMerge(int x, int y, int tileType, int liquidType, int otherLiquidType)
        {
            WorldGen.PlaceTile(x, y, tileType, true, true);
            TileChangeType liquidChangeType = WorldGen.GetLiquidChangeType(liquidType, otherLiquidType);
            WorldGen.PlayLiquidChangeSound(liquidChangeType, x, y);
            WorldGen.SquareTileFrame(x, y);
            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendTileSquare(-1, x - 1, y - 1, 3, liquidChangeType);
        }
        private static List<Point> combinePoints = new();
        private static List<CombineInfo> combineInfos = new();
        public static void Update()
        {
            for (int i = combineInfos.Count - 1; i >= 0; i--)
            {
                CombineInfo combineInfo = combineInfos[i];
                ref int delay = ref combineInfo.Delay;
                delay--;
                if (delay <= 0)
                {
                    combineInfo.Action();
                    combinePoints.RemoveAt(i);
                    combineInfos.RemoveAt(i);
                }
            }
        }
    }
    public class CombineInfo
    {
        public int InitialDelay;
        public int Delay;
        public Action Action;
        public CombineInfo(int delay, Action action)
        {
            InitialDelay = delay;
            Delay = delay;
            Action = action;
        }
    }
    public static class DirectionID
    {
        public const int None = 0;
        public const int Left = 1;
        public const int Right = 2;
        public const int Up = 3;
        public const int Down = 4;
        public const int Count = 5;

        public static void ApplyDirection(ref int x, ref int y, int direction)
        {
            switch (direction)
            {
                case Up:
                    y--;
                    break;
                case Down:
                    y++;
                    break;
                case Left:
                    x--;
                    break;
                case Right:
                    x++;
                    break;
            }
        }

        public static int GetOppositeDirection(int direction)
        {
            switch (direction)
            {
                case Up:
                    return Down;
                case Down:
                    return Up;
                case Left:
                    return Right;
                case Right:
                    return Left;
                default:
                    return None;
            }
        }

        public static (int, int)[] GetDirections(int x, int y)
        {
            (int, int)[] directions = new (int, int)[5];
            directions[None] = (x, y);
            directions[Left] = (x - 1, y);
            directions[Right] = (x + 1, y);
            directions[Up] = (x, y - 1);
            directions[Down] = (x, y + 1);

            return directions;
        }
        /// <summary>
        /// Not Safe.  Need to check if out of world before using.
        /// </summary>
        public static Tile[] GetTiles(int x, int y)
        {
            Tile[] tiles = new Tile[5];
            tiles[None] = Main.tile[x, y];
            tiles[Left] = Main.tile[x - 1, y];
            tiles[Right] = Main.tile[x + 1, y];
            tiles[Up] = Main.tile[x, y - 1];
            tiles[Down] = Main.tile[x, y + 1];

            return tiles;
        }
    }
}