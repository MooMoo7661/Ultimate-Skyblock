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
    [Autoload(Side = ModSide.Client)]
    public class GuidebookSystem : ModSystem
    {
        private UserInterface GuidebookUserInterface;
        internal GuidebookUIState GuidebookUI;

        public void ShowMyUI()
        {
            GuidebookUserInterface?.SetState(GuidebookUI);
        }

        public void HideMyUI()
        {
            GuidebookUserInterface?.SetState(null);
        }
        public bool IsUIOpen()
        {
            return GuidebookUserInterface.CurrentState != null;
        }

        public override void Load()
        {
            GuidebookUserInterface = new UserInterface();
            GuidebookUI = new GuidebookUIState();
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (GuidebookUserInterface?.CurrentState != null)
                GuidebookUserInterface?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "OneBlock: Guidebook",
                    delegate {
                        if (GuidebookUserInterface?.CurrentState != null)
                            GuidebookUserInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}
