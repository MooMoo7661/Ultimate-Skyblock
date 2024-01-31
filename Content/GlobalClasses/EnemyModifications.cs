using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OneBlock.Content.GlobalClasses
{
    public class EnemyModifications : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {

            return NPCID.Sets.Zombies[entity.type];
        }
        public override void PostAI(NPC npc)
        {
            if (Main.dayTime && !npc.HasBuff(BuffID.Wet))
                npc.AddBuff(BuffID.OnFire3, 60);

            if ((npc.HasBuff(BuffID.OnFire) || npc.HasBuff(BuffID.OnFire3)) && npc.HasBuff(BuffID.Wet))
            {
                npc.RequestBuffRemoval(BuffID.OnFire);
                npc.RequestBuffRemoval(BuffID.OnFire3);
            }
        }

        public override void SetStaticDefaults()
        {
            NPCID.Sets.Zombies[NPCID.ArmedTorchZombie] = true;
            NPCID.Sets.Zombies[NPCID.ArmedZombie] = true;
            NPCID.Sets.Zombies[NPCID.ArmedZombieCenx] = true;
            NPCID.Sets.Zombies[NPCID.ArmedZombieEskimo] = true;
            NPCID.Sets.Zombies[NPCID.ArmedZombiePincussion] = true;
            NPCID.Sets.Zombies[NPCID.ArmedZombieSlimed] = true;
            NPCID.Sets.Zombies[NPCID.ArmedZombieSwamp] = true;
            NPCID.Sets.Zombies[NPCID.ArmedZombieTwiggy] = true;
            NPCID.Sets.Zombies[NPCID.MaggotZombie] = true;
        }
    }
}