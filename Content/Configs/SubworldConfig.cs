using System;
using System.ComponentModel;
using Terraria.ModLoader.Config;
using UltimateSkyblock.Content.Subworlds;

namespace UltimateSkyblock.Content.Configs
{
    public class SubworldConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [Header("ColorGuide")]
        [DefaultValue(3)]
        [BackgroundColor(255, 0, 0)]
        [Slider]
        [Range(0, 5)]
        [ReloadRequired]
        public int PerlinNoiseType { get; set; }

        [DefaultValue(1)]
        [BackgroundColor(255, 0, 0)]
        [Slider]
        [Range(0, 5)]
        [ReloadRequired]
        public int FractalType { get; set; }

        [DefaultValue(true)]
        [BackgroundColor(255, 187, 127)]
        [ReloadRequired]
        public bool SubworldSaving { get; set; }
    }
}
