using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace UltimateSkyblock.Content.Configs
{
    public class NotificationsConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [DefaultValue(true)]
        public bool EnabledNotifs { get; set; }

        [DefaultValue(true)]
        public bool EnterWorldNotification { get; set; }

        [DefaultValue(true)]
        public bool EnterMiningSubworldNotification { get; set; }

        [DefaultValue(true)]
        public bool EnterDungeonNotification { get; set; }

        [DefaultValue(true)]
        public bool DayProgressNotification { get; set; }

    }
}
