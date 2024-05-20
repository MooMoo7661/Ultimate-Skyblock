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
            if (SubworldSystem.IsActive<MiningSubworld>() || SubworldSystem.IsActive<DungeonSubworld>())
            {
                Player.ZoneSkyHeight = false;
                Player.ZoneBeach = false;
                Main.oceanBG = 0;
            }            
                
            SubworldSystem.hideUnderworld = SubworldSystem.Current == ModContent.GetInstance<MiningSubworld>() && ((Player.position.Y / 16) <= Main.UnderworldLayer - 20);
        }
    }
}