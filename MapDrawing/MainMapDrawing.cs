using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Map;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OneBlock.SkyblockWorldGen;

namespace OneBlock.MapDrawing
{
    public class MainMapLayer : ModMapLayer
    {
        public override void Draw(ref MapOverlayDrawContext context, ref string text)
        {
            var config = ModContent.GetInstance<MapIconDrawConfig>();

            if (!config.DrawMapIcons) { return; }

            const float scaleIfNotSelected = 1f;
            const float scaleIfSelected = 1.5f;

            string path = "OneBlock/MapDrawing/Icons/";

            var dungeonIcon = TextureAssets.NpcHeadBoss[19].Value;
            var corruptionIcon = WorldGen.crimson ? ModContent.Request<Texture2D>(path + "IconEvilCrimson").Value : ModContent.Request<Texture2D>(path + "IconEvilCorruption").Value;
            var forestIcon = ModContent.Request<Texture2D>(path + "IconForest").Value;
            var jungleIcon = ModContent.Request<Texture2D>(path + "IconJungle").Value;
            var snowIcon = ModContent.Request<Texture2D>(path + "IconSnow").Value;
            var hellIcon = ModContent.Request<Texture2D>(path + "IconHell").Value;
            var mushroomIcon = ModContent.Request<Texture2D>(path + "IconMushroom").Value;

            int universalY = WorldHelpers.Evil.Y + 200;

            string evilText = WorldGen.crimson ? "Crimson" : "Corruption";

            if (config.MapIconDungeon)
            {
                var dungeon = context.Draw(dungeonIcon, new Vector2(Main.dungeonX, Main.dungeonY), Color.White, new SpriteFrame(1, 1, 0, 0), scaleIfNotSelected, scaleIfSelected, Alignment.Center);
                if (dungeon.IsMouseOver) { text = "Dungeon"; }
            }

            if (config.MapIconEvil)
            {
                var evil = context.Draw(corruptionIcon, new Vector2(WorldHelpers.Evil.X, universalY), Color.White, new SpriteFrame(1, 1, 0, 0), scaleIfNotSelected, scaleIfSelected, Alignment.Center);
                if (evil.IsMouseOver) { text = evilText; }
            }

            if (config.MapIconForest)
            {
                var forest = context.Draw(forestIcon, new Vector2(Main.spawnTileX, universalY), Color.White, new SpriteFrame(1, 1, 0, 0), scaleIfNotSelected, scaleIfSelected, Alignment.Center);
                if (forest.IsMouseOver) { text = "Forest"; }
            }

            if (config.MapIconJungle)
            {
                var jungle = context.Draw(jungleIcon, new Vector2(WorldHelpers.Jungle.X + 400, universalY), Color.White, new SpriteFrame(1, 1, 0, 0), scaleIfNotSelected, scaleIfSelected, Alignment.Center);
                if (jungle.IsMouseOver) { text = "Jungle"; }
            }

            if (config.MapIconSnow)
            {
                var snow = context.Draw(snowIcon, new Vector2(WorldHelpers.Snow.X + 400 + MainWorld.ScaleBasedOnWorldSizeX * 1.5f, universalY), Color.White, new SpriteFrame(1, 1, 0, 0), scaleIfNotSelected, scaleIfSelected, Alignment.Center);
                if (snow.IsMouseOver) { text = "Snow"; }
            }

            if (config.Hell)
            {
                var hell = context.Draw(hellIcon, new Vector2(WorldHelpers.Hell.X, WorldHelpers.Hell.Y), Color.White, new SpriteFrame(1, 1, 0, 0), scaleIfNotSelected, scaleIfSelected, Alignment.Center);
                if (hell.IsMouseOver) { text = "Hell"; }
            }

            if (config.Mushroom)
            {
                var mushroom = context.Draw(mushroomIcon,new(WorldHelpers.Mushroom.X + 50, WorldHelpers.Mushroom.Y + 30), Color.White, new SpriteFrame(1, 1, 0, 0), scaleIfNotSelected, scaleIfSelected, Alignment.Center);
                if (mushroom.IsMouseOver) { text = "Mushroom"; }
            }

        }
    }

    public class MapPositionSyncSystem : ModSystem
    {
        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(Main.spawnTileX);
            writer.Write(Main.spawnTileY);

            writer.Write(Main.dungeonX);
            writer.Write(Main.dungeonY);
        }
        public override void NetReceive(BinaryReader reader)
        {
            Main.spawnTileX = reader.ReadInt32();
            Main.spawnTileY = reader.ReadInt32();
            
            Main.dungeonX = reader.ReadInt32();
            Main.dungeonY = reader.ReadInt32();
        }
    }
}
