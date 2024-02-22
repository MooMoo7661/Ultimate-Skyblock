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
using Microsoft.Xna.Framework;
using StructureHelper;
using Terraria.DataStructures;
using Terraria.Utilities;
using Terraria.ModLoader;

namespace UltimateSkyblock.Content.Subworlds.Passes
{
    public class DeepstoneBunkerPass : GenPass
    {
        private static UltimateSkyblock Modref = UltimateSkyblock.Instance;

        public static string prePath = "Content/Subworlds/SubStructures";
        public static string path = "Content/Subworlds/SubStructures/Deepstone";

        #region Enums
        public enum RoomID
        {
            Hallway1,
            Hallway2,
            Hallway3,
            Hallway4,
            Hallway5,
            Hallway6,
            Hallway7,
            Hallway8,
            Hallway9,
            Hallway10,
            Hallway11,
            Hallway12,
            Hallway13,
            Hallway14,
            Hallway15,
            Hallway16,
            Hallway17,
            Hallway18,
            Hallway19,
            Hallway20,
            Hallway21,
            LeftAlchemyRoom1,
            LeftAlchemyRoom2,
            LeftBlacksmithRoom1,
            LeftBlacksmithRoom2,
            LeftBlacksmithRoom3,
            LeftLibrary1,
            LeftLibrary2,
            LeftLibrary3,
            LeftLounge1,
            LeftLounge2,
            LeftSpiderStatueRoom,
            RightLibrary1,
            RightLibrary2,
            RightLibrary3,
            RightLounge1,
            RightLounge2,
            RightLoungeTrapped1,
            RightLoungeTrapped2,
            RightTreasureRoom1,
            RightTreasureRoom2,
            RightTreasureRoom3,
            RightTreasureRoomTrapped1,
            RightTreasureRoomTrapped2,
            RightTreasureRoomTrapped3,

            Basement1,
            BasementCatacombCenter,
            BasementLeftStairs,
            BasementRightStairs,
            BasementLeftRuinedDropdown1,
            BasementLeftRuinedDropdown2,
            BasementLeftFlooded1,
            BasementRightRuinedDropdown1,
            BasementRightRuinedDropdown2,
        }

        public static List<RoomID> Hallways = new List<RoomID>
        {
            RoomID.Hallway1,
            RoomID.Hallway2,
            RoomID.Hallway3,
            RoomID.Hallway4,
            RoomID.Hallway5,
            RoomID.Hallway6,
            RoomID.Hallway7,
            RoomID.Hallway8,
            RoomID.Hallway9,
            RoomID.Hallway10,
            RoomID.Hallway11,
            RoomID.Hallway12,
            RoomID.Hallway13,
            RoomID.Hallway14,
            RoomID.Hallway15,
            RoomID.Hallway16,
            RoomID.Hallway17,
            RoomID.Hallway18,
            RoomID.Hallway19,
            RoomID.Hallway20,
            RoomID.Hallway21,

        };

        public static List<RoomID> LeftRooms = new List<RoomID>
        { 
            RoomID.LeftAlchemyRoom1,
            RoomID.LeftAlchemyRoom2,
            RoomID.LeftBlacksmithRoom1,
            RoomID.LeftBlacksmithRoom2,
            RoomID.LeftBlacksmithRoom3,
            RoomID.LeftLibrary1,
            RoomID.LeftLibrary2,
            RoomID.LeftLibrary3,
            RoomID.LeftLounge1,
            RoomID.LeftLounge2,
            RoomID.LeftSpiderStatueRoom,
        };

        public static List<RoomID> RightRooms = new List<RoomID>
        { 
            RoomID.RightLibrary1,
            RoomID.RightLibrary2,
            RoomID.RightLibrary3,
            RoomID.RightLounge1,
            RoomID.RightLounge2,
            RoomID.RightLoungeTrapped1,
            RoomID.RightLoungeTrapped2,
            RoomID.RightTreasureRoom1,
            RoomID.RightTreasureRoom3,
            RoomID.RightTreasureRoomTrapped1,
            RoomID.RightTreasureRoomTrapped2,
            RoomID.RightTreasureRoomTrapped3,
        };

        public static List<RoomID> LeftBasementDropdownRooms = new List<RoomID>
        {
            RoomID.BasementLeftStairs,
            RoomID.BasementLeftRuinedDropdown1,
            RoomID.BasementLeftRuinedDropdown2,
            RoomID.BasementLeftFlooded1
        };

        public static List<RoomID> RightBasementDropdownRooms = new List<RoomID>
        {
            RoomID.BasementRightRuinedDropdown1,
            RoomID.BasementRightRuinedDropdown2,
        };

        #endregion

        public static Dictionary<int, string> RoomsDictionary = new Dictionary<int, string>();
            
        public DeepstoneBunkerPass(string name, double loadWeight) : base(name, loadWeight) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            foreach (RoomID room in Enum.GetValues(typeof(RoomID)))
            {
                RoomsDictionary.TryAdd((int)room, room.ToString());
                Modref.Logger.Info("Tried to add key " + room.ToString());
            }

            Point16 placePoint = new(Main.maxTilesX / 2, Main.UnderworldLayer - 150);

            Generator.GenerateStructure(path + "Tower", placePoint, Modref);
            GenerateBasement(placePoint.X - 12, placePoint.Y + 44);
            HallwayRunner(placePoint.X, placePoint.Y);

