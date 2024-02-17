using UltimateSkyblock.Content.Configs;
using UltimateSkyblock.Notifications;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using SubworldLibrary;

namespace UltimateSkyblock.Content.Notifications
{
    public class NotifModPlayer : ModPlayer
    {
        public override void OnEnterWorld()
        {
            var config = ModContent.GetInstance<NotificationsConfig>();
            

            if (!config.EnabledNotifs || SubworldSystem.Current != null) { return; }

            if (Player.whoAmI == Main.myPlayer) { InGameNotificationsTracker.AddNotification(new JoinWorldNotif()); }
        }
    }
}

