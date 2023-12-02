using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.Config;

namespace OneBlock.Configs
{
    public class NotificationsConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Label("All Configs Enabled")]
        [DefaultValue(true)]
        public bool EnabledNotifs { get; set; }
    }
}
