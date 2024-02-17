using SubworldLibrary;
using UltimateSkyblock.Content.Subworlds;
using Terraria;
using Terraria.ModLoader;

namespace UltimateSkyblock.Content.SceneEffects
{
    public class SubworldSceneEffects : ModSceneEffect
	{
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/UndergroundJungle");

        public override bool IsSceneEffectActive(Player player)
        {
            if (SubworldSystem.Current == ModContent.GetInstance<PlanteraSubworld>())
            {
                return true;                
            }

            return false;
        }

        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;
    }
}