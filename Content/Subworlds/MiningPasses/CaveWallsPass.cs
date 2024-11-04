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
using UltimateSkyblock.Content.Utils;

namespace UltimateSkyblock.Content.Subworlds.MiningPasses
{
    public class CaveWallsPass : GenPass
    {
        public CaveWallsPass(string name, double loadWeight) : base(name, loadWeight) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            for (int i = 0; i < (Main.maxTilesX * Main.maxTilesY) * 0.0003; i++)
            {
                int x = WorldGen.genRand.Next(20, Main.maxTilesX - 20);
                int y = WorldGen.genRand.Next(20, Main.maxTilesY - 400);

                if (!Main.tile[x, y].HasTile)
                {
                    WorldGen.Spread.Wall(x, y, RollCaveWall(y));
                }
            }
        }

        #region quickrefs
        /*
        RockyDirtWall = 59, //Cave6Echo
        OldStoneWall = 61, //Cave7Echo
        CaveDirtWall = 270, //CaveWall1Echo
        RoughDirtWall = 271, //CaveWall2Echo
        CraggyStoneWall = 185, //Cave8Unsafe
        WornStoneWall = 212, //RocksUnsafe1
        StalactiteStoneWall = 213, //RocksUnsafe2
        MottledStoneWall = 214, //RocksUnsafe3
        FracturedStoneWall = 215, //RocksUnsafe4
        */
        #endregion

        public int RollCaveWall(int y)
        {
            if (y < Main.maxTilesY - 800)
            {
                return new List<int> { WallID.Cave6Unsafe, WallID.Cave7Unsafe, WallID.CaveWall2 }.Random();
            }
            else if (y < Main.maxTilesY - 700)
            {
                return new List<int> { WallID.Cave6Unsafe, WallID.Cave7Unsafe, WallID.CaveWall2, WallID.CaveWall, WallID.RocksUnsafe1 }.Random();
            }
            else if (y < Main.maxTilesY - 600)
            {
                return new List<int> { WallID.Cave7Unsafe, WallID.CaveWall, WallID.CaveWall2, WallID.RocksUnsafe1, WallID.RocksUnsafe2, WallID.RocksUnsafe3 }.Random();
            }
            else if (y < Main.maxTilesY - 500)
            {
                return new List<int> { WallID.RocksUnsafe1, WallID.RocksUnsafe3, WallID.RocksUnsafe2, WallID.CaveWall2 }.Random();
            }
            else
            {
                return new List<int> { WallID.Cave8Unsafe, WallID.CaveWall2, WallID.RocksUnsafe2, WallID.RocksUnsafe3, WallID.RocksUnsafe4 }.Random();
            }

        }
    }
}
