using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubworldLibrary;
using Terraria.GameContent.Biomes;
using Terraria.GameContent.UI.States;
using UltimateSkyblock.Content.Subworlds.DungeonPasses;

namespace UltimateSkyblock.Content.Subworlds
{
    public class DungeonSubworld : Subworld
    {
        public override int Width => 900;
        public override int Height => 500;
        public override bool ShouldSave => false;
        public override string Name => "Dungeon Subworld";

        private UIWorldLoad _menu;

        public override void DrawMenu(GameTime gameTime)
        {
            if (WorldGenerator.CurrentGenerationProgress != null)
                (_menu ??= new UIWorldLoad()).Draw(Main.spriteBatch);
            else
                base.DrawMenu(gameTime);
        }

        public sealed override List<GenPass> Tasks
        {
            get
            {
                List<GenPass> returnList = new List<GenPass>
                {
                    new BasicWorldGenPass("Setting world stats", 5f),
                };

                return returnList;
            }
        }

        public override void OnEnter()
        {
            SubworldSystem.hideUnderworld = false;
        }
    }
}