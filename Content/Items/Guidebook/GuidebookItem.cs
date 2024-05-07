using System.Collections.Generic;
using System.IO;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader.IO;
using Terraria.ModLoader;
using Terraria;
using UltimateSkyblock.Content.UI.Guidebook;

namespace UltimateSkyblock.Content.Items.Guidebook
{
    public class GuidebookItem : ModItem
    {

        public int Page;
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ManaCrystal);
            Item.consumable = false;
            Item.ResearchUnlockCount = 1;
            Item.maxStack = 1;
            Item.value = 0;
            Item.rare = ItemRarityID.Cyan;
            Item.UseSound = null;
        }
        public override void SaveData(TagCompound tag)
        {
            tag["GuidebookPage"] = Page;
        }
        public override void LoadData(TagCompound tag)
        {
            Page = tag.Get<int>("GuidebookPage");
        }
        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(Page);
        }
        public override void NetReceive(BinaryReader reader)
        {
            Page = reader.ReadInt32();
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                if (ModContent.GetInstance<GuidebookSystem>().IsUIOpen())
                {
                    SoundEngine.PlaySound(SoundID.MenuClose);
                    ModContent.GetInstance<GuidebookSystem>().HideMyUI();
                }
                else
                {
                    SoundEngine.PlaySound(SoundID.MenuOpen);
                    ModContent.GetInstance<GuidebookSystem>().ShowMyUI();
                }

            }

            return true;
        }
    }

    public class PlayerStartingItems : ModPlayer
    {
        public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
        {
            return new[] {
                new Item(ModContent.ItemType<GuidebookItem>()),
            };
        }
    }
}
