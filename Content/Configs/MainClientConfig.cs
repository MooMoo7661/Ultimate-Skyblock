using System.ComponentModel;
using Terraria.ModLoader.Config;
using static UltimateSkyblock.Content.SceneEffects.SkyblockScene_Radio;

namespace UltimateSkyblock.Content.Configs
{
    public class MainClientConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Header("Music")]
        [DefaultValue(true)]
        public bool OWSoundtrack { get; set; }

        [DefaultValue(-1)]
        [Slider]
        [Range(-1, 1)]
        [Increment(1)]
        public RadioID RadioSlider { get; set; }
    }
}
