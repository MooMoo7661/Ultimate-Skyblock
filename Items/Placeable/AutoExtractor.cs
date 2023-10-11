using OneBlock.SkyblockWorldGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using OneBlock.Tiles.Blocks;
using OneBlock.Tiles.Extractinators;

namespace OneBlock.Items.Placeable
{
    public class AutoExtractor : ModItem
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
            Item.createTile = ModContent.TileType<AutoExtractorTier1Tile>();
            Item.rare = ItemRarityID.Green;
            Item.useTime = 10;
            Item.useTurn = true;
            Item.useAnimation = 15;
            ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.Extractinator;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Extractinator)
                .AddRecipeGroup(Recipes.evilBars, 5)
                .AddIngredient(ItemID.Diamond, 3)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
