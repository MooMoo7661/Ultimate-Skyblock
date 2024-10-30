using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureHelper;
using Terraria.ModLoader.IO;
using Terraria.ModLoader;
using UltimateSkyblock.Content.Subworlds.DungeonRoomUtils;
using System.Reflection;
using Terraria.GameContent.Events;
using System.Diagnostics;
using Humanizer;

namespace UltimateSkyblock.Content.Subworlds.DungeonPasses
{
    public class DungeonGenerationPass : GenPass
    {
        public DungeonGenerationPass(string name, double loadWeight) : base(name, loadWeight) { }

        public static int previousRoom = -1;
        public static string prePath = "Content/Subworlds/DungeonStructures/";
        public static int roomIndex = 0;

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            GenLogger.ColorLog("Starting Generation", ConsoleColor.Red);
            Stopwatch stopwatch = Stopwatch.StartNew();

            Point16 dims = Point16.Zero;
            Generator.GetDimensions(prePath + "DungeonEntrance", UltimateSkyblock.Instance, ref dims);
            Point16 MainPoint = new Point16(Main.maxTilesX / 2 - dims.X / 2, Main.maxTilesY - Main.maxTilesY / 3);

            Generator.GenerateMultistructureRandom(prePath + "DungeonEntrance", MainPoint, UltimateSkyblock.Instance);
            HallwayRunner(new Point16(MainPoint.X, MainPoint.Y));

            GenLogger.ColorLog("Getting Dungeon Entrance dimensions", ConsoleColor.Gray);
            Generator.GetMultistructureDimensions(prePath + "DungeonEntrance", UltimateSkyblock.Instance, 0, ref dims);
            Point spawn = new(MainPoint.X + dims.X / 2, MainPoint.Y + dims.Y - 6);
            Main.spawnTileX = spawn.X;
            Main.spawnTileY = spawn.Y;
            
            MakeDungeonUnsafe();

            stopwatch.Stop();
            UltimateSkyblock.Instance.Logger.Info("Finished Dungeon Generation in " + stopwatch.Elapsed.TotalSeconds + " seconds.");
        }

        public void HallwayRunner(Point16 genPoint)
        {
            GenerateBottomHallway(genPoint.X, genPoint.Y + 34, -1, 8);
            GenerateBottomHallway(genPoint.X + 67, genPoint.Y + 34, 1, 8);
        }

