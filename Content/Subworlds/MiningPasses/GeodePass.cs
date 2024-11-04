using Terraria.ID;
using Terraria;
using Terraria.IO;
using Terraria.WorldBuilding;
using UltimateSkyblock.Content.Items.Placeable;
using static UltimateSkyblock.Content.Subworlds.MiningSubworld;
using Microsoft.Xna.Framework;

namespace UltimateSkyblock.Content.Subworlds.MiningPasses
{
    public class GeodePass : GenPass
    {
        public GeodePass(string name, double loadWeight) : base(name, loadWeight) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Generating Geodes";

            for (int q = 0; q < 20; q++)
            {
                CreateGeode(ref progress, 0); 
            }
        }

        void CreateGeode(ref GenerationProgress progress, byte iteration)
        {
            int x = WorldGen.genRand.Next(60, Main.maxTilesX - 60);
            int y = WorldGen.genRand.Next(60, Main.UnderworldLayer - 200);

            Tile tile = Framing.GetTileSafely(x, y);

            if (new Vector2(x, y).Distance(new(Main.spawnTileX, Main.spawnTileY)) > 95 || tile.HasTile && !GenUtils.AreaContainsSensitiveTiles(new List<int> { TileID.Marble, TileID.WoodBlock, TileID.GrayBrick }, x, y, 9, 9) && GenUtils.MostlyAir(20, 20, x, y) && tile.TileType == TileID.Stone && WorldGen.InWorld(x, y) && iteration < 60)
            {
                Point placePoint = new Point(x, y);
                ShapeData fullData = new ShapeData();
                ShapeData innerData = new ShapeData();

                for (int i = 0; i < 2; i++)
                {
                    int width = WorldGen.genRand.Next(8, 14);
                    int height = WorldGen.genRand.Next(8, 14);

                    WorldUtils.Gen(placePoint, new Shapes.Circle(width, height), new Actions.SetTile(TileID.Marble));
                    WorldUtils.Gen(placePoint, new Shapes.Circle(width, height), new Actions.Blank().Output(fullData));
                }

                for (int i = 0; i < 5; i++)
                {
                    int width = WorldGen.genRand.Next(4, 7);
                    int height = WorldGen.genRand.Next(4, 7);

                    WorldUtils.Gen(placePoint, new Shapes.Circle(width, height), new Actions.ClearTile());
                    WorldUtils.Gen(placePoint, new Shapes.Circle(width, height), new Actions.Blank().Output(innerData));
                }

                WorldGen.gemCave(x, y);
                progress.Set((y + x * Main.maxTilesY) / (float)(Main.maxTilesX * Main.maxTilesY));
            }
            else
                CreateGeode(ref progress, iteration++);
        }
    }
}
