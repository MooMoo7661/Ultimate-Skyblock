using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Humanizer;
using Terraria.Audio;
using UltimateSkyblock.Content.DaySystem;
using UltimateSkyblock.Notifications;

namespace UltimateSkyblock.Content.Notifications
{
    public class JoinWorldNotif : BaseNotification
    {
        protected override Asset<Texture2D> iconTexture => TextureAssets.Item[ItemID.DirtBlock];
        protected override bool ShouldDissapearOnClick => true;
        protected override string Title => Language.GetTextValue("Mods.UltimateSkyblock.Notifications.JoinWorldNotif");
    }

    public class DayProgressNotification : BaseNotification
    {
        protected override Asset<Texture2D> iconTexture => ModContent.Request<Texture2D>("UltimateSkyblock/Assets/Textures/Sun");
        protected override SoundStyle? NotifSound => SoundID.Item35;
        protected override bool ShouldDissapearOnClick => true;
        protected override string Title
        {
            get
            {
                return Language.GetTextValue("Mods.UltimateSkyblock.Notifications.DayProgressNotification").FormatWith(DayCounter.Day);
            }
        }
    }
}
