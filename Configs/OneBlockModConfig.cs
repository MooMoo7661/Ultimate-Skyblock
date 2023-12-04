using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.Config;

namespace OneBlock.Configs
{
    public class OneBlockModConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [Header("General")]
        [Label("Falling out of the world teleports you to the top")]
        [DefaultValue(true)]
        public bool TeleportToTopOfWorldOnDeath { get; set; }

        [Label("Stone Generator Logic")]
        [Tooltip("If lava and water are seperated by one block of air, it will generate stone in that spot, or obsidian if in the caverns.\nIt must be left -> right, or right -> left.")]
        [DefaultValue(true)]
        public bool StoneGenerator { get; set; }

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

        [Label("Render Fog Cloud Tiles")]
        [DefaultValue(false)]
        public bool RenderFogCloudTiles { get; set; }

        [Label("Warning text upon entering a world that is small")]
        [DefaultValue(true)]
        public bool SmallWorldWarning { get; set; }

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
}
