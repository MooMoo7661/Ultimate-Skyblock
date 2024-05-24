using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.Config;

namespace UltimateSkyblock.Content.Configs
{
    public class BugsConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [BackgroundColor(172, 228, 170)]
        [DefaultValue(false)]
        public bool HeartCrystalDropFix { get; set; }
    }
}
