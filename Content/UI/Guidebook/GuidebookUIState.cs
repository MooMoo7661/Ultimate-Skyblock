using System;
using UltimateSkyblock.Content.Utils;

namespace UltimateSkyblock.Content.UI.Guidebook
{
    public class GuidebookUIState : UIState
    {
        public DraggableUIPanel GuidebookPanel;

        UIText Title;
        UIText PageName;
        UIText MainText;
        UIText PageNumber;

        UITextButton RightButton;
        UITextButton LeftButton;
        UITextButton wikiPageButton;
        UITextButton discordLinkButton;

        UIImage wikiPageIcon;
        UIImage discordLinkIcon;

        Color DefaultColor = new Color(44, 57, 105, 178); // Default color of standard terraria UI.

        public int PageIndex = 0;
        string MainPage = "";

        /// <summary>
        /// Used for storing pages.
        /// Values can be quickly obtained through TryGetValue(), and the corresponding StyleID.
        /// </summary>
        public static readonly Dictionary<int, Page> Pages = new();

        /// <summary>
        /// Used for determining what entry to get with TryGetEntry
        /// </summary>
        public enum StyleID
        {
            Page,
            Name,
            WikiPage
        }

        /// <summary>
        /// Used for quick referencing page numbers by name.
        /// </summary>
        public enum PageID
        {
            Main = 0,
            Hardmode = 4,
            Fishing = 5,
            MiningSW = 6,
            PlanteraSW = 7,
            Shimmer = 8,
        }

        /// <summary>
        /// Used to get either the current page name, or the text to display
        /// </summary>
        /// <param name="index">Index to search the dictionary for. Intended to have PageIndex passed in.</param>
        /// <returns>Dictionary entry for the provided key, or null if not found</returns>
        public static string TryGetEntry(int index, StyleID style)
        {
            if (Pages.TryGetValue(index, out Page page))
            {
                switch (style)
                {
                    case StyleID.Page:
                        return page.Text;

                    case StyleID.Name:
                        return page.Title;

                    case StyleID.WikiPage:
                        return page.Wiki;
                }
            }
            return null;
        }

        /// <summary>
        /// Adding the pages to the dictionary
        /// </summary>
        public static void AddPages()
        {
            string path = "Mods.UltimateSkyblock.LocalizedPages.";
            Pages.TryAdd(0, new Page(Language.GetTextValue(path + "PageNames.Intro"), Language.GetTextValue(path + "IntroPage")));
            Pages.TryAdd(1, new Page(Language.GetTextValue(path + "PageNames.TableOfContents"), Language.GetTextValue(path + "TableOfContents")));
            Pages.TryAdd(2, new Page(Language.GetTextValue(path + "PageNames.Progression"), Language.GetTextValue(path + "Progression1")));
            Pages.TryAdd(3, new Page(Language.GetTextValue(path + "PageNames.Progression"), Language.GetTextValue(path + "Progression2")));
            Pages.TryAdd(4, new Page(Language.GetTextValue(path + "PageNames.Hardmode"), Language.GetTextValue(path + "Hardmode"), "https://terraria.wiki.gg/wiki/Hardmode"));
            Pages.TryAdd(5, new Page(Language.GetTextValue(path + "PageNames.Fishing"), Language.GetTextValue(path + "Fishing"), "https://terraria.wiki.gg/wiki/Hardmode"));
            Pages.TryAdd(6, new Page(Language.GetTextValue(path + "PageNames.MiningSubworld"), Language.GetTextValue(path + "MiningSubworld")));
            Pages.TryAdd(7, new Page(Language.GetTextValue(path + "PageNames.PlanteraSubworld"), Language.GetTextValue(path + "PlanteraSubworld"), "https://terraria.wiki.gg/wiki/Plantera"));
            Pages.TryAdd(8, new Page(Language.GetTextValue(path + "PageNames.Shimmer"), Language.GetTextValue(path + "Shimmer"), "https://terraria.wiki.gg/wiki/Shimmer"));
      
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

            UITextButton miningSWButton = new UITextButton("", 0, 0.05f, 40, 40, "Mining Subworld", SoundID.MenuClose);
            miningSWButton.OnLeftClick += new MouseEvent(MiningClicked);
            miningSWButton.MarginLeft = 15;
            miningSWButton.MarginTop = 135;
            GuidebookPanel.Append(miningSWButton);

            Asset<Texture2D> mining = ModContent.Request<Texture2D>("UltimateSkyblock/Content/UI/Guidebook/Assets/MiningSWIcon", AssetRequestMode.ImmediateLoad);
            UIImage miningPage = new UIImage(mining);
            miningPage.HAlign = 0.5f;
            miningPage.VAlign = 0.5f;
            miningPage.ScaleToFit = true;
            miningSWButton.Append(miningPage);

            UITextButton planteraSWButton = new UITextButton("", 0, 0.05f, 40, 40, "Plantera Subworld", SoundID.MenuClose);
            planteraSWButton.OnLeftClick += new MouseEvent(PlanteraClicked);
            planteraSWButton.MarginLeft = 15;
            planteraSWButton.MarginTop = 180;
            GuidebookPanel.Append(planteraSWButton);

            Asset<Texture2D> plantera = ModContent.Request<Texture2D>("UltimateSkyblock/Content/UI/Guidebook/Assets/PlanteraSWIcon", AssetRequestMode.ImmediateLoad);
            UIImage planteraPage = new UIImage(plantera);
            planteraPage.HAlign = 0.5f;
            planteraPage.VAlign = 0.5f;
            planteraPage.ScaleToFit = true;
            planteraSWButton.Append(planteraPage);
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

            wikiPageButton = new UITextButton("", 0, 0.05f, 40, 40, "", SoundID.MenuOpen);
            wikiPageButton.OnLeftClick += new MouseEvent(WikiOpen);
            wikiPageButton.HAlign = 0.2f;
            wikiPageButton.VAlign = 0.95f;
            GuidebookPanel.Append(wikiPageButton);

            Main.instance.LoadItem(ItemID.Book);
            Asset<Texture2D> book = TextureAssets.Item[ItemID.Book];
            wikiPageIcon = new UIImage(book);
            wikiPageIcon.HAlign = 0.5f;
            wikiPageIcon.VAlign = 0.5f;
            wikiPageIcon.ScaleToFit = true;
            wikiPageButton.Append(wikiPageIcon);

            discordLinkButton = new UITextButton("", 0, 0.05f, 40, 40, "", SoundID.MenuOpen);
            discordLinkButton.OnLeftClick += new MouseEvent(DiscordClicked);
            discordLinkButton.HAlign = 0.25f;
            discordLinkButton.VAlign = 0.95f;
            GuidebookPanel.Append(discordLinkButton);

            Asset<Texture2D> discord = ModContent.Request<Texture2D>("UltimateSkyblock/Content/UI/Guidebook/Assets/DiscordIcon", AssetRequestMode.ImmediateLoad);
            discordLinkIcon = new UIImage(discord);
            discordLinkIcon.HAlign = 0.5f;
            discordLinkIcon.VAlign = 0.5f;
            discordLinkIcon.ScaleToFit = false;
            discordLinkButton.Append(discordLinkIcon);

            UITextButton trello = new UITextButton("", 0, 0.05f, 40, 40, "", SoundID.MenuOpen);
            trello.OnLeftClick += new MouseEvent(TrelloClicked);
            trello.HAlign = 0.3f;
            trello.VAlign = 0.95f;
            GuidebookPanel.Append(trello);

            Asset<Texture2D> trelloIcon = ModContent.Request<Texture2D>("UltimateSkyblock/Content/UI/Guidebook/Assets/TrelloIcon", AssetRequestMode.ImmediateLoad);
            UIImage trelloLinkIcon = new UIImage(trelloIcon);
            trelloLinkIcon.HAlign = 0.5f;
            trelloLinkIcon.VAlign = 0.5f;
            //trelloLinkIcon.ScaleToFit = true;
            trelloLinkIcon.ImageScale = 2f;
            trello.Append(trelloLinkIcon);
        }

