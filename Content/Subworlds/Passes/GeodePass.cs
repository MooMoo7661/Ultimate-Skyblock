using Terraria.ID;
using Terraria;
using Terraria.IO;
using Terraria.WorldBuilding;
using UltimateSkyblock.Content.Items.Placeable;
using static UltimateSkyblock.Content.Subworlds.MiningSubworld;
using Microsoft.Xna.Framework;

namespace UltimateSkyblock.Content.Subworlds.Passes
{
    public class GeodePass : GenPass
    {
        public GeodePass(string name, double loadWeight) : base(name, loadWeight) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            int geodesCount = 0;
            progress.Message = "Generating Geodes";

            for (int q = 0; q < 30; q++)
            {

                int x = Main.rand.Next(100, Main.maxTilesX - 100);
                int y = Main.rand.Next(100, Main.UnderworldLayer - 200);

                Tile tile = Framing.GetTileSafely(x, y);

                if (tile.HasTile && tile.TileType == TileID.Stone && WorldGen.InWorld(x, y) && geodesCount < 15)
                {
                    Point placePoint = new Point(x, y);
                    ShapeData fullData = new ShapeData();
                    ShapeData innerData = new ShapeData();

                    for (int i = 0; i < 2; i++)
                    {
                        int width = Main.rand.Next(7, 12);
                        int height = Main.rand.Next(7, 12);


                        WorldUtils.Gen(placePoint, new Shapes.Circle(width, height), new Actions.SetTile(TileID.Marble));
                        WorldUtils.Gen(placePoint, new Shapes.Circle(width, height), new Actions.Blank().Output(fullData));

                    }

                    for (int i = 0; i < 5; i++)
                    {
                        int width = Main.rand.Next(4, 7);
                        int height = Main.rand.Next(4, 7);

                        WorldUtils.Gen(placePoint, new Shapes.Circle(width, height), new Actions.ClearTile());
                        WorldUtils.Gen(placePoint, new Shapes.Circle(width, height), new Actions.Blank().Output(innerData));
                    }

                    WorldGen.gemCave(x, y);
                    progress.Set((y + x * Main.maxTilesY) / (float)(Main.maxTilesX * Main.maxTilesY));
                }
            }
        }
    }
}
