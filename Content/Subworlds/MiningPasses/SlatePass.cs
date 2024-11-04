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
using UltimateSkyblock.Content.Subworlds;
using Terraria.ModLoader;
using UltimateSkyblock.Content.Tiles.Blocks;
using static UltimateSkyblock.Content.Subworlds.GenUtils;

namespace UltimateSkyblock.Content.Subworlds.MiningPasses
{
    public class SlatePass : GenPass
    {
        public SlatePass(string name, double loadWeight) : base(name, loadWeight) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            LoopWorldAndGenerateTilesWithDepthModifiers(15, strength: Main.rand.Next(25, 35), steps: Main.rand.Next(15, 32), type: ModContent.TileType<SlateTile>(), new List<int> { TileID.Stone }, levelToDisperse: (int)Main.rockLayer + 100, canGenerateAfterLevel: true);
        }
    }
}
