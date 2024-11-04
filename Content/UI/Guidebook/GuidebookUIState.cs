using System;
using Terraria.Audio;
using Terraria;
using UltimateSkyblock.Content.Utils;
using Terraria.ModLoader.UI;
using UltimateSkyblock.Content.Keybinds;

namespace UltimateSkyblock.Content.UI.Guidebook
{
    public class GuidebookUIState : UIState
    {
        public DraggableUIPanel GuidebookPanel;

        UIText Title;
        UIText PageName;
        UIText MainText;
        UIText PageNumber;

        List<UIImage> PageImages;
        
        UITextButton RightButton;
        UITextButton LeftButton;
        UITextButton wikiPageButton;
        UITextButton discordLinkButton;

        UIImage wikiPageIcon;
        UIImage discordLinkIcon;

        Color DefaultColor = new(UICommon.MainPanelBackground.R, UICommon.MainPanelBackground.G, UICommon.MainPanelBackground.B, 222);
        public int PageIndex = 0;
        string MainPage = "";

        /// <summary>
        /// Used for storing pages.
        /// Values can be quickly obtained through TryGetValue(), and the corresponding StyleID.
        /// </summary>
        public static Dictionary<int, Page> Pages = new();

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
            Shimmer = 7,
            MapMarkers = 8,
            Liquids = 9,
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
            Pages.TryAdd(8, new Page(Language.GetTextValue(path + "PageNames.BiomeCores"), Language.GetTextValue(path + "BiomeCores")));

            AddDetailedPages();
        }

        public static void AddDetailedPages()
        {
            string path = "Mods.UltimateSkyblock.LocalizedPages.";
            string texPath = "UltimateSkyblock/Content/UI/Guidebook/PageImages/";

            UIImage lakeDiagram = new Page().Image(ModContent.Request<Texture2D>(texPath + "FishingDimensions"), 0.55f, 0.515f);
            Pages.TryAdd(5, new Page(Language.GetTextValue(path + "PageNames.Fishing"), Language.GetTextValue(path + "Fishing"), "https://terraria.wiki.gg/wiki/Fishing", new List<UIImage> { lakeDiagram }));

            UIImage glowcapDiagram = new Page().Image(ModContent.Request<Texture2D>(texPath + "GlowcapGuidebookImage"), 0.6f, 0.515f);
            Pages.TryAdd(6, new Page(Language.GetTextValue(path + "PageNames.MiningSubworld"), Language.GetTextValue(path + "MiningSubworld"), null, new List<UIImage> { glowcapDiagram }));

            Main.instance.LoadItem(ItemID.AegisFruit);
            Main.instance.LoadItem(ItemID.BottomlessShimmerBucket);
            Main.instance.LoadItem(ItemID.Ambrosia);

            UIImage walt = new Page().Image(TextureAssets.Item[ItemID.BottomlessShimmerBucket], 0.5f, 0.8f, 1.3f);
            UIImage ambrosia = new Page().Image(TextureAssets.Item[ItemID.Ambrosia], 0.425f, 0.8f, 1.3f);
            UIImage aegis = new Page().Image(TextureAssets.Item[ItemID.AegisFruit], 0.575f, 0.8f, 1.3f);
            Page page = new Page(Language.GetTextValue(path + "PageNames.Shimmer"), Language.GetTextValue(path + "Shimmer"), "https://terraria.wiki.gg/wiki/Shimmer", new List<UIImage> { walt, ambrosia, aegis });
            Pages.TryAdd(7, page);

            UIImage liquidDiagram = new Page().Image(ModContent.Request<Texture2D>(texPath + "LiquidDuplicationSetup"), HAlign:0.365f, VAlign:0.5f);
            Page liquid = new Page(Language.GetTextValue(path + "PageNames.LiquidDuplication"), Language.GetTextValue(path + "LiquidDuplication"), "https://terraria.wiki.gg/wiki/Liquids", new List<UIImage> { liquidDiagram });
            Pages.TryAdd(9, liquid);
        }

