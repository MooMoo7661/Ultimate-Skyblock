﻿using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using UltimateSkyblock.Content.Tiles.Extractinators;

namespace UltimateSkyblock.Content.Items.Placeable.Objects
{
    public class AutoExtractorTier5 : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;

            Item.placeStyle = 0;
            Item.consumable = true;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.maxStack = Item.CommonMaxStack;
            Item.createTile = ModContent.TileType<AutoExtractorTier5Tile>();
            Item.rare = ItemRarityID.Red;
            Item.useTime = 10;
            Item.useTurn = true;
            Item.useAnimation = 15;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<AutoExtractorTier4>())
                .AddIngredient(ItemID.LunarBar, 8)
                .AddIngredient(ItemID.SoulofLight, 25)
                .AddIngredient(ItemID.Diamond, 15)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
