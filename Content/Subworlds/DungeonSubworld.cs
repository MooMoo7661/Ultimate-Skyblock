using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubworldLibrary;
using Terraria.Audio;
using Terraria.GameContent.Biomes;
using Terraria.GameContent.UI.States;
using Terraria.ModLoader.Utilities;
using UltimateSkyblock.Content.Configs;
using UltimateSkyblock.Content.Subworlds.DungeonPasses;

namespace UltimateSkyblock.Content.Subworlds
{
    public class DungeonSubworld : Subworld
    {
        public override int Width => 3000;
        public override int Height => 1000;
        public override bool ShouldSave => false;
        public override string Name => "Dungeon Subworld";

        public static bool Music = false;

        private UIWorldLoad _menu;

        public override void DrawMenu(GameTime gameTime)
        {
            if (WorldGenerator.CurrentGenerationProgress != null)
                (_menu ??= new UIWorldLoad()).Draw(Main.spriteBatch);
            else
                base.DrawMenu(gameTime);
        }

        public override bool ChangeAudio()
        {
            if (Main.gameMenu && ModContent.GetInstance<SubworldClientConfig>().SubworldLoadingMusic)
            {
                Main.newMusic = MusicLoader.GetMusicSlot("UltimateSkyblock/Content/Sounds/Music/BubbleBobble");
                return true;
            }

            return false;
        }

        public override void OnEnter()
        {
            SubworldSystem.hideUnderworld = true;
            Music = false;

            UltimateSkyblock.Instance.Logger.Info("Logging tag \"NPC.downedPlantBoss\"" + " - " + NPC.downedPlantBoss);
            UltimateSkyblock.Instance.Logger.Info("Logging tag \"NPC.downedGolemBoss\"" + " - " + NPC.downedGolemBoss);
            UltimateSkyblock.Instance.Logger.Info("Logging tag \"NPC.downedBoss3\"" + " - "+ NPC.downedBoss3);
            UltimateSkyblock.Instance.Logger.Info("Logging tag \"Main.hardMode:\"" + " - " + Main.hardMode);
        }

        public override void OnLoad()
        {
            Main.worldSurface = 0;
            GenVars.worldSurfaceHigh = 0;
            GenVars.worldSurfaceLow = 0;
            GenVars.rockLayer = Main.maxTilesY / 2;
            GenVars.oceanWaterStartRandomMin = 0;
            GenVars.oceanWaterStartRandomMax = 0;
        }

        public sealed override List<GenPass> Tasks
        {
            get
            {
                List<GenPass> returnList = new List<GenPass>
                {
                    new TileFiller("Setting world stats", 5f),
                    new DungeonGenerationPass("Dungeon Generation", 1f),
                    new WallFiller("Filling the other half", 5f),

                    new WhiteFramingPass("Generating White Extensions", 10f),
                    new PlatformPass("Dynamic Platforms", 10f),
                    new PurpleFramingPass("Generating Purple Extensions", 10f),
                    new ChestGenerationPass("Generating Chests", 10f),
                    new CleanupPass("Cleaning Up", 5f),

                    new SpikeRunner("Spikes", 10f),
                    new PaintingMaker("Placing Paintings", 15f),
                    new FurnitureGenerator("Generating Furntiture", 20f),
                    new PotMaker("Creating Pots", 10f),
                    new RubbleCreator("Growing Rubble", 15f),
                    new ShelfInhabitor("Feeding the Shelves", 15f),
                    new BannerPlacer("Cultivating Banners", 5f)
                };

                return returnList;
            }
        }
    }
}