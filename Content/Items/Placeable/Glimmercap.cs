using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using UltimateSkyblock.Content.Tiles.Blocks;
using UltimateSkyblock.Content.Tiles.Environment.Foliage;
using UltimateSkyblock.Content.Buffs;
using Microsoft.Xna.Framework;

namespace UltimateSkyblock.Content.Items.Placeable
{
    public class Glimmercap : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 50;

            ItemID.Sets.FoodParticleColors[Item.type] = new Color[3] {
                new Color(224, 116, 214),
                new Color(121, 84, 229),
                new Color(0, 30, 156)
            };
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 24;
            Item.consumable = true;
            Item.autoReuse = false;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.maxStack = Item.CommonMaxStack;
            Item.rare = ItemRarityID.Blue;
            Item.buffType = ModContent.BuffType<GlimmerBuff>();
            Item.useTime = 10;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.UseSound = SoundID.Item2;
            ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.Mushroom;
        }

        public override void OnConsumeItem(Player player)
        {
            player.AddBuff(ModContent.BuffType<GlimmerBuff>(), 60 * 15 + 1);
        }
    }
}
