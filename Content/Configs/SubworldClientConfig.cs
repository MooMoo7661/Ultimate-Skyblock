using System;
using System.ComponentModel;
using Terraria.ModLoader.Config;
using UltimateSkyblock.Content.Subworlds;

namespace UltimateSkyblock.Content.Configs
{
    public class SubworldClientConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Header("WorldGenDebugInfo")]
        [DefaultValue(true)]
        [BackgroundColor(0, 255, 0)]
        public bool ShowFastNoiseSeed { get; set; }

        [DefaultValue(false)]
        [BackgroundColor(0, 255, 0)]
        public bool ShowFractalType { get; set; }

        [DefaultValue(false)]
        [BackgroundColor(0, 255, 0)]
        public bool ShowPerlinType { get; set; }
    }
}
