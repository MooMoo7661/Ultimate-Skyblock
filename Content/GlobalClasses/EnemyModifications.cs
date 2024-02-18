using System.Collections.Generic;
using SubworldLibrary;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UltimateSkyblock.Content.Biomes;
using UltimateSkyblock.Content.NPCs;
using UltimateSkyblock.Content.Subworlds;

namespace UltimateSkyblock.Content.GlobalClasses
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

        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if (SubworldSystem.IsActive<MiningSubworld>())
            {
                if (spawnInfo.Player.ZoneNormalCaverns)
                {
                    pool.Add(NPCID.WallCreeper, 0.7f);
                    pool.Add(NPCID.CaveBat, 1f);
                }

                if (spawnInfo.Player.InModBiome<DeepstoneBiome>())
                {
                    pool.Clear();
                    pool.Add(NPCID.Ghost, 0.5f);
                    pool.Add(ModContent.NPCType<DeepstoneBat>(), 0.9f);
                    pool.Add(NPCID.ArmoredSkeleton, 0.3f);
                    pool.Add(NPCID.WallCreeper, 0.5f);
                    pool.Add(NPCID.Skeleton, 0.3f);
                    pool.Add(NPCID.Demon, 0.6f);
                    pool.Add(NPCID.BlackSlime, 0.8f);
                    pool.Add(NPCID.Worm, 0.4f);
                }    
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