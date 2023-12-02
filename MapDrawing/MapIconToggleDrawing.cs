using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria;
using Terraria.Map;
using Terraria.ModLoader;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using System.IO;

namespace OneBlock.MapDrawing
{
    public class MapIconToggleDrawing : ModMapLayer
    {
        ToggleButton MainToggle;

        ToggleButton DungeonToggle;
        ToggleButton ForestToggle;
        ToggleButton EvilToggle;
        ToggleButton JungleToggle;
        ToggleButton SnowToggle;
        ToggleButton HellToggle;
        ToggleButton MushroomToggle;

        public readonly string path = "OneBlock/MapDrawing/Icons/";
        public override void Draw(ref MapOverlayDrawContext context, ref string text)
        {
            MainToggle ??= new()
            {
                BesideTexture = ModContent.Request<Texture2D>(path + "IconMain", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value,
                distanceBetweenBesideAndToggle = 150,
                tooltip = "All map icons\n",
            };
            MainToggle.DrawPos = new Point(Main.maxTilesX / 2, -750);
            MainToggle.Draw(ref context, ref text);
            MapIconDrawBools.AllIcons = MainToggle.Toggled;

            #region Column 1
            DungeonToggle ??= new()
            {
                BesideTexture = TextureAssets.NpcHeadBoss[19].Value,
                    distanceBetweenBesideAndToggle = 150,
            };
            DungeonToggle.DrawPos = new Point(Main.maxTilesX / 2 - 150, -600);
            DungeonToggle.Draw(ref context, ref text);
            MapIconDrawBools.MapIconDungeon = DungeonToggle.Toggled;

            ForestToggle ??= new()
            {
                    BesideTexture = ModContent.Request<Texture2D>(path + "IconForest", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value,
                    distanceBetweenBesideAndToggle = 150,
            };
            ForestToggle.DrawPos = new Point(Main.maxTilesX / 2 - 150, -450);
            ForestToggle.Draw(ref context, ref text);
            MapIconDrawBools.MapIconForest = ForestToggle.Toggled;

            EvilToggle ??= new()
            {
                distanceBetweenBesideAndToggle = 150,
            };
            EvilToggle.DrawPos = new Point(Main.maxTilesX / 2 - 150, -300);
            EvilToggle.BesideTexture = ModContent.Request<Texture2D>(path + (WorldGen.crimson ? "IconEvilCrimson" : "IconEvilCorruption"), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            EvilToggle.Draw(ref context, ref text);
            MapIconDrawBools.MapIconEvil = EvilToggle.Toggled;

            JungleToggle ??= new()
            {
                distanceBetweenBesideAndToggle = 150,
            };
            JungleToggle.DrawPos = new Point(Main.maxTilesX / 2 - 150, -150);
            JungleToggle.BesideTexture = ModContent.Request<Texture2D>(path + "IconJungle", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            JungleToggle.Draw(ref context, ref text);
            MapIconDrawBools.MapIconJungle = JungleToggle.Toggled;
            #endregion

            #region Column 2
            SnowToggle ??= new()
            {
                distanceBetweenBesideAndToggle = 150,
            };
            SnowToggle.DrawPos = new Point(Main.maxTilesX / 2 + 150, -600);
            SnowToggle.BesideTexture = ModContent.Request<Texture2D>(path + "IconSnow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            SnowToggle.Draw(ref context, ref text);
            MapIconDrawBools.MapIconSnow = SnowToggle.Toggled;

            HellToggle ??= new()
            {
                distanceBetweenBesideAndToggle = 150,
            };
            HellToggle.DrawPos = new Point(Main.maxTilesX / 2 + 150, -450);
            HellToggle.BesideTexture = ModContent.Request<Texture2D>(path + "IconHell", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            HellToggle.Draw(ref context, ref text);
            MapIconDrawBools.Hell = HellToggle.Toggled;

            MushroomToggle ??= new()
            {
                distanceBetweenBesideAndToggle = 150,
            };
            MushroomToggle.DrawPos = new Point(Main.maxTilesX / 2 + 150, -300);
            MushroomToggle.BesideTexture = ModContent.Request<Texture2D>(path + "IconMushroom", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            MushroomToggle.Draw(ref context, ref text);
            MapIconDrawBools.Mushroom = MushroomToggle.Toggled;

            #endregion
        }
    }
}
