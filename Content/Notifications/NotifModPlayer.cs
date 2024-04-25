using UltimateSkyblock.Content.Configs;
using UltimateSkyblock.Notifications;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using SubworldLibrary;
using UltimateSkyblock.Content.Subworlds;

namespace UltimateSkyblock.Content.Notifications
{
    public class NotifModPlayer : ModPlayer
    {
        public override void OnEnterWorld()
        {
            var config = ModContent.GetInstance<NotificationsConfig>();
            if (!config.EnabledNotifs) { return; }

            if (Player.whoAmI == Main.myPlayer && SubworldSystem.Current == null) { InGameNotificationsTracker.AddNotification(new JoinWorldNotif()); }
            if (Player.whoAmI == Main.myPlayer && SubworldSystem.Current == ModContent.GetInstance<MiningSubworld>()) { InGameNotificationsTracker.AddNotification(new MiningSubworldEnterNotification()); }
        }
    }
}

