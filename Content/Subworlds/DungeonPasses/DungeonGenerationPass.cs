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
using static UltimateSkyblock.Content.Subworlds.DungeonRoomUtils.DungeonRoom;
using System.Reflection;

namespace UltimateSkyblock.Content.Subworlds.DungeonPasses
{
    public class DungeonGenerationPass : GenPass
    {
        public DungeonGenerationPass(string name, double loadWeight) : base(name, loadWeight) { }

        public static int previousRoom = -1;
        public static string prePath = "Content/Subworlds/DungeonStructures/";

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            Point16 dims = Point16.Zero;
            Generator.GetDimensions(prePath + "DungeonEntrance", UltimateSkyblock.Instance, ref dims);
            Point16 MainPoint = new Point16(Main.maxTilesX / 2 - dims.X / 2, Main.maxTilesY / 2);

            Generator.GenerateMultistructureRandom(prePath + "DungeonEntrance", MainPoint, UltimateSkyblock.Instance);
            HallwayRunner(new Point16(MainPoint.X, MainPoint.Y));

            Generator.GetMultistructureDimensions(prePath + "DungeonEntrance", UltimateSkyblock.Instance, 0, ref dims);
            Point spawn = new(MainPoint.X + dims.X / 2, MainPoint.Y + dims.Y - 6);
            Main.spawnTileX = spawn.X;
            Main.spawnTileY = spawn.Y;
        }

        public void HallwayRunner(Point16 genPoint)
        {
            GenerateBottomHallway(genPoint.X, genPoint.Y + 34, -1, 4);
            GenerateBottomHallway(genPoint.X + 67, genPoint.Y + 34, 1, 4);
        }

        public void GenerateBottomHallway(int x, int y, int dir, int maxRooms)
        {
            int offset = 0;
            previousRoom = -1;

            for (int i = 0; i < maxRooms; i++)
            {
                int index = RollRoom(i);
                Point16 dims = Point16.Zero;
                Generator.GetMultistructureDimensions(prePath + "DungeonRooms", UltimateSkyblock.Instance, index, ref dims);

                //When generating to the right, only offset AFTER generation
                if (dir == 1)
                {
                    Generator.GenerateMultistructureSpecific(prePath + "DungeonRooms", new Point16(x + offset, y - dims.Y), UltimateSkyblock.Instance, index);
                    offset += dims.X;
                }
                else
                {
                    offset += dims.X * dir;
                    Generator.GenerateMultistructureSpecific(prePath + "DungeonRooms", new Point16(x + offset, y - dims.Y), UltimateSkyblock.Instance, index);
                }
            }
        }

        int RollRoom(int randIndex, int iterations = 0)
        {
            TagCompound tag = DynamicStructureSystem.GetTag(prePath + "DungeonRooms", UltimateSkyblock.Instance);
            List<TagCompound> list = (List<TagCompound>)tag.GetList<TagCompound>("Structures");
            int seed = DateTime.Now.Millisecond + randIndex * iterations;
            int index = new UnifiedRandom(seed).Next(list.Count);
            UltimateSkyblock.Instance.Logger.Info("Seed: " + seed);

            UltimateSkyblock.Instance.Logger.Info("Rolled index: " + index + ", previous index: " + previousRoom);
            if (index == previousRoom)
                return RollRoom(randIndex, iterations++);
            else
            {
                previousRoom = index;
                return index;
            }
        }
    }
}
