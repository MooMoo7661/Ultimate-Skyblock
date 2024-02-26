using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using UltimateSkyblock.Content.Buffs;
using UltimateSkyblock.Content.Items.Placeable;

namespace UltimateSkyblock.Content.Items.Generic
{
    public class GlimmerTonic : ModItem
    {

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 30;

            ItemID.Sets.DrinkParticleColors[Item.type] = new Color[3] {
                new Color(224, 116, 214),
                new Color(121, 84, 229),
                new Color(0, 30, 156)
            };
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 30;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item3;
            Item.maxStack = 30;
            Item.consumable = true;
            Item.rare = ItemRarityID.Orange;
            Item.value = Terraria.Item.buyPrice(gold: 1);

            Item.buffType = ModContent.BuffType<GlimmerBuff>();
            Item.buffTime = 8 * 60 * 60; // too lazy for math
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Glimmercap>())
                .AddIngredient(ItemID.BottledWater)
                .AddTile(TileID.Bottles)
                .Register();
        }
    }
}