        public void GenerateBottomHallway(int x, int y, int dir, int maxRooms)
        {
            GenLogger.ColorLog("Starting to generate bottom hallway with params: " + x + " " + y + " " + dir + " " + maxRooms, ConsoleColor.Green);
            int offset = 0;
            previousRoom = -1;
            roomIndex = 0;
            int towerIndex = new Random().Next(maxRooms / 2, maxRooms);

            for (int i = 0; i <= maxRooms; i++)
            {
                if (roomIndex == towerIndex)
                {
                    GenLogger.ColorLog("Starting tower generation");
                    Point16 towerDims = Point16.Zero;

                    TagCompound towerTag = DynamicStructureSystem.GetTag(prePath + "Tower", UltimateSkyblock.Instance);
                    List<TagCompound> list2 = (List<TagCompound>)towerTag.GetList<TagCompound>("Structures");
                    int index3 = new Random().Next(list2.Count);

                    Generator.GetMultistructureDimensions(prePath + "Tower", UltimateSkyblock.Instance, index3, ref towerDims);
                    Point genPos = new(x + offset, y - towerDims.Y);
                    //When generating to the left, the offset needs to be subtracted before generation.
                    //Generating to the right does not require this, as we can simply just generate it THEN add the offset for the next room.
                    if (dir == -1)
                        genPos.X -= towerDims.X;

                    Generator.GenerateMultistructureSpecific(prePath + "Tower", new Point16(genPos.X, genPos.Y), UltimateSkyblock.Instance, index3);
                    GenLogger.ColorLog("Generated tower");
                    offset += towerDims.X * dir;

                    roomIndex++;
                    maxRooms++;

                    if (dir == -1)
                    {
                        GenLogger.ColorLog("Generating top hallways");
                        GenerateTopHallway(genPos.X, genPos.Y + 36, -1, new Random().Next(4, 8), 1, true); // left
                        GenerateTopHallway(genPos.X + 44, genPos.Y + 36, 1, new Random().Next(1, 3), 1, false); // right
                        GenLogger.ColorLog("Finished top hallways");
                    }
                    else
                    {
                        GenLogger.ColorLog("Generating top hallways");
                        GenerateTopHallway(genPos.X, genPos.Y + 36, -1, new Random().Next(1, 3), 1, false); // left
                        GenerateTopHallway(genPos.X + 44, genPos.Y + 36, 1, new Random().Next(4, 8), 1, true); // right
                        GenLogger.ColorLog("Finished top hallways");
                    }

                    continue;
                }

                int index = RollRoom(i);
                Point16 dims = Point16.Zero;
                GenLogger.ColorLog("Getting dungeon rooms dimensions, with index " + index);
                Generator.GetMultistructureDimensions(prePath + "DungeonRooms", UltimateSkyblock.Instance, index, ref dims);
                GenLogger.ColorLog("Dimensions: " + dims);

                //When generating to the right, only offset AFTER generation.
                if (dir == 1)
                {
                    GenLogger.ColorLog("Attempting to generate DungeonRooms with index " + index + " at coordinates " + new Point16(x + offset, y - dims.Y));
                    Generator.GenerateMultistructureSpecific(prePath + "DungeonRooms", new Point16(x + offset, y - dims.Y), UltimateSkyblock.Instance, index);
                    GenLogger.ColorLog("Generated room");
                    offset += dims.X;
                }
                else
                {
                    offset += dims.X * dir;
                    GenLogger.ColorLog("Attempting to generate DungeonRooms with index " + index + " at coordinates " + new Point16(x + offset, y - dims.Y));
                    Generator.GenerateMultistructureSpecific(prePath + "DungeonRooms", new Point16(x + offset, y - dims.Y), UltimateSkyblock.Instance, index);
                }
                roomIndex++;
            }

            GenLogger.ColorLog("Trying to get basement index");
            TagCompound tag =DynamicStructureSystem. GetTag(prePath + "Basement", UltimateSkyblock.Instance);
            List<TagCompound> list = (List<TagCompound>)tag.GetList<TagCompound>("Structures");
            int index2 = new Random().Next(list.Count);
            GenLogger.ColorLog("Rolled index " + index2 + " for generating basement stairs");

            GenLogger.ColorLog("Attempting to generate basement stairs at position " + new Point16(x + offset, y - 27));
            Point16 basementDims = Point16.Zero;
            GenLogger.ColorLog("Trying to get dimensions for Basement at index " + index2);
            Generator.GetMultistructureDimensions(prePath + "Basement", UltimateSkyblock.Instance, index2, ref basementDims);
            if (dir == -1)
                offset -= basementDims.X;
            Generator.GenerateMultistructureSpecific(prePath + "Basement", new Point16(x + offset, y  - 27), UltimateSkyblock.Instance, index2);
            GenLogger.ColorLog("Generated basement stairs", ConsoleColor.Green);
            if (dir == -1)
            GenerateBasementHallway(x + offset, y + basementDims.Y - 27, -1, new Random().Next(2, 5), 0, new Random().Next(3, 5));
            if (dir == 1)
            GenerateBasementHallway(x + offset + basementDims.X, y + basementDims.Y - 27, 1, new Random().Next(2, 5), 0, new Random().Next(3, 5));
            GenLogger.ColorLog("Generated basement hallways", ConsoleColor.Green);
        }

        public void GenerateBasementHallway(int x, int y, int dir, int maxRooms, int floor, int floorToTerminateAt = 3)
        {
            int offset = 0;
            roomIndex = 0;
            Point16 dims = Point16.Zero;
            for (int i = 0; i <= maxRooms; i++)
            {
                int index = RollRoom(i, roomType: "BasementRooms");

                Generator.GetMultistructureDimensions(prePath + "BasementRooms", UltimateSkyblock.Instance, index, ref dims);

                if (dir == 1)
                {
                    Generator.GenerateMultistructureSpecific(prePath + "BasementRooms", new Point16(x + offset, y - dims.Y), UltimateSkyblock.Instance, index);
                    offset += dims.X * dir;
                }
                else
                {
                    offset += dims.X * dir;
                    Generator.GenerateMultistructureSpecific(prePath + "BasementRooms", new Point16(x + offset, y - dims.Y), UltimateSkyblock.Instance, index);
                }
                roomIndex++;
            }

            if (floor >= floorToTerminateAt || (floor >= 2 && dir == -1))
            {
                return;
            }

            TagCompound tag = DynamicStructureSystem.GetTag(prePath + "Basement", UltimateSkyblock.Instance);
            List<TagCompound> list = (List<TagCompound>)tag.GetList<TagCompound>("Structures");
            int index2 = new Random().Next(list.Count);

            Point16 basementDims = Point16.Zero;
            Generator.GetMultistructureDimensions(prePath + "Basement", UltimateSkyblock.Instance, index2, ref basementDims);

            if (dir == -1)
                offset -= basementDims.X;

            Generator.GenerateMultistructureSpecific(prePath + "Basement", new Point16(x + offset, y - 27), UltimateSkyblock.Instance, index2);

            if (dir == 1)
                offset += basementDims.X;
            
            floor += 1;
            GenerateBasementHallway(x + offset, y + basementDims.Y - 27, dir, new Random().Next(3, 5) - floor, floor);
        }

        ///TODO: Accurate room tracking for other hallways than bottom. Currently it's messed up a little.

