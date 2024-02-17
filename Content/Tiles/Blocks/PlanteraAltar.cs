using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.Localization;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SubworldLibrary;

namespace UltimateSkyblock.Content.Tiles.Blocks
{
    public class PlanteraAltar : ModTile
    {
        public override void SetStaticDefaults()
        {
            // Properties
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileFrameImportant[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;
            TileID.Sets.IgnoredByNpcStepUp[Type] = true;

            DustType = DustID.Mud;

            // Placement
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
            TileObjectData.newTile.Width = 3;
            TileObjectData.newTile.Height = 5;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16, 18 };
            TileObjectData.addTile(Type);

            // Etc
            AddMapEntry(new Color(200, 200, 200), Language.GetText("MapObject.Table"));
        }

        public override bool RightClick(int i, int j)
        {

            Player player = Main.LocalPlayer;
            bool plantera = false;
            for (int k = 0; k < Main.maxNPCs; k++)
            {
                NPC npc = Main.npc[k];
                if (npc != null)
                {
                    if (npc.type == NPCID.Plantera && npc.active)
                    {
                        plantera = true;
                    }
                }
            }

            if (!plantera)
            {
                int spawnPosY = Main.rand.Next(2) switch
                {
                    0 => 1200,
                    1 => -1200,
                    _ => 0
                };

                int spawnPosX = Main.rand.Next(2) switch
                {
                    0 => 1200,
                    1 => -1200,
                    _ => 0
                };


                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int npcID = NPC.NewNPC(NPC.GetSource_NaturalSpawn(), (i * 16) + spawnPosX, (j * 16) + spawnPosY, NPCID.Plantera);
                    Main.npc[npcID].netUpdate2 = true;
                }
                else if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    UltimateSkyblock.SpawnBossFromClient((byte)Main.LocalPlayer.whoAmI, NPCID.Plantera, (i * 16) + spawnPosX, (j * 16) + spawnPosY);
                }
            }

            return true;
        }

        public enum PacketID : byte
        {
            SpawnPlantera
        }

        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
        {
            int uniqueAnimationFrame = Main.tileFrame[Type];
            frameXOffset = uniqueAnimationFrame * 54; // replace this with your tile size (5 * 18 + 2 i think)
        }

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            if (++frameCounter >= 10)
            {
                frameCounter = 0;
                frame = ++frame % 4;
            }
        }

        public override void MouseOverFar(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = -1;
            player.cursorItemIconText = "Summon Plantera";

        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = -1;
            player.cursorItemIconText = "Summon Plantera";
        }
    }
}
