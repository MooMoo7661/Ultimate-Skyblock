using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubworldLibrary;
using Terraria.ID;
using Terraria.ModLoader;

namespace UltimateSkyblock.Content.Subworlds
{
    public class SubworldNPCSpawning : GlobalNPC
    {
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if (SubworldSystem.IsActive<MiningSubworld>())
            {
                pool.Remove(NPCID.LavaSlime);
            }
        }
    }
}
