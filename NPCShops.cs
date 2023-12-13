using OneBlock.Items;
using OneBlock.Items.Guidebook;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace OneBlock
{
	public class NPCShops : GlobalNPC
	{
        public override void ModifyShop(NPCShop shop)
        {
            if (shop.NpcType == NPCID.Merchant)
            {
                shop.Add(new Item(ItemID.Silk)
                {
                    shopCustomPrice = Item.buyPrice(0, 0, 12, 10)
                });

                shop.Add(new Item(ModContent.ItemType<GuidebookItem>())
                {
                    shopCustomPrice = Item.buyPrice(0, 0, 15, 0)
                });
            }
        }
    }
}