using OneBlock.Items;
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
        //public static readonly List<int> items = new List<int>
        //{
        //    ItemID.DirtBlock,
        //    ItemID.StoneBlock,
        //    ItemID.Cobweb,
        //    ItemID.SandBlock,
        //    ItemID.SlushBlock,
        //    ItemID.SiltBlock,
        //    ItemID.ClayBlock,
        //    ItemID.DesertFossil,
        //    ItemID.SnowBlock,
        //    ItemID.IceBlock,
        //    ItemID.Sandstone,
        //    ItemID.Cloud,
        //    ItemID.RainCloud,
        //    ItemID.MudBlock,
        //    ItemID.LivingFireBlock,
        //    ItemID.GraniteBlock,
        //    ItemID.MarbleBlock,
        //    ItemID.SnowCloudBlock,
        //    ItemID.Glass,
        //    ItemID.BlueBrick,
        //    ItemID.GreenBrick,
        //    ItemID.PinkBrick,
        //    ItemID.BambooBlock,
        //    ItemID.DartTrap,

        //};

        public override void ModifyShop(NPCShop shop)
        {
            if (shop.NpcType == NPCID.Merchant)
            {
                shop.Add(new Item(ItemID.Silk)
                {
                    shopCustomPrice = Item.buyPrice(0, 0, 12, 10)
                });
            }
        }
    }
}