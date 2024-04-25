using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UltimateSkyblock.Content.Utils;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.UI;
using Terraria.Audio;
using UltimateSkyblock.Content.SkyblockWorldGen;

namespace UltimateSkyblock.Notifications
{
    public abstract class BaseNotification : IInGameNotification
    {
        protected abstract Asset<Texture2D> iconTexture { get; }
        protected abstract bool ShouldDissapearOnClick { get; }
        protected virtual SoundStyle? NotifSound { get => null; }
        protected virtual Color BackgroundColor { get => new Color(64, 109, 164); }

        /// <summary> Localization key for the notification name.</summary>
        protected abstract string Title { get; }
        public bool ShouldBeRemoved => timeLeft <= 0;
        private int timeLeft = 300;

        private bool locked = false;

        private float Scale
        {
            get
            {
                if (timeLeft < 30)
                {
                    return MathHelper.Lerp(0f, 1f, timeLeft / 30f);
                }

                if (timeLeft > 285)
                {
                    return MathHelper.Lerp(1f, 0f, (timeLeft - 285) / 15f);
                }

                return 1f;
            }
        }

        private float Opacity
        {
            get
            {
                if (Scale <= 0.5f)
                {
                    return 0f;
                }

                return (Scale - 0.5f) / 0.5f;
            }
        }

        public void OnCreation()
        {
            if (NotifSound != null)
            SoundEngine.PlaySound(NotifSound);
        }

        public void Update()
        {
            if (!locked)
            {
                OnCreation();
                locked = true;
            }

            timeLeft--;

            if (timeLeft < 0)
            {
                timeLeft = 0;
            }
        }

        public void DrawInGame(SpriteBatch spriteBatch, Vector2 bottomAnchorPosition)
        {
            if (Opacity <= 0f)
            {
                return;
            }

            float effectiveScale = Scale * 1.1f;
            Vector2 size = (FontAssets.ItemStack.Value.MeasureString(Title) + new Vector2(58f, 10f)) * effectiveScale;
            Rectangle panelSize = Utils.CenteredRectangle(bottomAnchorPosition + new Vector2(0f, (0f - size.Y) * 0.5f), size);

            bool hovering = panelSize.Contains(Main.MouseScreen.ToPoint());

            Utils.DrawInvBG(spriteBatch, panelSize, BackgroundColor * (hovering ? 0.75f : 0.5f));
            float iconScale = effectiveScale * 0.7f;
            Vector2 vector = panelSize.Right() - Vector2.UnitX * effectiveScale * (12f + iconScale * iconTexture.Width());
            spriteBatch.Draw(iconTexture.Value, vector, null, Color.White * Opacity, 0f, new Vector2(0f, iconTexture.Width() / 2f), iconScale, SpriteEffects.None, 0f);
            Utils.DrawBorderString(color: new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor / 5, Main.mouseTextColor) * Opacity, sb: spriteBatch, text: Title, pos: vector - Vector2.UnitX * 10f, scale: effectiveScale * 0.9f, anchorx: 1f, anchory: 0.4f);

            if (hovering)
            {
                if (!PlayerInput.IgnoreMouseInterface && (Main.mouseLeft || Main.mouseLeftRelease))
                {
                    OnMouseOver();
                }
            }
        }

        public virtual void OnClick()
        {
            Main.LocalPlayer.mouseInterface = true;
            Main.mouseLeftRelease = false;

            if (ShouldDissapearOnClick && timeLeft > 30)
                timeLeft = 30;
        }

        private void OnMouseOver()
        {
            if (timeLeft > 30)
            {
                timeLeft = 30;
            }
        }

        public void PushAnchor(ref Vector2 positionAnchorBottom) => positionAnchorBottom.Y -= 50f * Opacity;

    }
}
