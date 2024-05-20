using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltimateSkyblock.Content.Tiles.Furniture;

namespace UltimateSkyblock.Content.Items.Placeable.Objects
{
    public class SunshinePaintingItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<SunshinePainting>());
            Item.placeStyle = 0; // Use this to place the chest in its locked style
            Item.width = 34;
            Item.height = 52;
            Item.value = Item.buyPrice(0, 0, 32, 48);
        }
    }
}
