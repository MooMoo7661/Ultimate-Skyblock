using OneBlock.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace OneBlock.Notifications
{
    public class NotifModPlayer : ModPlayer
    {
        public override void OnEnterWorld()
        {
            var config = ModContent.GetInstance<NotificationsConfig>();

            if (!config.EnabledNotifs) { return; }

            if (Player.whoAmI == Main.myPlayer) { InGameNotificationsTracker.AddNotification(new JoinWorldNotif()); }
        }
    }
}

