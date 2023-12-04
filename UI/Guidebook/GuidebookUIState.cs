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
using Terraria;
using OneBlock.UI.Guidebook;
using Microsoft.Xna.Framework;
using OneBlock.Items.Guidebook;

namespace OneBlock.UI.GuideBook
{
    public class GuidebookUIState : UIState
    {
        public DragableUIPanel SkyGuidePanel;
        UITextButton TipsButton;
        UIText TipsButtonText;
        UIText IntroButtonText;
        UIText Title;
        UIText Intro;
        UIText Tips;
        UITextButton EasyButton;
        UITextButton HardButton;
        UITextButton BrutalButton;
        bool tipsPage;
        public override void OnInitialize()
        {
            string tipsButtonText = "Tips";//Language.GetTextValue("Mods.SkyblockBrutalism.UI.TipsButton");
            string introButtonText = Language.GetTextValue("Mods.SkyblockBrutalism.UI.IntroButton");
            string title = Language.GetTextValue("Mods.SkyblockBrutalism.UI.Title");
            string introText = Language.GetTextValue("Mods.SkyblockBrutalism.UI.Intro");
            string tipsText = Language.GetTextValue("Mods.SkyblockBrutalism.UI.Tips");
            string easyButtonText = Language.GetTextValue("Mods.SkyblockBrutalism.UI.EasyButton");
            string hardButtonText = Language.GetTextValue("Mods.SkyblockBrutalism.UI.HardButton");
            string brutalButtonText = Language.GetTextValue("Mods.SkyblockBrutalism.UI.BrutalButton");

            // Window
            SkyGuidePanel = new DragableUIPanel();
            SkyGuidePanel.SetPadding(0);
            SkyGuidePanel.HAlign = 0.5f;
            SkyGuidePanel.VAlign = 0.1f;
            SkyGuidePanel.Width.Set(600f, 0f);
            SkyGuidePanel.Height.Set(400f, 0f);
            SkyGuidePanel.BackgroundColor = new Color(73, 94, 171);
            Append(SkyGuidePanel);

            //Hover displays localized text "Close"
            UITextButton close = new UITextButton("X", 0.975f, 0.025f, 40, 30, Language.GetTextValue("LegacyInterface.52"), SoundID.MenuClose);
            close.OnLeftClick += new MouseEvent(CloseClicked);
            SkyGuidePanel.Append(close);

            //Title
            Title = new UIText(title);
            Title.HAlign = 0.5f;
            Title.MarginTop = 15;
            SkyGuidePanel.Append(Title);

            //Tips button with alternating Text
            //Note this is the only "OnLeftMouseUp" used here.  DragableUIPanel gets glued to the mouse if dragging while hovering on a swapping element (the button text)
            TipsButton = new UITextButton("", 0.5f, 0.975f, 80, 34, "", SoundID.MenuOpen);
            TipsButton.OnLeftMouseUp += new MouseEvent(TipsButtonClicked);
            SkyGuidePanel.Append(TipsButton);

            //if I could just refresh the text element in the TipsButton, I wouldn't need either of these.
            IntroButtonText = new UIText(introButtonText);
            IntroButtonText.HAlign = 0.5f;
            IntroButtonText.VAlign = 0.35f;

            TipsButtonText = new UIText(tipsButtonText);
            TipsButtonText.HAlign = 0.5f;
            TipsButtonText.VAlign = 0.35f;

            //SkyGuide Starting Info
            Intro = new UIText(introText, 1f);
            Intro.MarginLeft = 15;
            Intro.MarginTop = 50;

            //SkyGuide Tips
            Tips = new UIText(tipsText, 1f);
            Tips.MarginLeft = 15;
            Tips.MarginTop = 50;

            //Conditional Intro Buttons.  This was the majority of my week of effort.
            EasyButton = new UITextButton(easyButtonText, 0.05f, 0.85f, 130, 34, Language.GetTextValue("RandomWorldName_Adjective.Easy"), SoundID.Pixie);
            EasyButton.OnLeftClick += new MouseEvent(EasyButtonClicked);

            HardButton = new UITextButton(hardButtonText, 0.5f, 0.85f, 130, 34, Language.GetTextValue("Prefix.Hard"), SoundID.ZombieMoan);
            HardButton.OnLeftClick += new MouseEvent(HardButtonClicked);

            BrutalButton = new UITextButton(brutalButtonText, 0.95f, 0.85f, 130, 34, Language.GetTextValue("RandomWorldName_Adjective.Brutal"), SoundID.Zombie105);
            BrutalButton.OnLeftClick += new MouseEvent(BrutalButtonClicked);

            if (!tipsPage)
            {
                SkyGuidePanel.Append(Intro);
                SkyGuidePanel.Append(EasyButton);
                SkyGuidePanel.Append(HardButton);
                SkyGuidePanel.Append(BrutalButton);
                TipsButton.Append(TipsButtonText);
            }
            else
            {
                SkyGuidePanel.Append(Tips);
                TipsButton.Append(IntroButtonText);
            }
        }