        public void GenerateTopHallway(int x, int y, int dir, int maxRooms, int floor, bool tower = true, int floorToTerminateAt = 3)
        {
            int offset = 0;
            previousRoom = -1;
            int towerIndex = new Random().Next(maxRooms / 2, Math.Clamp(maxRooms - floor, maxRooms / 2 + 1, maxRooms));
            int roomIndex = 0;

            for (int i = 0; i < maxRooms; i++)
            {
                if (tower && roomIndex == towerIndex && floor != floorToTerminateAt)
                {
                    Point16 towerDims = Point16.Zero;
                    TagCompound towerTag = DynamicStructureSystem.GetTag(prePath + "Tower", UltimateSkyblock.Instance);
                    List<TagCompound> list2 = (List<TagCompound>)towerTag.GetList<TagCompound>("Structures");
                    int index3 = new Random().Next(list2.Count);
                    Generator.GetMultistructureDimensions(prePath + "Tower", UltimateSkyblock.Instance, index3, ref towerDims);
                    Point genPos = new(x + offset, y - towerDims.Y);

                    //When generating to the left, the offset needs to be subtracted before generation.
                    //Generating to the right does not require this, as we can simply just generate it THEN add the offset for the next room.
                    if (dir == -1)
                        genPos.X -= towerDims.X;

                    Generator.GenerateMultistructureSpecific(prePath + "Tower", genPos.ToPoint16(), UltimateSkyblock.Instance, index3);

                    floor += 1;
                    bool canDoTower = floor != 4;
                    bool switchTower = floor > 1;

                    if (dir == -1)
                    {
                        GenerateTopHallway(genPos.X, genPos.Y + 36, -1, new Random().Next(5, 8) - floor, floor, (canDoTower && !switchTower)); // left
                        GenerateTopHallway(genPos.X + 44, genPos.Y + 36, 1, new Random().Next(5, 8) - floor, floor, (canDoTower && switchTower)); // right
                    }
                    else
                    {
                        GenerateTopHallway(genPos.X, genPos.Y + 36, -1, new Random().Next(5, 8) - floor, floor, (canDoTower && switchTower)); // left
                        GenerateTopHallway(genPos.X + 44, genPos.Y + 36, 1, new Random().Next(5, 8) - floor, floor, (canDoTower && !switchTower)); // right
                    }

                    offset += towerDims.X * dir;
                    roomIndex++;
                    maxRooms++;
                    continue;
                }

                int index = RollRoom(i, roomType: "TopRooms");
                Point16 dims = Point16.Zero;
                Generator.GetMultistructureDimensions(prePath + "TopRooms", UltimateSkyblock.Instance, index, ref dims);
                if (dir == 1)
                {
                    Generator.GenerateMultistructureSpecific(prePath + "TopRooms", new Point16(x + offset, y - dims.Y), UltimateSkyblock.Instance, index);
                    offset += dims.X;
                }
                else
                {
                    offset += dims.X * dir;
                    Generator.GenerateMultistructureSpecific(prePath + "TopRooms", new Point16(x + offset, y - dims.Y), UltimateSkyblock.Instance, index);
                }
                roomIndex++;
            }
        }

        int RollRoom(int randIndex, int iterations = 0, string roomType = "DungeonRooms")
        {
            TagCompound tag = DynamicStructureSystem.GetTag(prePath + roomType, UltimateSkyblock.Instance);
            List<TagCompound> list = (List<TagCompound>)tag.GetList<TagCompound>("Structures");
            int index = new Random().Next(list.Count);

            if (index == previousRoom)
                return RollRoom(randIndex, iterations++, roomType);
            else
            {
                previousRoom = index;
                return index;
            }
        }


        /// <summary>
        /// The only reason this exists is because building with Unsafe Walls is incredibly annoying.
        /// Yes, you can destroy them after defeating skeletron, but they still need an unfilled wall
        /// next to the one you are trying to mine for it to break.
        /// I decided this was a better approach, albeit a little slower, but who cares, it's during generation time anyways.
        /// </summary>
        void MakeDungeonUnsafe()
        {
            GenLogger.ColorLog("Replacing all safe walls with unsafe walls");

            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = 0; y < Main.maxTilesY; y++)
                {
                    Tile tile = Main.tile[x, y];
                    if (tile.WallType != WallID.None)
                    {
                        switch (tile.WallType)
                        {
                            case WallID.BlueDungeon:
                                tile.WallType = WallID.BlueDungeonUnsafe;
                                break;
                            case WallID.BlueDungeonSlab:
                                tile.WallType = WallID.BlueDungeonSlabUnsafe;
                                break;
                            case WallID.BlueDungeonTile:
                                tile.WallType = WallID.BlueDungeonTileUnsafe;
                                break;

                        }
                    }
                }
            }

            GenLogger.ColorLog("Finished replacing walls");
        }

    }
}
