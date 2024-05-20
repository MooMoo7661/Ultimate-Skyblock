using System.Text.RegularExpressions;
using SubworldLibrary;
using Terraria;
using Terraria.Audio;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.Localization;
using Terraria.ObjectData;
using UltimateSkyblock.Content.Items.Placeable;
using UltimateSkyblock.Content.NPCs;
using UltimateSkyblock.Content.StoneGenerator;
using UltimateSkyblock.Content.Subworlds;
using UltimateSkyblock.Content.Tiles.Blocks;
using UltimateSkyblock.Content.UI.MapDrawing;
using static UltimateSkyblock.Content.Subworlds.MiningSubworld;

namespace UltimateSkyblock.Content.Tiles.Furniture
{
    public class CoffinTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            // Properties
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileFrameImportant[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;
            TileID.Sets.IgnoredByNpcStepUp[Type] = true;
            TileID.Sets.PreventsSandfall[Type] = true;
            TileID.Sets.AvoidedByMeteorLanding[Type] = true;

            DustType = DustID.Stone;            

            // Placement
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.Width = 4;
            TileObjectData.newTile.Height = 7;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.DrawYOffset = 0;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16, 16, 16, 16 };
            TileObjectData.addTile(Type);

            RegisterItemDrop(ItemID.StoneBlock);
            RegisterItemDrop(ItemID.StoneBlock, 1);
            RegisterItemDrop(ItemID.StoneBlock, 0);
            MineResist = 5f;

            // Etc
            AddMapEntry(new Color(69, 43, 28), Language.GetText("Mods.UltimateSkyblock.Tiles.DungeonDoor"));
        }

        public override bool RightClick(int i, int j)
        {
            WorldGen.KillTile(i, j);

            return true;
        }

        public override bool CreateDust(int i, int j, ref int type)
        {
            return false;
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            
            noItem = true;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            var topLeft = TileUtils.GetTopLeftTileInMultitile(i, j);
            for (int x = topLeft.X; x < topLeft.X + 4; x++)
            {
                for (int y = topLeft.Y; y < topLeft.Y + 7; y++)
                {
                    Main.tile[x, y].Clear(TileDataType.Tile);
                }
            }

            Vector2 pos = new(topLeft.X + 2.5f, topLeft.Y + 5f);
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                int npcID = NPC.NewNPC(Entity.GetSource_NaturalSpawn(), (int)pos.X * 16, (int)pos.Y * 16, ModContent.NPCType<ReDead>());
                Main.npc[npcID].netUpdate2 = true;
            }
            else if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                UltimateSkyblock.SpawnBossFromClient((byte)Main.LocalPlayer.whoAmI, ModContent.NPCType<ReDead>(), (int)pos.X * 16, (int)pos.Y * 16);
            }
        }
    }
}

