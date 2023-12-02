using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.Map;
using Terraria.ModLoader;
using Terraria.UI;

namespace OneBlock.MapDrawing
{
    public class NpcIconDrawing : ModMapLayer
    {
        private static readonly string path = "OneBlock/MapDrawing/Icons/";
        public Texture2D friendly = ModContent.Request<Texture2D>(path + "IconFriendly", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
        public Texture2D hostile = ModContent.Request<Texture2D>(path + "IconHostile", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
        public override void Draw(ref MapOverlayDrawContext context, ref string text)
        {
            if (ModLoader.TryGetMod("WorldMapExpansion", out Mod MapExpansion)) { return; }

            foreach(NPC npc in Main.npc.SkipLast(1)) //Last is a dummy npc, don't want to interact with it
            {
                Texture2D iconToDraw;
                if (npc.CanBeChasedBy() || npc.damage > 0)
                {
                    iconToDraw = hostile;
                }
                else
                {
                    iconToDraw = friendly;
                }

                if (npc.active && npc.life > 0 && !npc.boss && !npc.townNPC)
                {
                    var hell = context.Draw(iconToDraw, new Vector2(npc.Center.ToTileCoordinates().X, npc.Center.ToTileCoordinates().Y), Color.White, new SpriteFrame(1, 1, 0, 0), 0.5f, 0.5f, Alignment.Center);
                    if (hell.IsMouseOver) { text = npc.FullName; }
                }
            }
        }
    }
}
