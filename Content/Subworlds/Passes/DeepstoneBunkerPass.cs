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

namespace UltimateSkyblock.Content.Subworlds.Passes
{
    public class DeepstoneBunkerPass : GenPass
    {
        private UltimateSkyblock Mod = UltimateSkyblock.Instance;

        public enum HallwayStyle
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
        }

        public enum LeftEndRooms
        {
            Alchemy1,
            Alchemy2,
            Blacksmith1,
            Blacksmith2,
            Blacksmith3,
            Library1,
            Library2,
            Library3,
            Lounge1,
            Lounge2,
            SpiderStatue,
        }

        public enum RightEndRooms
        {
            
        }

        public DeepstoneBunkerPass(string name, double loadWeight) : base(name, loadWeight) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            Point16 placePoint = new(Main.maxTilesX / 2, Main.UnderworldLayer - 150);
            string prePath = "Content/Subworlds/SubStructures";
            string path = "Content/Subworlds/SubStructures/Deepstone";

            Generator.GenerateStructure(path + "Tower", placePoint, Mod);



        }

        public static void GenerateHallway(int x, int y, int direction)
        {

        }
    }
}
