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
        public bool ShowFastNoiseSeed { get; set; }

        [DefaultValue(false)]
        public bool ShowFractalType { get; set; }

        [DefaultValue(false)]
        public bool ShowPerlinType { get; set; }
    }
}
