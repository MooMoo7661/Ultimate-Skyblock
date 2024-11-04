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
        public static Asset<Texture2D> friendly;
        public static Asset<Texture2D> hostile;

        public override void Load()
        {
            hostile = ModContent.Request<Texture2D>(path + "IconHostile");
            friendly = ModContent.Request<Texture2D>(path + "IconFriendly");
        }

        public override void Draw(ref MapOverlayDrawContext context, ref string text)
        {
            // Check for my other mod that does something similar.
            if (ModLoader.TryGetMod("WorldMapExpansion", out Mod _) || !ModContent.GetInstance<SkyblockModConfig>().DrawNPCIcons || !UltimateSkyblock.IsSkyblock()) { return; }

            foreach (NPC npc in Main.npc.SkipLast(1)) //Last is a dummy npc, don't want to interact with it
            {
                Texture2D iconToDraw = (npc.CanBeChasedBy() || npc.damage > 0) ? hostile.Value : friendly.Value;

                if (npc.active && npc.life > 0 && !npc.boss && !npc.townNPC)
                {
                    var npcIcon = context.Draw(iconToDraw, new Vector2(npc.Center.ToTileCoordinates().X, npc.Center.ToTileCoordinates().Y), Color.White, new SpriteFrame(1, 1, 0, 0), 0.5f, 0.5f, Alignment.Center);
                    if (npcIcon.IsMouseOver) { text = npc.FullName; }
                }
            }
        }
    }
}
