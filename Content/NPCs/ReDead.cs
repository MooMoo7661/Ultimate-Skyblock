using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.UI;
using UltimateSkyblock.Content.Biomes;
using UltimateSkyblock.Content.Buffs;

namespace UltimateSkyblock.Content.NPCs
{
    public class ReDead : ModNPC
    {
        public enum State
        {
            Idle,
            Chasing,
            Attacking,
            Dead,
            Fall
        }

        public int frameHeight;
        public ref float NPCState => ref NPC.ai[0];
        public ref float NPCTimer => ref NPC.ai[1];
        public bool scream = false;
        public int screamCooldown = 0;
        public bool locked = false;
        public int detectionRange = 150;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 9; // make sure to set this for your modnpcs.

            // Specify the debuffs it is immune to.
            // This NPC will be immune to the Poisoned debuff.
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
        }

        public override void SetDefaults()
        {
            NPC.width = 29;
            NPC.height = 92;
            NPC.aiStyle = -1;
            NPC.damage = 80;
            NPC.defense = 8;
            NPC.lifeMax = 250;
            NPC.HitSound = new SoundStyle("UltimateSkyblock/Content/Sounds/ReDeadHit");
            NPC.DeathSound = new SoundStyle("UltimateSkyblock/Content/Sounds/ReDeadDie");
            NPC.value = 300;
            NPC.knockBackResist = 5f;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<DeepstoneBiome>().Type };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                new FlavorTextBestiaryInfoElement("A tall, zombie-like monster. They frequently inhabit tombs and dungeons, acting like a statue until approached."),

                new BestiaryPortraitBackgroundProviderPreferenceInfoElement(ModContent.GetInstance<DeepstoneBiome>().ModBiomeBestiaryInfoElement),
        });
        }

        public override void OnSpawn(IEntitySource source)
        {
            NPCState = (int)State.Idle;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            
        }

        public override void AI()
        {
            NPC.TargetClosest(false);
           
            Collision.StepUp(ref NPC.position, ref NPC.velocity, NPC.width, NPC.height, ref NPC.stepSpeed, ref NPC.gfxOffY);

            NPCTimer++;
            if (NPCTimer >= 50)
            {
                NPCTimer = 0;
            }

            NPC.velocity.Y += 0.35f;
            
            if (NPC.HasValidTarget)
            {
                if (Main.player[NPC.target].Distance(NPC.Center) < detectionRange && !Main.player[NPC.target].dead)
                    Chasing();
                else
                    Idle();
            }

            if (screamCooldown > 0)
            {
                screamCooldown--;
                scream = true;
            }
            else
                scream = false;
        }

        public void Idle()
        {
            NPCState = (int)State.Idle;
            NPC.velocity.X = 0;
            scream = false;
            locked = false;
            detectionRange = 150;
        }

        public void Chasing()
        {
            NPCState = (int)State.Chasing;

            if (NPCTimer <= 25)
                NPC.velocity.X = NPC.velocity.DirectionTo(Main.player[NPC.target].Center).X * 0.8f * NPC.direction * -1;

            NPC.direction = Main.player[NPC.target].Center.X < NPC.Center.X ? 1 : -1;
            NPC.spriteDirection = NPC.direction;
            detectionRange = 450;

            if (!scream && !locked)
            {
                SoundEngine.PlaySound(new SoundStyle("UltimateSkyblock/Content/Sounds/ReDeadScream"));
                Main.player[NPC.target].AddBuff(ModContent.BuffType<Fear>(), 300);
                screamCooldown = 240;
                locked = true;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            switch (NPCState)
            {
                case (int)State.Idle:
                    NPC.frame.Y = 2;
                    NPC.frameCounter = 0;
                    break;

                case (int)State.Chasing:
                    NPC.frameCounter++;
                    if (NPC.frameCounter == 15)
                        NPC.frame.Y = 4 + 92;
                    else if (NPC.frameCounter == 30)
                        NPC.frame.Y = 6 + 92 * 2;
                    else if (NPC.frameCounter == 45)
                        NPC.frame.Y = 8 + 92 * 3;
                    else if (NPC.frameCounter == 60)
                        NPC.frame.Y = 10 + 92 * 4;
                    else if (NPC.frameCounter == 75)
                        NPC.frame.Y = 12 + 92 * 5;
                    else if (NPC.frameCounter == 90)
                        NPC.frame.Y = 14 + 92 * 6;
                    else if (NPC.frameCounter == 105)
                        NPC.frame.Y = 16 + 92 * 7;
                    else if (NPC.frameCounter == 120)
                        NPC.frame.Y = 18 + 92 * 8;
                    else if (NPC.frameCounter >= 120)
                        NPC.frameCounter = 0;
                    break;
            }
        }
    }
}
