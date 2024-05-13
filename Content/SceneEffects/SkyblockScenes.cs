using System;
using log4net;
using SubworldLibrary;
using Terraria;
using Terraria.ModLoader;
using UltimateSkyblock.Content.Biomes;
using UltimateSkyblock.Content.Configs;
using UltimateSkyblock.Content.Subworlds;
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
                    1 => MusicLoader.GetMusicSlot(Mod, "Content/Sounds/Music/PortalRadio"),
                    _ => MusicLoader.GetMusicSlot(Mod, "Content/Sounds/Music/SubwaySurfers")
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

    public class SkyblockScene_OceanRemover : ModSceneEffect
    {
        public override bool IsSceneEffectActive(Player player)
        {
            return player.ZoneBeach && SubworldSystem.Current == ModContent.GetInstance<MiningSubworld>();
        }

        public override int Music => MusicID.Underground;

        public override ModUndergroundBackgroundStyle UndergroundBackgroundStyle => ModContent.GetInstance<OceanRemoverBackground>();

        public override SceneEffectPriority Priority => SceneEffectPriority.Environment;

        public override float GetWeight(Player player)
        {
            return 1f;
        }
    }

    public class OceanRemoverBackground : ModUndergroundBackgroundStyle
    {
        public override void FillTextureArray(int[] textureSlots)
        {
            textureSlots[0] = BackgroundTextureLoader.GetBackgroundSlot(Mod, "Assets/Textures/Backgrounds/OceanRemover0");
            textureSlots[1] = BackgroundTextureLoader.GetBackgroundSlot(Mod, "Assets/Textures/Backgrounds/OceanRemover1");
            textureSlots[2] = BackgroundTextureLoader.GetBackgroundSlot(Mod, "Assets/Textures/Backgrounds/OceanRemover0");
            textureSlots[3] = BackgroundTextureLoader.GetBackgroundSlot(Mod, "Assets/Textures/Backgrounds/OceanRemover1");
        }
    }
}