        /// <summary>
        /// Once created. Used to create the UI for the book, and assign sizes, colors, etc.
        /// </summary>
        public override void OnInitialize()
        {
            AddPages();

            GuidebookPanel = new DraggableUIPanel();
            GuidebookPanel.SetPadding(0);
            GuidebookPanel.HAlign = 0.5f;
            GuidebookPanel.VAlign = 0.1f;
            GuidebookPanel.Width.Set(1000f, 0f);
            GuidebookPanel.Height.Set(650f, 0f);
            GuidebookPanel.BackgroundColor = DefaultColor;
            Append(GuidebookPanel);

            Title = new UIText("Skyblock Guidebook")
            {
                HAlign = 0.5f,
                MarginTop = 15
            };
            GuidebookPanel.Append(Title);

            UIText QuickPages = new UIText("Select", 0.9f)
            {
                MarginLeft = 15,
                MarginTop = 30
            };
            GuidebookPanel.Append(QuickPages);

            PageNumber = new UIText("- " + (PageIndex + 1).ToString() + " -")
            {
                HAlign = 0.5f,
                MarginTop = 450
            };
            GuidebookPanel.Append(PageNumber);

            PageName = new UIText("Introduction")
            {
                HAlign = 0.5f,
                MarginTop = 450
            };
            GuidebookPanel.Append(PageName);

            MainPage = Language.GetTextValue("Mods.UltimateSkyblock.LocalizedPages.IntroPage");
            MainText = new UIText(MainPage, 0.95f)
            {
                MarginLeft = 75,
                MarginTop = 50
            };
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
            UIImage mainIcon = new UIImage(home)
            {
                HAlign = 0.5f,
                VAlign = 0.5f,
                ScaleToFit = true
            };
            mainButton.Append(mainIcon);
            UITextButton fishingButton = new UITextButton("", 0, 0.05f, 40, 40, "Fishing", SoundID.MenuClose);
            fishingButton.OnLeftClick += new MouseEvent(FishingClicked);
            fishingButton.MarginLeft = 15;
            fishingButton.MarginTop = 90;
            GuidebookPanel.Append(fishingButton);

            Main.instance.LoadItem(ItemID.Goldfish);
            Asset<Texture2D> fish = TextureAssets.Item[ItemID.Goldfish];
            UIImage quickPage = new UIImage(fish)
            {
                HAlign = 0.5f,
                VAlign = 0.5f,
                ScaleToFit = true
            };
            fishingButton.Append(quickPage);

            UITextButton miningSWButton = new UITextButton("", 0, 0.05f, 40, 40, "Mining Subworld", SoundID.MenuClose);
            miningSWButton.OnLeftClick += new MouseEvent(MiningClicked);
            miningSWButton.MarginLeft = 15;
            miningSWButton.MarginTop = 135;
            GuidebookPanel.Append(miningSWButton);
            Asset<Texture2D> mining = ModContent.Request<Texture2D>("UltimateSkyblock/Content/UI/Guidebook/Assets/MiningSWIcon", AssetRequestMode.ImmediateLoad);
            UIImage miningPage = new UIImage(mining)
            {
                HAlign = 0.5f,
                VAlign = 0.5f,
                ScaleToFit = true
            };
            miningSWButton.Append(miningPage);

            UITextButton shimmerButton = new UITextButton("", 0, 0.05f, 40, 40, "Shimmer", SoundID.MenuClose);
            shimmerButton.OnLeftClick += new MouseEvent(ShimmerClicked);
            shimmerButton.MarginLeft = 15;
            shimmerButton.MarginTop = 180;
            GuidebookPanel.Append(shimmerButton);
            Main.instance.LoadItem(ItemID.BottomlessShimmerBucket);
            Asset<Texture2D> shimmer = TextureAssets.Item[ItemID.BottomlessShimmerBucket];
            UIImage shimmerIcon = new Page().Image(shimmer, 0.5f, 0.5f, ScaleToFit: true);
            shimmerButton.Append(shimmerIcon);

            UITextButton mapMarkersButton = new UITextButton("", 0, 0.05f, 40, 40, "Map Markers", SoundID.MenuClose);
            mapMarkersButton.OnLeftClick += new MouseEvent(MarkerClicked);
            mapMarkersButton.MarginLeft = 15;
            mapMarkersButton.MarginTop = 225;
            GuidebookPanel.Append(mapMarkersButton);
            Asset<Texture2D> marker = ModContent.Request<Texture2D>("UltimateSkyblock/Content/UI/Guidebook/Assets/IconMarker", AssetRequestMode.ImmediateLoad);
            UIImage markerIcon = new Page().Image(marker, 0.5f, 0.5f, ScaleToFit: true);
            mapMarkersButton.Append(markerIcon);

            UITextButton liquidsButton = new UITextButton("", 0, 0.05f, 40, 40, "Liquids", SoundID.MenuClose);
            liquidsButton.OnLeftClick += new MouseEvent(LiquidsClicked);
            liquidsButton.MarginLeft = 15;
            liquidsButton.MarginTop = 270;
            GuidebookPanel.Append(liquidsButton);
            Main.instance.LoadItem(ItemID.WaterBucket);
            Asset<Texture2D> liquid = TextureAssets.Item[ItemID.WaterBucket];
            UIImage liquidIcon = new Page().Image(liquid, 0.5f, 0.5f, ScaleToFit: true);
            liquidsButton.Append(liquidIcon);
        }

