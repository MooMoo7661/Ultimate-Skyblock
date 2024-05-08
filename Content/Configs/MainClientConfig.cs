using System.ComponentModel;
using Terraria.ModLoader.Config;
using static UltimateSkyblock.Content.SceneEffects.SkyblockScene_Radio;

namespace UltimateSkyblock.Content.Configs
{
    public class MainClientConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Header("Music")]
        [BackgroundColor(185, 216, 194)]
        [DefaultValue(true)]
        public bool OWSoundtrack { get; set; }

        [BackgroundColor(170, 205, 198)]
        [DefaultValue(-1)]
        [Slider]
        [Range(-1, 1)]
        [DrawTicks]
        [Increment(1)]
        public RadioID RadioSlider { get; set; }

        [BackgroundColor(154, 194, 201)]
        [Header("WorldSelectMenu")]
        [DefaultValue(true)]
        [ReloadRequired]
        public bool ToggleWorldSelectDetour { get; set; }

        [BackgroundColor(146, 178, 189)]
        [DefaultValue(true)]
        public bool WorldSelectTagIdentifier { get; set; }

        [BackgroundColor(138, 161, 177)]
        [DefaultValue(true)]
        public bool WorldBackgroundColorLerp { get; set; }

        [BackgroundColor(106, 121, 122)]
        [DefaultValue(true)]
        public bool WorldBorderColor { get; set; }

        ///90, 101, 95
        ///74, 98, 93
        ///90, 112, 108
        ///105, 125, 121
        ///119, 137, 133,
        ///131, 148, 144
        
    }
}
