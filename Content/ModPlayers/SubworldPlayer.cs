using Microsoft.Xna.Framework;
using SubworldLibrary;
using UltimateSkyblock.Content.Subworlds;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Microsoft.Xna.Framework.Graphics;

namespace UltimateSkyblock.Content.ModPlayers
{
    public class SubworldPlayer : ModPlayer
	{
        public override void PreUpdate()
        {
            if (SubworldSystem.AnyActive())
            {
                Player.ZoneSkyHeight = false;
                Player.ZoneBeach = false;
                Main.oceanBG = 0;
            }            
             
            // Commented code below is flashlight functionality
            //if (Player.HeldItem.type == ModContent.ItemType<Content.Items.Placeable.Glimmercap>() && !Framing.GetTileSafely(Main.MouseWorld.ToTileCoordinates().X, Main.MouseWorld.ToTileCoordinates().Y).HasTile)
            //{
            //    Lighting.AddLight(Main.MouseWorld, 6, 6, 6);
            //}
        }

        //public override void PostUpdate()
        //{
        //    if (Player.HeldItem.type == ModContent.ItemType<Content.Items.Placeable.Glimmercap>() && !Framing.GetTileSafely(Main.MouseWorld.ToTileCoordinates().X, Main.MouseWorld.ToTileCoordinates().Y).HasTile)
        //    {
        //        Lighting.AddLight(Main.MouseWorld, 6, 6, 6);
        //    }
        //}

        public override void OnEnterWorld()
        {
            if (SubworldSystem.AnyActive())
            {
                Main.NewText("This world is seperate from the main world.\nThis means bosses defeated in here will not sync with the main world, and bosses defeated in the main world will not sync in here.");
            }
        }
    }
}