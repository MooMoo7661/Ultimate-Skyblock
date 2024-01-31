using Terraria;
using Terraria.ModLoader;
using OneBlock.Content.Configs;

namespace OneBlock.Content.SceneEffects
{
    public class OneBlockScenes_Minecraft : ModSceneEffect
    {
        public override bool IsSceneEffectActive(Player player)
        {
            return player.ZoneForest && ModContent.GetInstance<OneBlockModConfig>().MinecraftSoundtrack;
        }
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Content/Sounds/Music/MinecraftSoundtrack");
        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;
    }

    public class OneBlockScenes_OtherworldDay : ModSceneEffect
    {
        public override bool IsSceneEffectActive(Player player)
        {
            return player.ZoneForest && ModContent.GetInstance<OneBlockModConfig>().OWSoundtrack;
        }
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Content/Sounds/Music/OWDay");
        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeMedium;
    }

    public class OneBlockScenes_OtherworldDesert : ModSceneEffect
    {
        public override bool IsSceneEffectActive(Player player)
        {
            return player.ZoneDesert && ModContent.GetInstance<OneBlockModConfig>().OWSoundtrack;
        }
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Content/Sounds/Music/OWDesert");
        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeMedium;
    }

    public class OneBlockScenes_SubwaySurfers : ModSceneEffect
    {
        public override bool IsSceneEffectActive(Player player)
        {
            return ModContent.GetInstance<OneBlockModConfig>().SubwaySurfers;
        }
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Content/Sounds/Music/SubwaySurfers");
        public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;

        public override float GetWeight(Player player)
        {
            return 1f;
        }
    }
}