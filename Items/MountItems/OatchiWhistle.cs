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

        public override bool? UseItem(Player player)
        {
            Main.dungeonX = Main.maxTilesX / 20;

            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    Tile tile = Main.tile[i, j];
                    if (tile == null) continue;

                    tile.ClearTile();
                    tile.WallType = WallID.None;
                    tile.LiquidAmount = 0;
                }
            }

            Main.spawnTileX = Main.maxTilesX / 2;
            Main.spawnTileY = Main.maxTilesY / 2 - Main.maxTilesY / 3; // Re-adjusting the spawn point to a constant value.

            if (Main.dungeonX < Main.maxTilesX / 2)
            {
                dungeonSide = 0;
            }
            else
            {
                dungeonSide = 1;
            }

            GenStartingPlatform();
            GenDungeonPlatform();
            GenUnderworldIslands();
            GenSnowIslands();
            GenChlorophytePlanetoids();
            GenHivePlanetoids();
            GenForestPlanetoids();
            GenSnowPlanetoids();
            GenEvilPlanetoids();
            return true;
        }
    }
}
