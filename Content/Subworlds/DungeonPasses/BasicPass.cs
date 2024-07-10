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
using System.Diagnostics;

namespace UltimateSkyblock.Content.Subworlds.DungeonPasses
{
    public class BasicPass : GenPass
    {
        public BasicPass(string name, double loadWeight) : base(name, loadWeight) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {

            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = 0; y < Main.maxTilesY; y++)
                {
                    Tile tile = Main.tile[x, y];
                    tile.Clear(TileDataType.All);
                }
            }

            Stopwatch watch = Stopwatch.StartNew();
            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = 0; y  < Main.maxTilesY; y++)
                {
                    Tile tile = Main.tile[x, y];
                    tile.TileType = TileID.Stone;
                    tile.HasTile = true;
                }
            }
            watch.Stop();
            UltimateSkyblock.Instance.Logger.Info(watch.Elapsed);

            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = 0; y < Main.maxTilesY; y++)
                {
                    Tile tile = Main.tile[x, y];
                    tile.Clear(TileDataType.All);
                }
            }

            watch = Stopwatch.StartNew();
            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = 0; y < Main.maxTilesY; y++)
                {
                    Tile tile = Framing.GetTileSafely(x, y);
                    tile.TileType = TileID.Stone;
                    tile.HasTile = true;
                }
            }
            watch.Stop();
            UltimateSkyblock.Instance.Logger.Info(watch.Elapsed);

            Main.tile[Main.maxTilesX / 2, Main.maxTilesY / 2].Clear(TileDataType.All);
            watch = Stopwatch.StartNew();
            Tile tile2 = Main.tile[Main.maxTilesX / 2, Main.maxTilesY / 2];
            tile2.TileType = TileID.Stone;
            tile2.HasTile = true;
            watch.Stop();
            UltimateSkyblock.Instance.Logger.Info(watch.Elapsed);
            Main.tile[Main.maxTilesX / 2, Main.maxTilesY / 2].Clear(TileDataType.All);
            watch = Stopwatch.StartNew();
            Tile tile3 = Framing.GetTileSafely(Main.maxTilesX / 2, Main.maxTilesY / 2);
            tile3.TileType = TileID.Stone;
            tile3.HasTile = true;
            watch.Stop();
            UltimateSkyblock.Instance.Logger.Info(watch.Elapsed);

        }
    }
}
