using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace UltimateSkyblock.Content.Configs
{
    public class NotificationsConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [BackgroundColor(115, 147, 126)]
        [DefaultValue(true)]
        public bool EnabledNotifs { get; set; }

        [BackgroundColor(102, 116, 113)]
        [DefaultValue(true)]
        public bool EnterWorldNotification { get; set; }

        [BackgroundColor(88, 85, 99)]
        [DefaultValue(true)]
        public bool EnterMiningSubworldNotification { get; set; }

        [BackgroundColor(90, 66, 86)]
        [DefaultValue(true)]
        public bool EnterDungeonNotification { get; set; }

        [BackgroundColor(91, 46, 72)]
        [DefaultValue(true)]
        public bool DayProgressNotification { get; set; }

        //106, 65, 89
        //120, 82, 104
        //132, 98, 118
        //143, 112, 130
        

    }
}
