using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace OneBlock.Content.Configs
{
    public class RecipesConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [Header("Misc")]
        [Label("Allow decrafting tombstones into gold and stone")]
        [DefaultValue(true)]
        public bool DecraftTombstones { get; set; }

        [Label("Tin cans can be smelted down")]
        [DefaultValue(true)]
        public bool SmeltTinCans { get; set; }

        [Header("Items")]
        [Label("Craft Staff of Regrowth")]
        [DefaultValue(true)]
        public bool CraftRegrowthStaff { get; set; }

        [Label("Craft Extractinator")]
        [DefaultValue(true)]
        public bool CraftExtractinator { get; set; }

        [Label("Craft Life Crystals")]
        [DefaultValue(true)]
        public bool CraftLifeCrystals { get; set; }

        [Label("Craft Gelatin Crystals")]
        [DefaultValue(true)]
        public bool CraftGelatinCrystals { get; set; }

        [Label("Craft Ice Skates")]
        [DefaultValue(true)]
        public bool CraftIceSkates { get; set; }

        [Label("Craft Hermes Boots")]
        [DefaultValue(true)]
        public bool CraftHermesBoots { get; set; }

        [Label("Craft Magic Mirror")]
        [DefaultValue(true)]
        public bool CraftMagicMirror { get; set; }

        [Label("Craft Ice Mirror")]
        [DefaultValue(true)]
        public bool CraftIceMirror { get; set; }

        [Label("Craft Band of Regeneration")]
        [DefaultValue(true)]
        public bool CraftRegenerationBand { get; set; }

        [Label("Craft Step Stool")]
        [DefaultValue(true)]
        public bool CraftStepStool { get; set; }

        [Label("Flurry Boots Recipe")]
        [DefaultValue(true)]
        public bool CraftFlurryBoots { get; set; }

        [Label("Dunerider Boots Recipe")]
        [DefaultValue(true)]
        public bool CraftDuneriderBoots { get; set; }

        [Label("Aglet Recipe")]
        [DefaultValue(true)]
        public bool CraftAglet { get; set; }

    }
}