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

namespace UltimateSkyblock.Content.Subworlds.Passes
{
    public class DeepstoneBunkerPass : GenPass
    {
        private static UltimateSkyblock Modref = UltimateSkyblock.Instance;

        public static string prePath = "Content/Subworlds/SubStructures";
        public static string path = "Content/Subworlds/SubStructures/Deepstone";

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
        }

        public static List<RoomID> hallways = new List<RoomID>
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

        public static List<RoomID> leftRooms = new List<RoomID>
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

        public static List<RoomID> rightRooms = new List<RoomID>
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
            HallwayRunner(placePoint.X, placePoint.Y);
            
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
        }

        public void GenerateHallway(int x, int y, int dir, int maxRooms)
        {
            if (dir != -1 && dir != 1)
                throw new Exception("Generate Hallway - parameter \"dir\" expected to be -1 or 1 only.");

            List<RoomID> _hallways = RollHallways(maxRooms);
            RoomID endRoom = RollRoom(dir);
            int Xoffset = 14 * dir;
            int currentXOffset = (dir == -1) ? -12 : 16;
            RoomID currentHallway;
                
            for (int i = 1; i < _hallways.Count; i++)
            {
                currentXOffset = Xoffset * i;
                int index = new UnifiedRandom(Environment.TickCount).Next(_hallways.Count);
                currentHallway = _hallways[index];
                _hallways.RemoveAt(index);

                Point16 pos = new Point16((x + 2) + currentXOffset, y + 14);
                Generator.GenerateStructure(path + RoomsDictionary[(int)currentHallway], pos, Modref);
            }

            currentXOffset += Xoffset;
            Point16 posEnd = new Point16(x + 2 + currentXOffset, y + 14);
            Generator.GenerateStructure(path + RoomsDictionary[(int)endRoom], posEnd, Modref);

        }

        public static RoomID RollRoom(int dir)
        {
            return dir == -1 ? leftRooms[new UnifiedRandom(Environment.TickCount).Next(0, leftRooms.Count)] : rightRooms[new UnifiedRandom(Environment.TickCount).Next(0, rightRooms.Count)];
        }

        public List<RoomID> RollHallways(int numRooms)
        {
            List<RoomID> locHallways = new List<RoomID>();
            foreach(RoomID room in hallways)
            {
                locHallways.Add(room);
            }

            List<RoomID> output = new List<RoomID>();

            for (int i = 0; i < numRooms; i++)
            {
                int index = new UnifiedRandom(Environment.TickCount).Next(locHallways.Count);
                Modref.Logger.Info("Index: " + index);
                Modref.Logger.Info("Rooms: " + numRooms);
                Modref.Logger.Info("Count: " + locHallways.Count + "\n");
                RoomID result = locHallways[index];
                locHallways.RemoveAt(index);
                output.Add(result);
            }

            return output;
        }

    }
}
