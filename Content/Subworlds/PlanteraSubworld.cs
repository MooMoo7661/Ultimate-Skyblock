using SubworldLibrary;
using System.Collections.Generic;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Terraria.IO;
using Microsoft.Xna.Framework;
using UltimateSkyblock.Content.Tiles.Blocks;

namespace UltimateSkyblock.Content.Subworlds
{
    public class PlanteraSubworld : Subworld
    {
        public override int Width => 500;

        public override int Height => 500;

        public override bool ShouldSave => false;

        public override string Name => "Plantera Subworld";

        public override List<GenPass> Tasks => new()
        {
           new ArenaPass("Dumping Mud", 1),
        };

        public override void OnEnter()
        {
            SubworldSystem.hideUnderworld = true;
        }

        public class ArenaPass : GenPass
        {
            public ArenaPass(string name, float loadWeight) : base(name, loadWeight)
            {
            }

            protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
            {
                progress.Message = "Building Arena";

                FillWorldWithMud();
                MudWallRunner();
                PlaceAltar(Main.maxTilesX / 2, Main.maxTilesY / 2 - 8);
                PlaceTorchesAndPlatforms();
            }
        }

        public static void FillWorldWithMud()
        {
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    WorldGen.PlaceTile(i, j, TileID.Mud, true, true);
                }
            }

            ShapeData shapeData = new ShapeData();
            ShapeData shapeData2 = new ShapeData();

            int size = 50;
            WorldUtils.Gen(new Point(Main.maxTilesX / 2, Main.maxTilesY / 2), new Shapes.Circle(size, size), new Actions.ClearTile());
            WorldUtils.Gen(new Point(Main.maxTilesX / 2, Main.maxTilesY / 2), new Shapes.Circle(size, size), new Actions.Blank().Output(shapeData));
            WorldUtils.Gen(new Point(Main.maxTilesX / 2, Main.maxTilesY / 2), new ModShapes.OuterOutline(shapeData, true), new Actions.SetTile(TileID.JungleGrass));
            WorldUtils.Gen(new Point(Main.maxTilesX / 2, Main.maxTilesY / 2), new ModShapes.OuterOutline(shapeData, true), new Actions.Smooth(true));

            WorldUtils.Gen(new Point(Main.maxTilesX / 2, Main.maxTilesY / 2), new Shapes.Circle(5, 5), new Actions.Blank().Output(shapeData2));
            WorldUtils.Gen(new Point(Main.maxTilesX / 2, Main.maxTilesY / 2), new Shapes.Circle(5, 5), new Actions.SetTile(TileID.Mud));
            WorldUtils.Gen(new Point(Main.maxTilesX / 2, Main.maxTilesY / 2), new ModShapes.OuterOutline(shapeData2, true), new Actions.SetTile(TileID.JungleGrass));
        }

        public static void MudWallRunner()
        {
            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = 0; y < Main.maxTilesY; y++)
                {
                    WorldGen.PlaceWall(x, y, WallID.MudUnsafe, true);
                }
            }
        }

        public static void PlaceAltar(int x, int y)
        {
            WorldGen.PlaceObject(x, y, ModContent.TileType<PlanteraAltar>());
            for (int i = 0; i < 5; i++)
            {
                WorldGen.PlaceTile(x + 2 - i, y + 2, TileID.Mudstone, true, true);
                if (i == 0 || i == 4)
                    WorldGen.PlaceTile(x + 2 - i, y + 1, TileID.Torches, true, false);
            }

            WorldGen.PlaceTile(x, y - 6, TileID.Torches);
        }

        public static void PlaceTorchesAndPlatforms()
        {
            int l = Main.maxTilesX / 2;
            int m = Main.maxTilesY / 2;

            WorldGen.PlaceTile(l, m, TileID.Mudstone);

            for (int i = 0; i < Main.maxTilesY; i++)
            {
                int torchCounter = 0;
                for (int j = 0; j < Main.maxTilesX; j++)
                {
                    Tile tile = Framing.GetTileSafely(i, j);
                    if (!tile.HasTile)
                    {
                        if (i % 10 == 0)
                        {
                            torchCounter++;
                            if (torchCounter == 10)
                            {
                                WorldGen.PlaceTile(i, j, TileID.Torches);
                                torchCounter = 0;
                            }
                        }

                        if (j % 13 == 0)
                        {
                            bool forced = false;
                            if (Framing.GetTileSafely(i, j).TileType == TileID.Torches)
                            {
                                forced = true;
                            }

                            WorldGen.PlaceTile(i, j, TileID.Platforms, true, forced);
                        }
                    }
                }
            }
        }
    }
}