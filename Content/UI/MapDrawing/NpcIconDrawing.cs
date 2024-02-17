using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UltimateSkyblock.Content.Configs;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.Map;
using Terraria.ModLoader;
using Terraria.UI;

namespace UltimateSkyblock.Content.UI.MapDrawing
{
    public class NpcIconDrawing : ModMapLayer
    {
        private static readonly string path = "UltimateSkyblock/Content/UI/MapDrawing/Icons/";
        public Texture2D friendly = ModContent.Request<Texture2D>(path + "IconFriendly", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
        public Texture2D hostile = ModContent.Request<Texture2D>(path + "IconHostile", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

        public override void Draw(ref MapOverlayDrawContext context, ref string text)
        {
            // Hardcoded check for my other mod that does something similar.
            if (ModLoader.TryGetMod("WorldMapExpansion", out Mod MapExpansion) || !ModContent.GetInstance<SkyblockModConfig>().DrawNPCIcons) { return; }

            foreach (NPC npc in Main.npc.SkipLast(1)) //Last is a dummy npc, don't want to interact with it
            {
                Texture2D iconToDraw = (npc.CanBeChasedBy() || npc.damage > 0) ? hostile : friendly;

                if (npc.active && npc.life > 0 && !npc.boss && !npc.townNPC)
                {
                    var hell = context.Draw(iconToDraw, new Vector2(npc.Center.ToTileCoordinates().X, npc.Center.ToTileCoordinates().Y), Color.White, new SpriteFrame(1, 1, 0, 0), 0.5f, 0.5f, Alignment.Center);
                    if (hell.IsMouseOver) { text = npc.FullName; }
                }
            }
        }
    }
}
