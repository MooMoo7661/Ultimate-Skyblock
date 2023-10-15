using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace OneBlock
{
    public class OneBlockModSystem : ModSystem
    {
        public override void PostUpdateEverything()
        {
            OB_Liquid.Update();
        }
    }
}
