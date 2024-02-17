using Microsoft.Xna.Framework;
using SubworldLibrary;
using UltimateSkyblock.Content.Subworlds;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace UltimateSkyblock.Content.ModPlayers
{
    public class SubworldPlayer : ModPlayer
	{
        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            SubworldSystem.Enter<MiningSubworld>();
        }

        public override void PostUpdate()
        {
            if (SubworldSystem.IsActive<MiningSubworld>() || SubworldSystem.IsActive<PlanteraSubworld>())
            {
                Player.ZoneSkyHeight = false;
                Player.ZoneBeach = false;
            }
        }

        public override void PreUpdate()
        {
            if (SubworldSystem.IsActive<MiningSubworld>() || SubworldSystem.IsActive<PlanteraSubworld>())
            {
                Player.ZoneSkyHeight = false;
                Player.ZoneBeach = false;
                Main.oceanBG = 0;
            }
        }
    }
}