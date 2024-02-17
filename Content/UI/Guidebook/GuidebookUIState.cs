using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using UltimateSkyblock.Content.Utils;

namespace UltimateSkyblock.Content.UI.Guidebook
{
    public class GuidebookUIState : UIState
    {
        public DraggableUIPanel GuidebookPanel;

        UIText Title;
        UIText PageNumber;
        UIText PageName;
        UIText MainText;
        UITextButton RightButton;
        UITextButton LeftButton;
        
        public enum PageID
        {
            Main = 0,
            Fishing = 4
        }

        Color DefaultColor = new Color(44, 57, 105, 178); // Default color of standard terraria UI.
        public int PageIndex = 0;
        string MainPage = "";

        /// <summary>
        /// Used for storing pages. Each page is a string. 
        /// Calculation for whether the book can be scrolled is done automatically.
        /// </summary>
        public static readonly Dictionary<int, string> Pages = new Dictionary<int, string>();
        public static readonly Dictionary<int, string> PageNames = new Dictionary<int, string>();

        /// <summary>
        /// Used for determining what entry to get with TryGetEntry
        /// </summary>
        public enum StyleID
        {
            Page,
            Name
        }

        /// <summary>
        /// Used to get either the current page name, or the text to display
        /// </summary>
        /// <param name="index">Index to search the dictionary for. Intended to have PageIndex passed in.</param>
        /// <returns>Dictionary entry for the provided key, or null if not found</returns>
        public static string TryGetEntry(int index, StyleID style)
        {
            switch (style)
            {
                case StyleID.Page:
                    if (Pages.TryGetValue(index, out string page)) { return page; }
                    break;

                case StyleID.Name:
                    if (PageNames.TryGetValue(index, out string name)) { return name; }
                    break;
            }

            return null;
        }

        /// <summary>
        /// Adding the pages to the dictionary
        /// </summary>
        public static void AddPages()
        {
            string path = "Mods.UltimateSkyblock.LocalizedPages.";
            Pages.Add(0, Language.GetTextValue(path + "IntroPage")); PageNames.Add(0, Language.GetTextValue(path + "PageNames.Intro"));
            Pages.Add(1, Language.GetTextValue(path + "TableOfContents")); PageNames.Add(1, Language.GetTextValue(path + "PageNames.TableOfContents"));
            Pages.Add(2, Language.GetTextValue(path + "Progression1")); PageNames.Add(2, Language.GetTextValue(path + "PageNames.Progression"));
            Pages.Add(3, Language.GetTextValue(path + "Progression2")); PageNames.Add(3, Language.GetTextValue(path + "PageNames.Progression"));
            Pages.Add(4, Language.GetTextValue(path + "Progression3")); PageNames.Add(4, Language.GetTextValue(path + "PageNames.Progression"));
            Pages.Add(5, Language.GetTextValue(path + "Fishing")); PageNames.Add(5, Language.GetTextValue(path + "PageNames.Fishing"));
        }

        /// <summary>
        /// Once created. Used to create the UI for the book, and assign sizes, colors, etc.
        /// </summary>
        public override void OnInitialize()
        {
            AddPages();

            string title = "Skyblock Guidebook";

            GuidebookPanel = new DraggableUIPanel();
            GuidebookPanel.SetPadding(0);
            GuidebookPanel.HAlign = 0.5f;
            GuidebookPanel.VAlign = 0.1f;
            GuidebookPanel.Width.Set(1000f, 0f);
            GuidebookPanel.Height.Set(500f, 0f);
            GuidebookPanel.BackgroundColor = new Color(73, 94, 171);
            Append(GuidebookPanel);

            Title = new UIText(title);
            Title.HAlign = 0.5f;
            Title.MarginTop = 15;
            GuidebookPanel.Append(Title);

            UIText QuickPages = new UIText("Select", 0.9f);
            QuickPages.MarginLeft = 15;
            QuickPages.MarginTop = 30;
            GuidebookPanel.Append(QuickPages);

            PageNumber = new UIText("- " + (PageIndex + 1).ToString() + " -");
            PageNumber.HAlign = 0.5f;
            PageNumber.MarginTop = 450;
            GuidebookPanel.Append(PageNumber);

            PageName = new UIText("Introduction");
            PageName.HAlign = 0.5f;
            PageName.MarginTop = 450;
            GuidebookPanel.Append(PageName);

            MainPage = Language.GetTextValue("Mods.UltimateSkyblock.LocalizedPages.IntroPage");
            MainText = new UIText(MainPage, 0.95f);
            MainText.MarginLeft = 75;
            MainText.MarginTop = 50;
            GuidebookPanel.Append(MainText);

            InitializeButtons(); // General buttons, like left and right arrows, & close
            InitializeQuickPages(); // Quick page select buttons
        }

