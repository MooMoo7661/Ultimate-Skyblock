using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace OneBlock.UI.GuideBook
{
    [Autoload(Side = ModSide.Client)] // This attribute makes this class only load on a particular side. Naturally this makes sense here since UI should only be a thing clientside. Be wary though that accessing this class serverside will error
    public class SkyUISystem : ModSystem
    {
        private UserInterface SkyUserInterface;
        internal GuidebookUIState SkyUI;

        // These two methods will set the state of our custom UI, causing it to show or hide
        public void ShowMyUI(int SkyChoice)
        {
            SkyUserInterface?.SetState(SkyUI);
            SkyUI.RecieveChoice(SkyChoice);
        }

        public void HideMyUI()
        {
            SkyUserInterface?.SetState(null);
        }
        public bool IsUIOpen()
        {
            return SkyUserInterface.CurrentState != null;
        }

        public override void Load()
        {
            // Create custom interface which can swap between different UIStates
            SkyUserInterface = new UserInterface();
            // Creating custom UIState
            SkyUI = new GuidebookUIState();
        }

        public override void UpdateUI(GameTime gameTime)
        {
            // Here we call .Update on our custom UI and propagate it to its state and underlying elements
            if (SkyUserInterface?.CurrentState != null)
                SkyUserInterface?.Update(gameTime);
        }

        // Adding a custom layer to the vanilla layer list that will call .Draw on your interface if it has a state
        // Setting the InterfaceScaleType to UI for appropriate UI scaling
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "SkyblockBrutalism: SkyGuide",
                    delegate {
                        if (SkyUserInterface?.CurrentState != null)
                            SkyUserInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}
