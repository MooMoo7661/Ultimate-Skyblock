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

        public override void PostAI(NPC npc)
        {
            if (!UltimateSkyblock.IsSkyblock() || !(NPCID.Sets.Zombies[npc.type] || NPCID.Sets.DemonEyes[npc.type]))
                return;

            if (Main.dayTime && !npc.wet && !Main.player[npc.FindClosestPlayer()].ZoneGraveyard)
                npc.AddBuff(BuffID.OnFire3, 60);

            if ((npc.HasBuff(BuffID.OnFire) || npc.HasBuff(BuffID.OnFire3)) && npc.wet)
            {
                npc.RequestBuffRemoval(BuffID.OnFire);
                npc.RequestBuffRemoval(BuffID.OnFire3);
            }
        }

        public override void OnKill(NPC npc)
        {
            if (npc.type == NPCID.SkeletronHead)
            {
                NPC.savedMech = true;
                Main.townNPCCanSpawn[NPCID.Mechanic] = true;
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

                if (spawnInfo.Player.InModBiome<DeepstoneBiome>() && spawnInfo.Player.position.Y >= GenVars.worldSurfaceLow)
                {
                    pool.Clear();
                    pool.Add(NPCID.Ghost, 0.2f);
                    pool.Add(ModContent.NPCType<DeepstoneBat>(), 0.7f);
                    pool.Add(ModContent.NPCType<Deepknight>(), 0.8f);
                    pool.Add(NPCID.WallCreeper, 0.5f);
                    pool.Add(NPCID.Skeleton, 0.3f);
                    pool.Add(NPCID.Demon, 0.6f);
                    pool.Add(NPCID.LavaSlime, 0.8f);
                    pool.Add(NPCID.Worm, 0.4f);
                    pool.Add(ModContent.NPCType<ReDead>(), 0.6f);
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