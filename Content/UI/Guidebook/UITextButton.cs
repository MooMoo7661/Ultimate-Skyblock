using Terraria.Audio;

namespace UltimateSkyblock.Content.UI.Guidebook
{
    public class UITextButton : UIPanel
    {
        //isSelected is the value I want to use externally to handle the visual aspect of "locking" the button choice.
        public bool? isSelected;
        internal string buttonText = "";
        internal string hoverText;
        internal UIText text;
        private SoundStyle? clickSound;
        private bool soundedHover;
        public override void OnInitialize()
        {
            if (buttonText != "")
            {
                text = new UIText(buttonText, 1f, false)
                {
                    HAlign = 0.5f,
                    VAlign = 0.35f
                };
                Append(text);
            }
        }
        public UITextButton(string buttonText = "", float alignX = 0.5f, float alignY = 0.5f, float width = 130f, float height = 34f, string hoverText = "", SoundStyle? clickSound = null)
        {
            this.buttonText = buttonText;
            this.hoverText = hoverText;
            this.clickSound = clickSound;
            HAlign = alignX;
            VAlign = alignY;
            Width.Set(width, 0f);
            Height.Set(height, 0f);
            SetPadding(0);
            Activate();
        }
        //On-click sounds.
        public override void LeftClick(UIMouseEvent evt)
        {
            if (clickSound != null && isSelected == null)
            {
                SoundEngine.PlaySound(clickSound);
            }
            base.LeftClick(evt);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            CalculatedStyle dimensions = GetDimensions();
            Color color = Colors.InventoryDefaultColor;

            //Draw a greyscale visual on the button to give it depth and a black border.
            Terraria.Utils.DrawSplicedPanel(spriteBatch, Main.Assets.Request<Texture2D>("Images/UI/CharCreation/PanelGrayscale").Value, (int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width, (int)dimensions.Height, 10, 10, 10, 10, Color.Lerp(Color.Black, color, 0.8f) * 0.7f);

            //A 'blushing' effect in the middle of the button when it's been clicked as an option
            if (isSelected == true)
            {
                Terraria.Utils.DrawSplicedPanel(spriteBatch, Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelHighlight").Value, (int)dimensions.X + 5, (int)dimensions.Y + 5, (int)dimensions.Width - 10, (int)dimensions.Height - 10, 10, 10, 10, 10, Color.Lerp(color, Color.White, 0.7f) * 0.7f);
            }

            if (IsMouseHovering)
            {
                //Hover text and tick sound, the sound requires this bool check or it goes off on the text child element as well
                if (hoverText != "")
                {
                    Main.hoverItemName = hoverText;
                }
                if (!soundedHover && isSelected == null)
                {
                    SoundEngine.PlaySound(SoundID.MenuTick);
                    soundedHover = true;
                }

                //Golden border around the button on hover unless a choice has been made.  A nullabool cleanly handles the 3 possible states.
                if (isSelected == null)
                {
                    Terraria.Utils.DrawSplicedPanel(spriteBatch, Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelBorder").Value, (int)dimensions.X - 1, (int)dimensions.Y - 1, (int)dimensions.Width + 2, (int)dimensions.Height + 2, 10, 10, 10, 10, Color.White);
                }
            }
            else soundedHover = false;
        }
    }
}
