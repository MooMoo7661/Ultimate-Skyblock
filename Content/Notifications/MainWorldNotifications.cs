using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using UltimateSkyblock.Notifications;

namespace UltimateSkyblock.Content.Notifications
{
    public class JoinWorldNotif : BaseNotification
    {
        protected override Asset<Texture2D> iconTexture => TextureAssets.Item[ItemID.DirtBlock];
        protected override bool ShouldDissapearOnClick => true;
        protected override string Title => Language.GetTextValue("Mods.UltimateSkyblock.Notifications.JoinWorldNotif");
    }
}
