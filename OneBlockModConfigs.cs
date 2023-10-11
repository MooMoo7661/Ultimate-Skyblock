using System.ComponentModel;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace OneBlock
{
    public class OneBlockModConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [Header("General")]
        [Label("Falling out of the world teleports you to the top")]
        [DefaultValue(true)]
        public bool TeleportToTopOfWorldOnDeath { get; set; }



        [Label("Trees grow at an accelerated rate")]
        [DefaultValue(true)]
        public bool FastTrees { get; set; }



        [Label("Trees drop much more acorns")]
        [DefaultValue(true)]
        public bool TreesDropMoreAcorns { get; set; }



        [Label("Saplings can be mined back into acorns (Don't question the logic)")]
        [DefaultValue(true)]
        public bool SaplingsDropAcorns { get; set; }



        [Label("Zombies burn during the day")]
        [DefaultValue(true)]
        public bool BurnZombiesDuringDaytime { get; set; }



        [Label("Dirt and Sand can be extracted for gems and ore")]
        [DefaultValue(true)]
        public bool DirtAndSandCanBeExtracted { get; set; }


        [Label("Warning text upon entering a world that is small")]
        [DefaultValue(true)]
        public bool SmallWorldWarning { get; set; }

        [Header("Islands")]
        [Label("IslandsStyle")]
        [DefaultValue(1)]
        [Slider]
        [Increment(1)]
        [Range(1, 2)]
        [ReloadRequired]
        public int IslandsStyle { get; set; }

        [Label("Starter Chest Style")]
        [Tooltip("1: Classic - contains simple skyblock items such as a water bucket, a lava bucket, and some extra items that aid progression.\n" +
         "2: Simple - contains the very basic items, not much is given.\n" +
         "3: Luxurious - contains a lot of items that will significantly speed up progression.\n" +
         "4: None - there is no chest. Only for those who want to experience the most pain and spend a long time grinding for rare drops.")]
        [Slider]
        [DefaultValue(1)]
        [Increment(1)]
        [Range(1, 4)]
        public int StarterChestStyle { get; set; }

        [Header("Music")]
        [Label("Replace various soundtracks with Otherworld sountracks")]
        [DefaultValue(true)]
        public bool OWSoundtrack { get; set; }

        [Label("Replace Forest music with familiar soundtrack")]
        [DefaultValue(false)]
        public bool MinecraftSoundtrack { get; set; }

        [Label("Subway Surfers!!!")]
        [DefaultValue(false)]
        public bool SubwaySurfers { get; set; }

    }

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