        private void CloseClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            ModContent.GetInstance<SkyUISystem>().HideMyUI();
        }
        //The last open page is remembered during the current session.  There are, IMO, several duplicate elements and variables I could delete if I just knew how to refresh an element with it's updated values on click rather simply removing and adding entire elements.
        private void TipsButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            if (!tipsPage)
            {
                tipsPage = true;
                SkyGuidePanel.RemoveChild(Intro);
                SkyGuidePanel.RemoveChild(EasyButton);
                SkyGuidePanel.RemoveChild(HardButton);
                SkyGuidePanel.RemoveChild(BrutalButton);
                TipsButton.RemoveChild(TipsButtonText);
                SkyGuidePanel.Append(Tips);
                TipsButton.Append(IntroButtonText);

            }
            else
            {
                tipsPage = false;
                SkyGuidePanel.RemoveChild(Tips);
                TipsButton.RemoveChild(IntroButtonText);
                SkyGuidePanel.Append(Intro);
                SkyGuidePanel.Append(EasyButton);
                SkyGuidePanel.Append(HardButton);
                SkyGuidePanel.Append(BrutalButton);
                TipsButton.Append(TipsButtonText);
            }
        }
        //function that exists just to pass the variable.
        public void RecieveChoice(int ReadSkyChoice)
        {
            if (ReadSkyChoice != 0)
            {
                EasyButton.isSelected = false;
                HardButton.isSelected = false;
                BrutalButton.isSelected = false;
                if (ReadSkyChoice == 1)
                {
                    EasyButton.isSelected = true;
                }
                else if (ReadSkyChoice == 2)
                {
                    HardButton.isSelected = true;
                }
                else if (ReadSkyChoice == 3)
                {
                    BrutalButton.isSelected = true;
                }
            }
            else
            {
                EasyButton.isSelected = null;
                HardButton.isSelected = null;
                BrutalButton.isSelected = null;
            }
        }

        private bool LootSkyGuide(int writeSkyChoice)
        {
            for (int i = 0; i < 58; i++)
            {
                if (Main.LocalPlayer.inventory[i].type == ModContent.ItemType<GuidebookItem>() && Main.LocalPlayer.inventory[i].ModItem is GuidebookItem guide)
                {
                    if (guide.Choice == 0)
                    {
                        guide.Choice = writeSkyChoice;
                        RecieveChoice(writeSkyChoice);
                        return true;
                    }
                }
            }
            return false;
        }
        //I didn't bother to figure out a fancy combined function for my choice buttons.
        private void EasyButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            if (EasyButton.isSelected == null && LootSkyGuide(1))
            {
                Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_FromThis(), ItemID.BottomlessShimmerBucket);
            }
        }
        private void HardButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            if (HardButton.isSelected == null && LootSkyGuide(2))
            {
                Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_FromThis(), ItemID.StoneBlock, 10);
            }
        }
        private void BrutalButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            if (BrutalButton.isSelected == null && LootSkyGuide(3))
            {
                Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_FromThis(), ItemID.AshBlock, 10);
            }
        }
    }
}