        private void InitializeQuickPages()
        {
            UITextButton mainButton = new UITextButton("", 0, 0.05f, 40, 40, "Main", SoundID.MenuClose);
            mainButton.OnLeftClick += new MouseEvent(HomeClicked);
            mainButton.MarginLeft = 15;
            mainButton.MarginTop = 45;
            GuidebookPanel.Append(mainButton);

            Asset<Texture2D> home = ModContent.Request<Texture2D>("UltimateSkyblock/Content/UI/Guidebook/Assets/MainPage", AssetRequestMode.ImmediateLoad);
            UIImage mainIcon = new UIImage(home);
            mainIcon.HAlign = 0.5f;
            mainIcon.VAlign = 0.5f;
            mainIcon.ScaleToFit = true;
            mainButton.Append(mainIcon);

            UITextButton fishingButton = new UITextButton("", 0, 0.05f, 40, 40, "Fishing", SoundID.MenuClose);
            fishingButton.OnLeftClick += new MouseEvent(FishingClicked);
            fishingButton.MarginLeft = 15;
            fishingButton.MarginTop = 90;
            GuidebookPanel.Append(fishingButton);

            Main.instance.LoadItem(ItemID.Goldfish);
            Asset<Texture2D> fish = TextureAssets.Item[ItemID.Goldfish];
            UIImage quickPage = new UIImage(fish);
            quickPage.HAlign = 0.5f;
            quickPage.VAlign = 0.5f;
            quickPage.ScaleToFit = true;
            fishingButton.Append(quickPage);
        }

        private void InitializeButtons()
        {
            UITextButton close = new UITextButton("X", 0.975f, 0.025f, 40, 30, Language.GetTextValue("LegacyInterface.52"), SoundID.MenuClose);
            close.OnLeftClick += new MouseEvent(CloseClicked);
            GuidebookPanel.Append(close);

            LeftButton = new UITextButton("<", 0.05f, 0.95f, 130, 34, "", SoundID.MenuTick);
            LeftButton.OnLeftClick += new MouseEvent(PageAdvancementLeft);
            GuidebookPanel.Append(LeftButton);

            RightButton = new UITextButton(">", 0.95f, 0.95f, 130, 34, "", SoundID.MenuTick);
            RightButton.OnLeftClick += new MouseEvent(PageAdvancementRight);
            GuidebookPanel.Append(RightButton);
        }

        private void CloseClicked(UIMouseEvent evt, UIElement listeningElement) => ModContent.GetInstance<GuidebookSystem>().HideMyUI();
        private void HomeClicked(UIMouseEvent evt, UIElement listeningElement) => PageIndex = (int)PageID.Main;
        private void FishingClicked(UIMouseEvent evt, UIElement listeningElement) => PageIndex = (int)PageID.Fishing;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            SafeguardIndex();
            UpdatePage();
        }

        private void SafeguardIndex() { if (PageIndex < 0) PageIndex = 0; }

        /// <summary>
        /// Handles updating the page. Updates text, title, page number, and buttons colors.
        /// </summary>
        private void UpdatePage()
        {
            string text = TryGetEntry(PageIndex, StyleID.Page);
            if (text != null)
            {
                GuidebookPanel.RemoveChild(MainText);
                MainText = new UIText(text, 1f);
                MainText.MarginLeft = 75;
                MainText.MarginTop = 50;
                GuidebookPanel.Append(MainText);
            }

            string pageName = TryGetEntry(PageIndex, StyleID.Name);
            if (pageName != null)
            {
                Color color = Color.Lerp(Color.AliceBlue, Color.MediumPurple, (MathF.Sin(Main.GlobalTimeWrappedHourly * 2.9f) + 1) / 2f);

                GuidebookPanel.RemoveChild(PageName);
                PageName = new UIText(pageName.ToHexString(color), 1f);
                PageName.HAlign = 0.5f;
                PageName.MarginTop = 420;
                GuidebookPanel.Append(PageName);
            }

            GuidebookPanel.RemoveChild(PageNumber);
            PageNumber = new UIText("- " + (PageIndex + 1) + " -", 1f);
            PageNumber.HAlign = 0.5f;
            PageNumber.MarginTop = 450;
            GuidebookPanel.Append(PageNumber);

            string page = TryGetEntry(PageIndex - 1, StyleID.Page);
            LeftButton.BackgroundColor = page == null ? Color.Gray : DefaultColor;

            page = TryGetEntry(PageIndex + 1, StyleID.Page);
            RightButton.BackgroundColor = page == null ? Color.Gray : DefaultColor;
        }

        /// <summary>
        /// Increases PageIndex by 1, but only if the current index + 1 is found in the dictionary. Also sets MainText to the index page.
        /// </summary>
        private void PageAdvancementRight(UIMouseEvent evt, UIElement listeningElement)
        {
            string page = TryGetEntry(PageIndex + 1, StyleID.Page);
            if (page != null)
                PageIndex++;
        }

        /// <summary>
        /// Decreases PageIndex by 1, but only if the current index - 1 is found in the dictionary. Also sets MainText to the index page.
        /// </summary>
        private void PageAdvancementLeft(UIMouseEvent evt, UIElement listeningElement)
        {
            string page = TryGetEntry(PageIndex - 1, 0);
            if (page != null)
                PageIndex--;
        }
    }
}

