using UltimateSkyblock.Content.StoneGenerator;
using Terraria.ModLoader;

namespace UltimateSkyblock.Content.ModSystems
{
    public class SkyblockModSystem : ModSystem
    {
        public override void PostUpdateEverything()
        {
            OB_Liquid.Update();
        }
    }
}
