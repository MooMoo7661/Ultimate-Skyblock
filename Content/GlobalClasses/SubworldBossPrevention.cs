using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubworldLibrary;
using UltimateSkyblock.Content.Subworlds;

namespace UltimateSkyblock.Content.GlobalClasses
{
    public class SubworldBossPrevention : GlobalNPC
    {
        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            if (npc.boss && (SubworldSystem.IsActive<MiningSubworld>() || SubworldSystem.IsActive<DungeonSubworld>()))
                npc.active = false;
        }
    }

    public class SubworldNPCSpawnPrevention : GlobalNPC
    {
        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            if ((npc.townNPC || npc.type == NPCID.BoundGoblin || npc.type == NPCID.BoundMechanic || npc.type == NPCID.BoundWizard || npc.type == NPCID.BoundWizard) && (SubworldSystem.IsActive<MiningSubworld>() || SubworldSystem.IsActive<DungeonSubworld>()))
                npc.active = false;
        }
    }
}
