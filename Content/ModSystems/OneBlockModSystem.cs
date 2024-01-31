using OneBlock.Content.StoneGenerator;
using Terraria.ModLoader;

namespace OneBlock.Content.ModSystems
{
    public class OneBlockModSystem : ModSystem
    {
        public override void PostUpdateEverything()
        {
            OB_Liquid.Update();
        }
    }
}
