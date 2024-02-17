using Terraria;
using Terraria.ModLoader;
using UltimateSkyblock.Content.Configs;

namespace UltimateSkyblock.Content.SceneEffects
{
    public class SkyblockScene_Minecraft : ModSceneEffect
    {
        public override bool IsSceneEffectActive(Player player)
        {
            return player.ZoneForest && ModContent.GetInstance<SkyblockModConfig>().MinecraftSoundtrack;
        }
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Content/Sounds/Music/MinecraftSoundtrack");
        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;
    }

    public class SkyblockScene_OtherworldDay : ModSceneEffect
    {
        public override bool IsSceneEffectActive(Player player)
        {
            return player.ZoneForest && ModContent.GetInstance<SkyblockModConfig>().OWSoundtrack;
        }
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Content/Sounds/Music/OWDay");
        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeMedium;
    }

    public class SkyblockScene_OtherworldDesert : ModSceneEffect
    {
        public override bool IsSceneEffectActive(Player player)
        {
            return player.ZoneDesert && ModContent.GetInstance<SkyblockModConfig>().OWSoundtrack;
        }
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Content/Sounds/Music/OWDesert");
        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeMedium;
    }

    public class SkyblockScene_SubwaySurfers : ModSceneEffect
    {
        public override bool IsSceneEffectActive(Player player)
        {
            return ModContent.GetInstance<SkyblockModConfig>().SubwaySurfers;
        }
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Content/Sounds/Music/SubwaySurfers");
        public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;

        public override float GetWeight(Player player)
        {
            return 1f;
        }
    }
}