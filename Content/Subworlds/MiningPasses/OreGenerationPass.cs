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
using static UltimateSkyblock.Content.Subworlds.GenUtils;

namespace UltimateSkyblock.Content.Subworlds.MiningPasses
{
    public class OreGenerationPass : GenPass
    {
        public OreGenerationPass(string name, double loadWeight) : base(name, loadWeight) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            UltimateSkyblock.Instance.Logger.Info("Generating Sandstone");
            LoopWorldAndGenerateTilesWithDepthModifiers(8, strength: Main.rand.Next(9, 18), steps: Main.rand.Next(8, 22), type: TileID.Sandstone, tilesThatCanBeGeneratedOn: new List<int> { MiningSubworld.Slate, TileID.Stone }, levelToDisperse: Main.UnderworldLayer, canGenerateAfterLevel: false);

            UltimateSkyblock.Instance.Logger.Info("Generating Dirt");
            LoopWorldAndGenerateTilesWithDepthModifiers(2, Main.rand.Next(4, 14), Main.rand.Next(14, 31), type: TileID.Dirt, new List<int> { TileID.Stone, MiningSubworld.Slate }, Main.maxTilesY / 2, true, 500);

            UltimateSkyblock.Instance.Logger.Info("Generating Copper or Tin");
            LoopWorldAndGenerateTilesWithDepthModifiers(6, Main.rand.Next(4, 7), Main.rand.Next(16, 19), SubVars.copper, new List<int> { TileID.Stone, MiningSubworld.Slate }, Main.UnderworldLayer - 100, false, 100);

            UltimateSkyblock.Instance.Logger.Info("Generating Silver or Tungsten");
            LoopWorldAndGenerateTilesWithDepthModifiers(7, Main.rand.Next(4, 7), Main.rand.Next(14, 19), SubVars.silver, new List<int> { TileID.Stone, MiningSubworld.Slate }, Main.UnderworldLayer - 100, false, 100);

            UltimateSkyblock.Instance.Logger.Info("Generating Iron or Lead");
            LoopWorldAndGenerateTilesWithDepthModifiers(6, Main.rand.Next(6, 10), Main.rand.Next(4, 8), SubVars.iron, new List<int> { TileID.Stone, MiningSubworld.Slate }, Main.UnderworldLayer - 100, false, 100);

            UltimateSkyblock.Instance.Logger.Info("Generating Gold or Platinum");
            LoopWorldAndGenerateTilesWithDepthModifiers(8, Main.rand.Next(7, 10), Main.rand.Next(4, 6), SubVars.gold, new List<int> { TileID.Stone, MiningSubworld.Slate }, Main.UnderworldLayer - 100, false, 100);
        }
    }
}