        private void InitializeButtons()
        {
            Asset<Texture2D> closeAsset = ModContent.Request<Texture2D>("UltimateSkyblock/Content/UI/Guidebook/Assets/SearchCancel", AssetRequestMode.ImmediateLoad);
            CloseButton closeButton = new CloseButton(closeAsset, "close")
            {
                HAlign = 0.98f,
                VAlign = 0.02f
            };
            GuidebookPanel.Append(closeButton);

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
            wikiPageIcon = new UIImage(book)
            {
                HAlign = 0.5f,
                VAlign = 0.5f,
                ScaleToFit = true
            };
            wikiPageButton.Append(wikiPageIcon);

            discordLinkButton = new UITextButton("", 0, 0.05f, 40, 40, "", SoundID.MenuOpen);
            discordLinkButton.OnLeftClick += new MouseEvent(DiscordClicked);
            discordLinkButton.HAlign = 0.25f;
            discordLinkButton.VAlign = 0.95f;
            GuidebookPanel.Append(discordLinkButton);

            Asset<Texture2D> discord = ModContent.Request<Texture2D>("UltimateSkyblock/Content/UI/Guidebook/Assets/DiscordIcon", AssetRequestMode.ImmediateLoad);
            discordLinkIcon = new Page().Image(discord, 0.5f, 0.5f);
            discordLinkButton.Append(discordLinkIcon);

            UITextButton trello = new UITextButton("", 0, 0.05f, 40, 40, "", SoundID.MenuOpen);
            trello.OnLeftClick += new MouseEvent(TrelloClicked);
            trello.HAlign = 0.3f;
            trello.VAlign = 0.95f;
            GuidebookPanel.Append(trello);

            Asset<Texture2D> trelloIcon = ModContent.Request<Texture2D>("UltimateSkyblock/Content/UI/Guidebook/Assets/TrelloIcon", AssetRequestMode.ImmediateLoad);
            UIImage trelloLinkIcon = new Page().Image(trelloIcon, 0.5f, 0.5f, 2f);
            trello.Append(trelloLinkIcon);


        }

        // Quick Icons
        private void HomeClicked(UIMouseEvent _, UIElement __) => PageIndex = (int)PageID.Main;
        private void FishingClicked(UIMouseEvent _, UIElement __) => PageIndex = (int)PageID.Fishing;
        private void MiningClicked(UIMouseEvent _, UIElement __) => PageIndex = (int)PageID.MiningSW;
        private void ShimmerClicked(UIMouseEvent _, UIElement __) => PageIndex = (int)PageID.Shimmer;
        private void MarkerClicked(UIMouseEvent _, UIElement __) => PageIndex = (int)PageID.MapMarkers;

        // Secondary Icons (info / resources)
        private void DiscordClicked(UIMouseEvent _, UIElement __) => Terraria.Utils.OpenToURL("https://discord.com/invite/G5cbT7tj9K");
        private void TrelloClicked(UIMouseEvent _, UIElement __) => Terraria.Utils.OpenToURL("https://trello.com/invite/b/Z5UV1Kji/ATTI97947e73d35995538596d6db7780e476427D47AF/ultimate-skyblock");
        private void LiquidsClicked(UIMouseEvent _, UIElement __) => PageIndex = (int)PageID.Liquids;

