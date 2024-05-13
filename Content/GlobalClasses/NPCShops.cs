using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UltimateSkyblock.Content.Items.Generic;
using UltimateSkyblock.Content.Items.Placeable.Tiles;
using UltimateSkyblock.Content.ModPlayers;
using UltimateSkyblock.Content.Utils;

namespace UltimateSkyblock.Content.GlobalClasses
{
    public class NPCShops : GlobalNPC
    {
        public override void ModifyShop(NPCShop shop)
        {
            if (shop.NpcType == NPCID.Merchant)
            {
                shop.AddWithValue(ItemID.Silk, Item.buyPrice(0, 0, 12, 10));

                shop.AddWithValue(ModContent.ItemType<GuidebookItem>(), Item.buyPrice(0, 0, 15, 0));

                shop.AddWithValue(ModContent.ItemType<MiningLantern>(), Item.buyPrice(0, 0, 35, 0), Condition.PlayerCarriesItem(ModContent.ItemType<MiningLantern>()));
            }
        }
    }
}