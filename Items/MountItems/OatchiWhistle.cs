using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using OneBlock.Mounts;

using static OneBlock.SkyblockWorldGen.MainWorld;

namespace OneBlock.Items.MountItems
{
    public class OatchiWhistle : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 26;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.HoldUp; // how the player's arm moves when using the item
            Item.value = Item.buyPrice(0, 50, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item128;
            Item.noMelee = true;
            //Item.mountType = ModContent.MountType<Oatchi>();
        }
    }
}