        private void CloseClicked(UIMouseEvent evt, UIElement listeningElement) => ModContent.GetInstance<GuidebookSystem>().HideMyUI();
        private void HomeClicked(UIMouseEvent evt, UIElement listeningElement) => PageIndex = (int)PageID.Main;
        private void FishingClicked(UIMouseEvent evt, UIElement listeningElement) => PageIndex = (int)PageID.Fishing;
        private void MiningClicked(UIMouseEvent evt, UIElement listeningElement) => PageIndex = (int)PageID.MiningSW;
        private void PlanteraClicked(UIMouseEvent evt, UIElement listeningElement) => PageIndex = (int)PageID.PlanteraSW;
        private void DiscordClicked(UIMouseEvent evt, UIElement listeningElement) => Terraria.Utils.OpenToURL("https://discord.com/invite/G5cbT7tj9K");
        private void TrelloClicked(UIMouseEvent evt, UIElement listeningElement) => Terraria.Utils.OpenToURL("https://trello.com/invite/b/Z5UV1Kji/ATTI97947e73d35995538596d6db7780e476427D47AF/ultimate-skyblock");

        private void SafeguardIndex() { if (PageIndex < 0) PageIndex = 0; }

        private void WikiOpen(UIMouseEvent evt, UIElement listeningElement)
        {
            string wikiLink = TryGetEntry(PageIndex, StyleID.WikiPage);
            if (wikiLink != null)
            {
                Terraria.Utils.OpenToURL(wikiLink);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            SafeguardIndex();
            UpdatePage();
        }

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
                Color color = Color.Lerp(Color.DodgerBlue, Color.MediumPurple, (MathF.Sin(Main.GlobalTimeWrappedHourly * 2.9f) + 1) / 2f);

                GuidebookPanel.RemoveChild(PageName);
                PageName = new UIText(pageName.ToHexString(color), 1f);
                PageName.HAlign = 0.5f;
                PageName.MarginTop = 420;
                GuidebookPanel.Append(PageName);
            }

            GuidebookPanel.RemoveChild(PageNumber);
            PageNumber = new UIText("- " + (PageIndex + 1).ToString() + " -");
            PageNumber.HAlign = 0.5f;
            PageNumber.MarginTop = 450;
            GuidebookPanel.Append(PageNumber);

            //Updating button/icon colors
            string page = TryGetEntry(PageIndex - 1, StyleID.Page);
            LeftButton.BackgroundColor = page == null ? Color.Gray : DefaultColor;

            page = TryGetEntry(PageIndex + 1, StyleID.Page);
            RightButton.BackgroundColor = page == null ? Color.Gray : DefaultColor;

            string wiki = TryGetEntry(PageIndex, StyleID.WikiPage);
            wikiPageIcon.Color = wiki == null ? Color.Gray : Color.White;
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

