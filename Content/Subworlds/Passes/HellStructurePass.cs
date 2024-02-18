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

namespace UltimateSkyblock.Content.Subworlds.Passes
{
    public class HellStructurePass : GenPass
    {
        public HellStructurePass(string name, double loadWeight) : base(name, loadWeight) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Creating Hell Structures";
        }
    }
}
