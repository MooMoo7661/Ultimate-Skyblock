using Terraria.Enums;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using UltimateSkyblock.Content.Configs;
using UltimateSkyblock.Content.StoneGenerator;
using static UltimateSkyblock.Content.Tiles.Blocks.PlanteraAltar;
using System.IO;
using SubworldLibrary;
using UltimateSkyblock.Content.DaySystem;

namespace UltimateSkyblock
{
    public class UltimateSkyblock : Mod
	{
        public static UltimateSkyblock Instance { get; private set; }
        public override void Load()
        {
            Instance = this;

            On_WorldGen.ShakeTree += On_WorldGen_ShakeTree;

            if (ModContent.GetInstance<SubworldConfig>().PlacementDetours)
            {
                On_WorldGen.PlaceTile += On_WorldGen_PlaceTile;
                On_WorldGen.PlaceObject += On_WorldGen_PlaceObject;
            }

            OB_Liquid.Load();
        }

        public override void Unload()
        {
            Instance = null;
        }

        private bool On_WorldGen_PlaceObject(On_WorldGen.orig_PlaceObject orig, int x, int y, int type, bool mute, int style, int alternate, int random, int direction)
        {
            return orig(x, y, type, mute || Main.gameMenu, style, alternate, random, direction);
        }

        private bool On_WorldGen_PlaceTile(On_WorldGen.orig_PlaceTile orig, int i, int j, int Type, bool mute, bool forced, int plr, int style)
        {
            return orig(i, j, Type, mute || Main.gameMenu, forced, plr, style);
        }

        private void On_WorldGen_ShakeTree(On_WorldGen.orig_ShakeTree orig, int i, int j)
        {
            orig(i, j);

            var config = ModContent.GetInstance<SkyblockModConfig>();

            if (!config.TreesDropMoreAcorns)
            {
                return;
            }

            WorldGen.GetTreeBottom(i, j, out int x, out int y);

            TreeTypes treeType = WorldGen.GetTreeType(Main.tile[x, y].TileType);

            if (treeType == TreeTypes.None)
            {
                return;
            }

            if (!Main.dedServ)
            {
                y--;
                while (y > 10 && Main.tile[x, y].HasTile && TileID.Sets.IsShakeable[(int)Main.tile[x, y].TileType]) { y--; }

                y++;

                if (!WorldGen.IsTileALeafyTreeTop(x, y) || Collision.SolidTiles(x - 2, x + 2, y - 2, y + 2))
                {
                    return;
                }

                if (Main.rand.NextBool(8))
                {
                   Item.NewItem(WorldGen.GetItemSource_FromTileBreak(x, y), new Vector2(x * 16, y * 16), Vector2.Zero, ItemID.Acorn, Stack: 1);
                }
            }
        }

        // Thank god SpritMod was open source
        // This code was "borrowed" from => https://github.com/GabeHasWon/SpiritMod/blob/367e1da73022ec8741673b4bfbc629c3798a04e4/SpiritMultiplayer.cs
        // Purpose of this is to spawn a boss from right clicking a tile, which is called client side only.

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            var id = (PacketID)reader.ReadByte();
            byte player;
            switch (id)
            {
                case PacketID.SpawnPlantera:
                    if (Main.netMode == NetmodeID.Server)
                    {
                        player = reader.ReadByte();
                        int bossType = reader.ReadInt32();
                        int spawnX = reader.ReadInt32();
                        int spawnY = reader.ReadInt32();

                        if (NPC.AnyNPCs(bossType))
                            return;

                        int npcID = NPC.NewNPC(NPC.GetSource_NaturalSpawn(), spawnX, spawnY, bossType);
                        Main.npc[npcID].netUpdate2 = true;
                    }
                    break;
            }
        }

        public static ModPacket WriteToPacket(ModPacket packet, byte msg, params object[] param)
        {
            packet.Write(msg);

            for (int m = 0; m < param.Length; m++)
            {
                object obj = param[m];
                if (obj is bool) packet.Write((bool)obj);
                else if (obj is byte) packet.Write((byte)obj);
                else if (obj is int) packet.Write((int)obj);
                else if (obj is float) packet.Write((float)obj);
                else if (obj is double) packet.Write((double)obj);
                else if (obj is short) packet.Write((short)obj);
                else if (obj is ushort) packet.Write((ushort)obj);
                else if (obj is sbyte) packet.Write((sbyte)obj);
                else if (obj is uint) packet.Write((uint)obj);
                else if (obj is decimal) packet.Write((decimal)obj);
                else if (obj is long) packet.Write((long)obj);
                else if (obj is string) packet.Write((string)obj);
            }
            return packet;
        }

        public static void SpawnBossFromClient(byte whoAmI, int type, int x, int y) => WriteToPacket(Instance.GetPacket(), (byte)PacketID.SpawnPlantera, whoAmI, type, x, y).Send();
    }
}