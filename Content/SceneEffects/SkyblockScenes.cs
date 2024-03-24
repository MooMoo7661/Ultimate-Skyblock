using Terraria;
using Terraria.ModLoader;
using UltimateSkyblock.Content.Configs;

namespace UltimateSkyblock.Content.SceneEffects
{
    public class SkyblockScene_OtherworldDay : ModSceneEffect
    {
        public override bool IsSceneEffectActive(Player player)
        {
            return player.ZoneForest && ModContent.GetInstance<MainClientConfig>().OWSoundtrack;
        }
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Content/Sounds/Music/OWDay");
        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeMedium;
    }

    public class SkyblockScene_OtherworldDesert : ModSceneEffect
    {
        public override bool IsSceneEffectActive(Player player)
        {
            return player.ZoneDesert && ModContent.GetInstance<MainClientConfig>().OWSoundtrack;
        }
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Content/Sounds/Music/OWDesert");
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