// Ignore Spelling: Teleport

using System;
using System.ComponentModel;
using Terraria.ModLoader.Config;
using static UltimateSkyblock.Content.SceneEffects.SkyblockScene_Radio;
using static UltimateSkyblock.Content.SkyblockWorldGen.MainWorld;

namespace UltimateSkyblock.Content.Configs
{
    public class SkyblockModConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [Header("General")]

        [BackgroundColor(222, 197, 227)]
        [DefaultValue(true)]
        public bool StoneGenerator { get; set; }

        [BackgroundColor(214, 217, 240)]
        [DefaultValue(true)]
        public bool FastTrees { get; set; }

        [BackgroundColor(205, 237, 253)]
        [DefaultValue(true)]
        public bool TreesDropMoreAcorns { get; set; }

        [BackgroundColor(194, 229, 254)]
        [DefaultValue(true)]
        public bool SaplingsDropAcorns { get; set; }

        [BackgroundColor(182, 220, 254)]
        [DefaultValue(true)]
        public bool BurnZombiesDuringDaytime { get; set; }

        [BackgroundColor(176, 234, 253)]
        [DefaultValue(true)]
        public bool DirtAndSandCanBeExtracted { get; set; }

        [BackgroundColor(169, 248, 251)]    
        [SliderColor(169, 195, 251)]
        [Slider]
        [DefaultValue(0)]
        [Increment(1)]
        [Range(0, 3)]
        public ChestType StarterChestStyle { get; set; }

        [BackgroundColor(129, 247, 229)]
        [DefaultValue(true)]
        public bool DrawNPCIcons { get; set; }

        [BackgroundColor(175, 248, 198)]
        [Header("Debug")]
        [DefaultValue(false)]
        public bool RenderFogCloudTiles { get; set; }

        [BackgroundColor(221, 249, 203)]
        [DefaultValue(true)]
        public bool SmallWorldWarning { get; set; }
    }
}
