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
using UltimateSkyblock.Content.Items.Placeable;
using UltimateSkyblock.Content.Items.Placeable.Objects;

namespace UltimateSkyblock.Content.NPCs
{
    public class Deepknight : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.BlueArmoredBonesSword];

            NPCID.Sets.ShimmerTransformToNPC[NPC.type] = NPCID.Skeleton;

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 1f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

        public override void SetDefaults()
        {

            NPC.CloneDefaults(NPCID.BlueArmoredBonesSword);
            NPC.damage = 25;
            NPC.defense = 8;
            NPC.lifeMax = 80;

            AIType = NPCID.BlueArmoredBonesSword;
            AnimationType = NPCID.BlueArmoredBonesSword;
            Banner = Item.NPCtoBanner(NPCID.Skeleton);
            BannerItem = Item.BannerToItem(Banner);
            SpawnModBiomes = new int[1] { ModContent.GetInstance<DeepstoneBiome>().Type };
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            var skeletonDropRules = Main.ItemDropsDB.GetRulesForNPCID(NPCID.Skeleton, false);
            foreach (var skeletonDropRule in skeletonDropRules)
            {
                npcLoot.Add(skeletonDropRule);
            }

            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<DeepstoneChestKey>(), 10));
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

				new FlavorTextBestiaryInfoElement(""),

				new BestiaryPortraitBackgroundProviderPreferenceInfoElement(ModContent.GetInstance<DeepstoneBiome>().ModBiomeBestiaryInfoElement),
        });
        }
    }
}

