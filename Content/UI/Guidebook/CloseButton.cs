using Microsoft.Xna.Framework.Input;
using Terraria.Audio;

namespace UltimateSkyblock.Content.UI.Guidebook
{
    public class CloseButton : UIImageButton
    {
        private string _hoverText;
        private Asset<Texture2D> _icon;

        public CloseButton(Asset<Texture2D> texture, string hoverText = "") : base(texture)
        {
            _icon = texture;
            _hoverText = hoverText;   
        }

        public override void MouseOver(UIMouseEvent evt)
        {
            if (_hoverText != null)
            {
                Main.hoverItemName = _hoverText;
            }

            base.MouseOver(evt);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Color color;
            if (!IsMouseHovering)
                color = Color.Gray;
            else
                color = Color.White;

            spriteBatch.Draw(_icon.Value, new Vector2(GetDimensions().X, GetDimensions().Y), color);
        }

        public override void LeftClick(UIMouseEvent evt)
        {
            base.LeftClick(evt);
            ModContent.GetInstance<GuidebookSystem>().HideMyUI();
            SoundEngine.PlaySound(SoundID.MenuClose);
        }
    }
}