            // Fixing previous right room structures that I forgot to place an extra layer of walls to connect hallways.
            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = Main.UnderworldLayer - 200;  y < Main.maxTilesY; y++)
                {
                    Tile tile = Framing.GetTileSafely(x, y);
                    if (!tile.HasTile && tile.WallType == WallID.None)
                    {
                        if (GenUtils.TileLeft(x, y).WallType == WallID.AncientObsidianBrickWall && GenUtils.TileRight(x, y).WallType == WallID.AncientObsidianBrickWall)
                        {
                            WorldGen.PlaceWall(x, y, WallID.AncientObsidianBrickWall);
                            WorldGen.paintWall(x, y, PaintID.GrayPaint, true);
                        }
                    } 
                }
            }
            
        }

        public void HallwayRunner(int x, int y)
        {
            //using new UnifiedRandoms for actual randomness,
            //due to Main.rand functioning identical to WorldGen.genRand in subworlds.

            GenerateHallway(x, y, -1, new UnifiedRandom(Environment.TickCount).Next(3, 6));
            GenerateHallway(x, y, 1, new UnifiedRandom(Environment.TickCount).Next(3, 6));
            GenerateHallway(x, y + 11, -1, new UnifiedRandom(Environment.TickCount).Next(5, 10));
            GenerateHallway(x, y + 11, 1, new UnifiedRandom(Environment.TickCount).Next(5, 10));
            GenerateHallway(x, y + 22, -1, new UnifiedRandom(Environment.TickCount).Next(8, 13));
            GenerateHallway(x, y + 22, 1, new UnifiedRandom(Environment.TickCount).Next(8, 13));

            //43
            //-12
        }

        /// <summary>
        /// Creates a hallway with the given direction.
        /// Automatically rolls the hallways and the rooms at the end of the hallway.
        /// </summary>
        /// <param name="x">X position of the top left of the tower.</param>
        /// <param name="y">Y position of the top left of the tower. To generate hallways further down, simply move the given coordinates down a little.</param>
        /// <param name="dir">Direction. Will throw if not -1 or 1</param>
        /// <param name="maxRooms">Maximum number of rooms to generate.</param>
        /// <exception cref="ArgumentException"></exception>
        public void GenerateHallway(int x, int y, int dir, int maxRooms)
        {
            if (dir != -1 && dir != 1)
                throw new ArgumentException("Generate Hallway - parameter \"dir\" expected to be -1 or 1 only.");

            List<RoomID> _hallways = RollHallways(maxRooms);
            RoomID endRoom = RollUpperRoom(dir);

            int Xoffset = 14 * dir; // Inverts the X offset if we are generating to the left. This is needed for the hallway to also generate to the left.
            int startingOffset = (dir == -1) ? -12 : 16; // Top left of the tower is the expected X and Y, meaning there is some offset for hallways since it's not centered.

            Point16 genPos = new Point16(0, 0);

            for (int i = 1; i < _hallways.Count; i++)
            {
                startingOffset = Xoffset * i;

                // Getting a random hallway to generate, then removing it to prevent repetition.
                int index = new UnifiedRandom(Environment.TickCount).Next(_hallways.Count);
                RoomID currentHallway = _hallways[index];
                _hallways.RemoveAt(index);

                genPos = new Point16((x + 2) + startingOffset, y + 14);
                Modref.Logger.Info("Generated hallway - " + currentHallway.ToString() + " at " + genPos);
                Generator.GenerateStructure(path + RoomsDictionary[(int)currentHallway], genPos, Modref);
            }

            //Generating the room at the end of the hall.
            startingOffset += Xoffset;
            genPos = new Point16(x + 2 + startingOffset, y + 14);
            Modref.Logger.Info("Generated room - " + endRoom.ToString() + " at " + genPos);
            Generator.GenerateStructure(path + RoomsDictionary[(int)endRoom], genPos, Modref);

        }

        public static RoomID RollUpperRoom(int dir) => dir == -1 ? LeftRooms[new UnifiedRandom(Environment.TickCount).Next(0, LeftRooms.Count)] : RightRooms[new UnifiedRandom(Environment.TickCount).Next(0, RightRooms.Count)];
        public RoomID RollBasementDropdownRoom(int dir) => dir == -1 ? LeftBasementDropdownRooms[new UnifiedRandom(Environment.TickCount).Next(0, LeftBasementDropdownRooms.Count)] : RightBasementDropdownRooms[new UnifiedRandom(Environment.TickCount).Next(0, RightBasementDropdownRooms.Count)];

        public List<RoomID> RollHallways(int numRooms)
        {
            List<RoomID> locHallways = new List<RoomID>();
            foreach(RoomID room in Hallways)
            {
                locHallways.Add(room);
            }

            List<RoomID> output = new List<RoomID>();

            for (int i = 0; i < numRooms; i++)
            {
                int index = new UnifiedRandom(Environment.TickCount).Next(locHallways.Count);
                RoomID result = locHallways[index];
                locHallways.RemoveAt(index);
                output.Add(result);
            }

            return output;
        }

        public void GenerateBasement(int x, int y)
        {
            Generator.GenerateStructure(path + RoomID.Basement1.ToString(), new Point16(x, y), Modref);
            Generator.GenerateStructure(path + RoomsDictionary[(int)RollBasementDropdownRoom(-1)], new Point16(x - 13, y + 3), Modref);
            Generator.GenerateStructure(path + RoomsDictionary[(int)RollBasementDropdownRoom(1)], new Point16(x + 42, y + 3), Modref);
            Generator.GenerateStructure(path + RoomID.BasementCatacombCenter.ToString(), new Point16(x, y + 13), Modref);

        }
    }
}

