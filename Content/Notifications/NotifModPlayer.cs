using UltimateSkyblock.Content.Configs;
using UltimateSkyblock.Notifications;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using SubworldLibrary;
using UltimateSkyblock.Content.Subworlds;
using Terraria.ModLoader.IO;

namespace UltimateSkyblock.Content.Notifications
{
    public class NotifModPlayer : ModPlayer
    {
        public override void OnEnterWorld()
        {
            var config = ModContent.GetInstance<NotificationsConfig>();
            if (!config.EnabledNotifs || !UltimateSkyblock.IsSkyblock()) { return; }

            if (Player.whoAmI == Main.myPlayer)
            {
                if (SubworldSystem.Current == null && config.EnterWorldNotification) { InGameNotificationsTracker.AddNotification(new JoinWorldNotif()); }
                if (SubworldSystem.Current == ModContent.GetInstance<MiningSubworld>() && config.EnterMiningSubworldNotification) { InGameNotificationsTracker.AddNotification(new MiningSubworldEnterNotification()); }
                if (SubworldSystem.Current == ModContent.GetInstance<DungeonSubworld>() && config.EnterMiningSubworldNotification) { InGameNotificationsTracker.AddNotification(new DungeonSubworldEnterNotification()); }
            }
        }
    }
}