        private void WikiOpen(UIMouseEvent _, UIElement __)
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
            UpdatePage(gameTime);
            CheckKeybinds();
        }

        private void SafeguardIndex() { if (PageIndex < 0) PageIndex = 0; }

        public void CheckKeybinds()
        {
            if (Main.LocalPlayer.mouseInterface)
                return;

            if (KeybindSystem.CloseBookKeybind.JustPressed)
            {
                ModContent.GetInstance<GuidebookSystem>().HideMyUI();
            }

            if (KeybindSystem.PageRightKeybind.JustPressed)
            {
                string page = TryGetEntry(PageIndex + 1, StyleID.Page);
                if (page != null)
                    PageIndex++;
            }

            if (KeybindSystem.PageLeftKeybind.JustPressed)
            {
                string page = TryGetEntry(PageIndex - 1, 0);
                if (page != null)
                    PageIndex--;
            }

            if (KeybindSystem.WikiPageKeybind.JustPressed)
            {
                string wikiLink = TryGetEntry(PageIndex, StyleID.WikiPage);
                if (wikiLink != null)
                {
                    Terraria.Utils.OpenToURL(wikiLink);
                }
            }
        }

        /// <summary>
        /// Handles updating the page. Updates text, title, page number, and buttons colors.
        /// </summary>
        private void UpdatePage(GameTime _)
        {
            string text = TryGetEntry(PageIndex, StyleID.Page);
            if (text != null)
            {
                GuidebookPanel.RemoveChild(MainText);
                MainText = new UIText(text, 1f)
                {
                    MarginLeft = 75,
                    MarginTop = 50
                };
                GuidebookPanel.Append(MainText);
            }

            if (PageImages != null)
            foreach(UIImage pageChild in PageImages)
            {
                if (GuidebookPanel.HasChild(pageChild))
                    GuidebookPanel.RemoveChild(pageChild);
            }

            Pages.TryGetValue(PageIndex, out Page pageImageObject);
            PageImages = pageImageObject.ListedImages;
            if (PageImages != null)
            {
                foreach(UIImage pageImage in PageImages)
                {
                    GuidebookPanel.Append(pageImage);
                }
            }

            string pageName = TryGetEntry(PageIndex, StyleID.Name);
            if (pageName != null)
            {
                Color color = Color.Lerp(Color.DodgerBlue, Color.MediumPurple, (MathF.Sin(Main.GlobalTimeWrappedHourly * 2.9f) + 1) / 2f);

                GuidebookPanel.RemoveChild(PageName);
                PageName = new UIText(pageName.ToHexString(color), 1f)
                {
                    HAlign = 0.5f,
                    VAlign = 0.9f
                };
                GuidebookPanel.Append(PageName);
            }

            GuidebookPanel.RemoveChild(PageNumber);
            PageNumber = new UIText("- " + (PageIndex + 1).ToString() + " -")
            {
                HAlign = 0.5f,
                VAlign = 0.95f
            };
            GuidebookPanel.Append(PageNumber);

            //Updating button availability
            string page = TryGetEntry(PageIndex - 1, StyleID.Page);
            if (page == null)
                GuidebookPanel.RemoveChild(LeftButton);
            else
            {
                if (!GuidebookPanel.HasChild(LeftButton))
                {
                    LeftButton = new UITextButton("<", 0.05f, 0.95f, 130, 34, "", SoundID.MenuTick);
                    LeftButton.OnLeftClick += new MouseEvent(PageAdvancementLeft);
                    GuidebookPanel.Append(LeftButton);
                }
            }

            page = TryGetEntry(PageIndex + 1, StyleID.Page);
            if (page == null)
                GuidebookPanel.RemoveChild(RightButton);
            else
            {
                if (!GuidebookPanel.HasChild(RightButton))
                {
                    RightButton = new UITextButton(">", 0.95f, 0.95f, 130, 34, "", SoundID.MenuTick);
                    RightButton.OnLeftClick += new MouseEvent(PageAdvancementRight);
                    GuidebookPanel.Append(RightButton);
                }
            }

            string wiki = TryGetEntry(PageIndex, StyleID.WikiPage);
            wikiPageIcon.Color = wiki == null ? Color.Gray : Color.White;
        }

        /// <summary>
        /// Increases PageIndex by 1, but only if the current index + 1 is found in the dictionary. Also sets MainText to the index page.
        /// </summary>
        private void PageAdvancementRight(UIMouseEvent _, UIElement __)
        {
            string page = TryGetEntry(PageIndex + 1, StyleID.Page);
            if (page != null)
                PageIndex++;
        }

        /// <summary>
        /// Decreases PageIndex by 1, but only if the current index - 1 is found in the dictionary. Also sets MainText to the index page.
        /// </summary>
        private void PageAdvancementLeft(UIMouseEvent _, UIElement __)
        {
            string page = TryGetEntry(PageIndex - 1, 0);
            if (page != null)
                PageIndex--;
        }
    }
}

