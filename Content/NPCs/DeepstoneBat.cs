using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using UltimateSkyblock.Content.Biomes;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.Bestiary;

namespace UltimateSkyblock.Content.NPCs
{
    public class DeepstoneBat : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.CaveBat];

            NPCID.Sets.ShimmerTransformToNPC[NPC.type] = NPCID.CaveBat;

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 1f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

        public override void SetDefaults()
        {

            NPC.CloneDefaults(NPCID.CaveBat);
            NPC.damage = 59;

            AIType = NPCID.CaveBat;
            AnimationType = NPCID.CaveBat;
            Banner = Item.NPCtoBanner(NPCID.CaveBat);
            BannerItem = Item.BannerToItem(Banner);
            SpawnModBiomes = new int[1] { ModContent.GetInstance<DeepstoneBiome>().Type };
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            var zombieDropRules = Main.ItemDropsDB.GetRulesForNPCID(NPCID.CaveBat, false);
            foreach (var zombieDropRule in zombieDropRules)
            {
                npcLoot.Add(zombieDropRule);
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

				new FlavorTextBestiaryInfoElement("A much more dangerous type of cave bat."),

				new BestiaryPortraitBackgroundProviderPreferenceInfoElement(ModContent.GetInstance<DeepstoneBiome>().ModBiomeBestiaryInfoElement),
        });
        }
    }
}

