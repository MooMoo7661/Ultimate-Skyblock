using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using UltimateSkyblock.Content.UI.Guidebook;
using UltimateSkyblock.Notifications;

namespace UltimateSkyblock.Content.Notifications
{
    public class MiningSubworldEnterNotification : BaseNotification
    {
        protected override Asset<Texture2D> iconTexture => ModContent.Request<Texture2D>("UltimateSkyblock/Content/Biomes/DeepstoneBiome_Icon");
        protected override bool ShouldDissapearOnClick => true;
        protected override string Title => Language.GetTextValue("Mods.UltimateSkyblock.Notifications.MiningSWNotif");
        protected override Color BackgroundColor => Color.DarkSlateGray;
    }
}
