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

        [DefaultValue(true)]
        public bool StoneGenerator { get; set; }

        [DefaultValue(true)]
        public bool FastTrees { get; set; }

        [DefaultValue(true)]
        public bool TreesDropMoreAcorns { get; set; }

        [DefaultValue(true)]
        public bool SaplingsDropAcorns { get; set; }

        [DefaultValue(true)]
        public bool BurnZombiesDuringDaytime { get; set; }

        [DefaultValue(true)]
        public bool DirtAndSandCanBeExtracted { get; set; }

        [Slider]
        [DefaultValue(0)]
        [Increment(1)]
        [Range(0, 3)]
        public ChestType StarterChestStyle { get; set; }

        [DefaultValue(true)]
        public bool DrawNPCIcons { get; set; }

        [Header("Debug")]
        [DefaultValue(false)]
        public bool RenderFogCloudTiles { get; set; }

        [DefaultValue(true)]
        public bool SmallWorldWarning { get; set; }
    }
}
