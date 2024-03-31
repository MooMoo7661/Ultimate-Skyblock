using System;
using Terraria;
using Terraria.ModLoader;
using UltimateSkyblock.Content.Biomes;
using UltimateSkyblock.Content.Configs;
using UltimateSkyblock.Content.Tiles.Blocks;

namespace UltimateSkyblock.Content.SceneEffects
{
    public class SkyblockScene_OtherworldTrackManager : ModSceneEffect
    {
        public override bool IsSceneEffectActive(Player player)
        {
            return ModContent.GetInstance<MainClientConfig>().OWSoundtrack;
        }
        public override int Music
        {
            get
            {
                Player player = Main.LocalPlayer;

                if (Main.dayTime && Main.LocalPlayer.ZoneForest)
                 return MusicLoader.GetMusicSlot(Mod, "Content/Sounds/Music/OWDay");
                 else if (!Main.dayTime && Main.LocalPlayer.ZoneForest)
                    return MusicLoader.GetMusicSlot(Mod, "Content/Sounds/Music/OWNight");

                if (player.ZoneHallow)
                    return MusicLoader.GetMusicSlot(Mod, "Content/Sounds/Music/OWHallow");

                if (player.ZoneDesert)
                    return MusicLoader.GetMusicSlot(Mod, "Content/Sounds/Music/OWDesert");

                if (player.ZoneUnderworldHeight)
                    return MusicLoader.GetMusicSlot(Mod, "Content/Sounds/Music/OWHell");

                return -1;
            }
        }
        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeMedium;
    }

    public class SkyblockScene_Radio : ModSceneEffect
    {
        public enum RadioID
        {
            None = -1,
            SubwaySurfers,
            PortalRadio
        }

        public override bool IsSceneEffectActive(Player player)
        {
            return ModContent.GetInstance<MainClientConfig>().RadioSlider != RadioID.None;
        }

        public override int Music
        {
            get
            {
                int result = (int)ModContent.GetInstance<MainClientConfig>().RadioSlider switch
                {
                    0 => MusicLoader.GetMusicSlot(Mod, "Content/Sounds/Music/SubwaySurfers"),
                    1 => MusicLoader.GetMusicSlot(Mod, "Content/Sounds/Music/PortalRadio")
                };

                return result;
            }
        }
        public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;

        public override float GetWeight(Player player)
        {
            return 1f;
        }
    }
}