using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace UltimateSkyblock.Content.Configs
{
    public class SubworldClientConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Header("WorldGenDebugInfo")]
        [BackgroundColor(193, 223, 240)]
        [DefaultValue(true)]
        public bool ShowFastNoiseSeed { get; set; }

        [BackgroundColor(136, 204, 241)]
        [DefaultValue(false)]
        public bool ShowFractalType { get; set; }

        [DefaultValue(false)]
        [BackgroundColor(53, 135, 164)]
        public bool ShowPerlinType { get; set; }

        [DefaultValue(true)]
        [BackgroundColor(45, 132, 138)]
        public bool SubworldLoadingMusic { get; set; }
    }
}
