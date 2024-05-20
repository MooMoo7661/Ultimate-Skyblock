using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace UltimateSkyblock.Content.Configs
{
    public class RecipesConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [Header("Misc")]
        [BackgroundColor(237, 231, 238)]
        [DefaultValue(true)]
        public bool DecraftTombstones { get; set; }

        [DefaultValue(true)]
        [BackgroundColor(227, 229, 229)]
        public bool SmeltTinCans { get; set; }

        [Header("Items")]
        [DefaultValue(true)]
        [BackgroundColor(222, 228, 225)]
        public bool CraftRegrowthStaff { get; set; }

        [DefaultValue(true)]
        [BackgroundColor(216, 226, 220)]
        public bool CraftExtractinator { get; set; }

        [DefaultValue(true)]
        [BackgroundColor(236, 228, 219)]
        public bool CraftLifeCrystals { get; set; }
 
        [DefaultValue(true)]
        [BackgroundColor(246, 229, 218)]
        public bool CraftGelatinCrystals { get; set; }

        [DefaultValue(true)]
        [BackgroundColor(255, 229, 217)]
        public bool CraftIceSkates { get; set; }

        [DefaultValue(true)]
        [BackgroundColor(255, 216, 215)]
        public bool CraftHermesBoots { get; set; }

        [DefaultValue(true)]
        [BackgroundColor(255, 202, 212)]
        public bool CraftIceMirror { get; set; }

        [DefaultValue(true)]
        [BackgroundColor(250, 187, 198)]
        public bool CraftRegenerationBand { get; set; }

        [DefaultValue(true)]
        [BackgroundColor(247, 180, 191)]
        public bool CraftStepStool { get; set; }

        [DefaultValue(true)]
        [BackgroundColor(244, 172, 183)]
        public bool CraftFlurryBoots { get; set; }

        [DefaultValue(true)]
        [BackgroundColor(201, 151, 160)]
        public bool CraftDuneriderBoots { get; set; }

        [DefaultValue(true)]
        [BackgroundColor(179, 140, 149)]
        public bool CraftAglet { get; set; }

        [DefaultValue(true)]
        [BackgroundColor(157, 129, 137)]
        public bool PotionPlantDecrafting { get; set; }

        //157, 129, 137
        //166, 140, 148
        //174, 150, 158
        //176, 153, 176
        //177, 154, 185
        //178, 155, 194
        //180, 158, 212
        //181, 160, 230

    }
}