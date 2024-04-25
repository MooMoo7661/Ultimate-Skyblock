using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using UltimateSkyblock.Content.ModSystems;
using UltimateSkyblock.Content.Tiles.Extractinators;
using UltimateSkyblock.Content.Tiles.Furniture;
using Terraria.ObjectData;
using Terraria.Enums;

namespace UltimateSkyblock.Content.Items.Placeable
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
            Item.createTile = ModContent.TileType<SunshinePainting>();
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

    public class GlobalNullTile : GlobalTile
    {
        public override void SetStaticDefaults()
        {
            if (ModLoader.TryGetMod("StructureHelper", out Mod structureHelper))
            {
                if (structureHelper.TryFind<ModTile>("NullBlock", out ModTile nullTile))
                {
                    Main.tileSolid[nullTile.Type] = false;
                    Main.tileBlockLight[nullTile.Type] = false;
                    Main.tileNoSunLight[nullTile.Type] = false;

                    TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
                    TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.None, count:0, start: 0);
                    TileObjectData.addTile(nullTile.Type);
                }
            }
        }

        public override bool CreateDust(int i, int j, int type, ref int dustType)
        {
            if (ModLoader.TryGetMod("StructureHelper", out Mod structureHelper))
            {
                if (structureHelper.TryFind<ModTile>("NullBlock", out _))
                {
                    return false;
                }
            }

            return base.CreateDust(i , j, type, ref dustType);
        }
    }
}
