using OneBlock.UI.GuideBook;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader.IO;
using Terraria.ModLoader;
using Terraria;

namespace OneBlock.Items.Guidebook
{
    public class GuidebookItem : ModItem
    {

        public int Choice;
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ManaCrystal);
            Item.consumable = false;
            Item.ResearchUnlockCount = 3;
            Item.maxStack = 1;
            Item.value = 0;
            Item.rare = ItemRarityID.Cyan;
            Item.UseSound = null;
        }
        public override void SaveData(TagCompound tag)
        {
            tag["SkyChoices"] = Choice;
        }
        public override void LoadData(TagCompound tag)
        {
            Choice = tag.Get<int>("SkyChoices");
        }
        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(Choice);
        }
        public override void NetReceive(BinaryReader reader)
        {
            Choice = reader.ReadInt32();
        }
        // If the player using the item is the client
        // (explicitly excluded serverside here)
        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                if (ModContent.GetInstance<SkyUISystem>().IsUIOpen())
                {
                    SoundEngine.PlaySound(SoundID.MenuClose);
                    ModContent.GetInstance<SkyUISystem>().HideMyUI();
                }
                else
                {
                    SoundEngine.PlaySound(SoundID.MenuOpen);
                    ModContent.GetInstance<SkyUISystem>().ShowMyUI(Choice);
                }

            }

            return true;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (Choice == 0)
            {
                //tooltips.RemoveAt(2);
            }
        }
    }
    //The book is given at the start of the game and otherwise unaquireable
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
