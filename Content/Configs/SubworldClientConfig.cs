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
        [Label("Show Fast Noise Seed")]
        [DefaultValue(true)]
        [BackgroundColor(0, 255, 0)]
        public bool ShowFastNoiseSeed { get; set; }

        [Label("Show Fractal Type")]
        [DefaultValue(false)]
        [BackgroundColor(0, 255, 0)]
        public bool ShowFractalType { get; set; }

        [Label("Show Perlin Type")]
        [DefaultValue(false)]
        [BackgroundColor(0, 255, 0)]
        public bool ShowPerlinType { get; set; }
    }
}
