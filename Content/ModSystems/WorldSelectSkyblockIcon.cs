using UltimateSkyblock.Content.StoneGenerator;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.GameContent.UI.States;
using System.Reflection;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using System;
using Terraria.ModLoader.UI;
using Terraria.GameContent.UI.Elements;
using UltimateSkyblock.Content.Configs;
using UltimateSkyblock.Content.UI.Guidebook;

namespace UltimateSkyblock.Content.ModSystems
{
    public class WorldSelectSkyblockIcon : ModSystem
    {
        public static bool Skyblock
        {
            get
            {
                return Main.ActiveWorldFileData.WorldGenModsRecorded &&
                    Main.ActiveWorldFileData.TryGetModVersionGeneratedWith("UltimateSkyblock", out _);
            }
        }

        public override void SaveWorldHeader(TagCompound tag)
        {
            tag["GeneratedWithSkyblock"] = Skyblock;
        }

        public override void Load()
        {
            if (ModContent.GetInstance<MainClientConfig>().ToggleWorldSelectDetour)
            {
                On_UIWorldListItem.DrawSelf += (orig, self, spriteBatch) =>
                {
                    DrawWorldSelectItemOverlay(self, spriteBatch);
                    orig(self, spriteBatch);
                    DrawWorldSelectItemOverlay(self, spriteBatch);
                };
            }
        }

        private void DrawWorldSelectItemOverlay(UIWorldListItem uiItem, SpriteBatch spriteBatch)
        {
            if (!uiItem.Data.TryGetHeaderData(this, out var data) || !data.GetBool("GeneratedWithSkyblock"))
                return;

            var config = ModContent.GetInstance<MainClientConfig>();

            //UIElement WorldIcon = (UIElement)typeof(UIWorldListItem).GetField("_worldIcon", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(uiItem);
            //UIImage element = new UIImage(ModContent.Request<Texture2D>("UltimateSkyblock/Assets/WorldIcons/Skyblock"))
            //{
            //    Top = new StyleDimension(0f, 0f),
            //    Left = new StyleDimension(1f, 0f),
            //    IgnoresMouseInteraction = true
            //};
            //WorldIcon.Append(element);

            if (config.WorldBackgroundColorLerp)
            uiItem.BackgroundColor = uiItem.IsMouseHovering ? UICommon.DefaultUIBlueMouseOver : Color.Lerp(UICommon.DefaultUIBlueMouseOver, new(15, 26, 68, UICommon.MainPanelBackground.A), (MathF.Sin(Main.GlobalTimeWrappedHourly * 2f) + 1) / 2f);
            if (config.WorldBorderColor)
                uiItem.BorderColor = UICommon.DefaultUIBorder;


            if (config.WorldSelectTagIdentifier)
            {
                var dims = uiItem.GetInnerDimensions();
                var pos = new Vector2(dims.X + 500, dims.Y);
                Color color = Color.Lerp(Color.Bisque, Color.BurlyWood, (MathF.Sin(Main.GlobalTimeWrappedHourly * 1.3f) + 1) / 2f);
                Terraria.Utils.DrawBorderString(spriteBatch, "Skyblock", pos, color);
            }
        }
    }
}
