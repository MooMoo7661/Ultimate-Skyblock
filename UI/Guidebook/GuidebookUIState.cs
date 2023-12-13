using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using OneBlock.UI.Guidebook;
using Microsoft.Xna.Framework;

namespace OneBlock.UI.GuideBook
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
            string path = "Mods.OneBlock.LocalizedPages.";
            Pages.Add(0, Language.GetTextValue(path + "IntroPage")); PageNames.Add(0, Language.GetTextValue(path + "PageNames.Intro"));
            Pages.Add(1, Language.GetTextValue(path + "TableOfContents")); PageNames.Add(1, Language.GetTextValue(path + "PageNames.TableOfContents"));
            Pages.Add(2, Language.GetTextValue(path + "Progression1")); PageNames.Add(2, Language.GetTextValue(path + "PageNames.Progression"));
            Pages.Add(3, Language.GetTextValue(path + "Progression2")); PageNames.Add(3, Language.GetTextValue(path + "PageNames.Progression"));
            Pages.Add(4, Language.GetTextValue(path + "Fishing")); PageNames.Add(4, Language.GetTextValue(path + "PageNames.Fishing"));
        }

        /// <summary>
        /// Once created. Used to create the UI for the book, and assign sizes, colors, etc.
        /// </summary>
        public override void OnInitialize()
        {
            AddPages();

            string introButtonText = Language.GetTextValue("Mods.SkyblockBrutalism.UI.IntroButton");
            string title = "Skyblock Guidebook";
            string rightButtonText = ">";
            string leftButtonText = "<";

            GuidebookPanel = new DraggableUIPanel();
            GuidebookPanel.SetPadding(0);
            GuidebookPanel.HAlign = 0.5f;
            GuidebookPanel.VAlign = 0.1f;
            GuidebookPanel.Width.Set(900f, 0f);
            GuidebookPanel.Height.Set(500f, 0f);
            GuidebookPanel.BackgroundColor = new Color(73, 94, 171);
            Append(GuidebookPanel);

            UITextButton close = new UITextButton("X", 0.975f, 0.025f, 40, 30, Language.GetTextValue("LegacyInterface.52"), SoundID.MenuClose);
            close.OnLeftClick += new MouseEvent(CloseClicked);
            GuidebookPanel.Append(close);

            Title = new UIText(title);
            Title.HAlign = 0.5f;
            Title.MarginTop = 15;
            GuidebookPanel.Append(Title);

            PageNumber = new UIText("- " + (PageIndex + 1).ToString() + " -");
            PageNumber.HAlign = 0.5f;
            PageNumber.MarginTop = 450;
            GuidebookPanel.Append(PageNumber);

            PageName = new UIText("Introduction");
            PageName.HAlign = 0.5f;
            PageName.MarginTop = 450;
            GuidebookPanel.Append(PageName);

            MainPage = Language.GetTextValue("Mods.OneBlock.LocalizedPages.IntroPage");
            MainText = new UIText(MainPage, 1f);
            MainText.MarginLeft = 15;
            MainText.MarginTop = 50;
            GuidebookPanel.Append(MainText);

            LeftButton = new UITextButton(leftButtonText, 0.05f, 0.95f, 130, 34, "", SoundID.MenuTick);
            LeftButton.OnLeftClick += new MouseEvent(PageAdvancementLeft);
            GuidebookPanel.Append(LeftButton);

            RightButton = new UITextButton(rightButtonText, 0.95f, 0.95f, 130, 34, "", SoundID.MenuTick);
            RightButton.OnLeftClick += new MouseEvent(PageAdvancementRight);
            GuidebookPanel.Append(RightButton);
        }

        private void CloseClicked(UIMouseEvent evt, UIElement listeningElement) => ModContent.GetInstance<GuidebookSystem>().HideMyUI();

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            SafeguardIndex();
            UpdateButtonColors();
            UpdatePageName();
        }

        private void SafeguardIndex() { if (PageIndex < 0) PageIndex = 0; }

        private void UpdatePageName()
        {
            string pageName = TryGetEntry(PageIndex, StyleID.Name);
            if (pageName != null)
            {
                GuidebookPanel.RemoveChild(PageName);
                PageName = new UIText(pageName, 1f);
                PageName.HAlign = 0.5f;
                PageName.MarginTop = 420;
                GuidebookPanel.Append(PageName);
            }
        }

        /// <summary>
        /// Used to handle graying the right and left arrow buttons depending on if index + 1 or - 1 is null
        /// </summary>
        private void UpdateButtonColors()
        {
            string page = TryGetEntry(PageIndex - 1, StyleID.Page);
            if (page == null)
            {
                LeftButton.BackgroundColor = Color.Gray;
            }
            else
            {
                LeftButton.BackgroundColor = DefaultColor;
            }

            page = TryGetEntry(PageIndex + 1, StyleID.Page);
            if (page == null)
            {
                RightButton.BackgroundColor = Color.Gray;
            }
            else
            {
                RightButton.BackgroundColor = DefaultColor;
            }
        }

        /// <summary>
        /// Increases PageIndex by 1, but only if the current index + 1 is found in the dictionary. Also sets MainText to the index page.
        /// </summary>
        private void PageAdvancementRight(UIMouseEvent evt, UIElement listeningElement)
        {
            string page = TryGetEntry(PageIndex + 1, StyleID.Page);
            if (page != null)
            {
                PageIndex++;
                GuidebookPanel.RemoveChild(MainText);
                GuidebookPanel.RemoveChild(PageNumber);

                MainPage = page;
                MainText = new UIText(page, 1f);
                MainText.MarginLeft = 15;
                MainText.MarginTop = 50;

                PageNumber = new UIText("- " + (PageIndex + 1).ToString() + " -", 1f);
                PageNumber.HAlign = 0.5f;
                PageNumber.MarginTop = 450;

                GuidebookPanel.Append(MainText);
                GuidebookPanel.Append(PageNumber);
            }
        }

        /// <summary>
        /// Decreases PageIndex by 1, but only if the current index - 1 is found in the dictionary. Also sets MainText to the index page.
        /// </summary>
        private void PageAdvancementLeft(UIMouseEvent evt, UIElement listeningElement)
        {
            string page = TryGetEntry(PageIndex - 1, 0);
            if (page != null)
            {
                PageIndex--;
                GuidebookPanel.RemoveChild(MainText);
                GuidebookPanel.RemoveChild(PageNumber);

                MainPage = page;
                MainText = new UIText(page, 1f);
                MainText.MarginLeft = 15;
                MainText.MarginTop = 50;

                PageNumber = new UIText("- " + (PageIndex + 1).ToString() + " -", 1f);
                PageNumber.HAlign = 0.5f;
                PageNumber.MarginTop = 450;

                GuidebookPanel.Append(MainText);
                GuidebookPanel.Append(PageNumber);
            }
        }

    }
}

