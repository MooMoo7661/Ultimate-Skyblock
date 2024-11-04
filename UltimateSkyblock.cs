using Terraria.Enums;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using UltimateSkyblock.Content.Configs;
using UltimateSkyblock.Content.StoneGenerator;
using static UltimateSkyblock.Content.Tiles.Furniture.PlanteraAltar;
using System.IO;
using SubworldLibrary;
using UltimateSkyblock.Content.DaySystem;
using UltimateSkyblock.Content.ModSystems;
using Terraria.ModLoader.IO;
using UltimateSkyblock.Content.Tiles.Extractinators;

namespace UltimateSkyblock
{
    public class UltimateSkyblock : Mod
    {
        public static UltimateSkyblock Instance { get; private set; }

        public static bool IsSkyblock()
        {
            Main.ActiveWorldFileData.TryGetHeaderData(ModContent.GetInstance<WorldSelectSkyblockIcon>(), out var data);

            return Main.ActiveWorldFileData.IsValid || data.GetBool("GeneratedWithSkyblock");
        }

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
            if (!IsSkyblock())
                return;

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

                if (Main.rand.NextBool(16))
                {
                    Item.NewItem(WorldGen.GetItemSource_FromTileBreak(x, y), new Vector2(x * 16, y * 16), Vector2.Zero, ItemID.Acorn, Stack: 1);
                }
            }
        }
        public enum PacketId {
            ChestIndicatorInfo,
            SpawnPlantera,
            SendIconDraw
        }
		public override void HandlePacket(BinaryReader reader, int whoAmI) {
			PacketId packetId = (PacketId)reader.ReadByte();
            switch (packetId) {
                case PacketId.ChestIndicatorInfo:
                    ChestIndicatorInfo.Read(reader);
					break;
                case PacketId.SpawnPlantera:
                    if (Main.netMode == NetmodeID.Server)
                    {
                        byte player = reader.ReadByte();
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
                if (obj is bool boolObj) packet.Write(boolObj);
                else if (obj is byte byteObj) packet.Write(byteObj);
                else if (obj is int intObj) packet.Write(intObj);
                else if (obj is float floatObj) packet.Write(floatObj);
                else if (obj is double doubleObj) packet.Write(doubleObj);
                else if (obj is short shortObj) packet.Write(shortObj);
                else if (obj is ushort ushortObj) packet.Write(ushortObj);
                else if (obj is sbyte sbyteObj) packet.Write(sbyteObj);
                else if (obj is uint uintObj) packet.Write(uintObj);
                else if (obj is decimal decimalObj) packet.Write(decimalObj);
                else if (obj is long longObj) packet.Write(longObj);
                else if (obj is string stringObj) packet.Write(stringObj);
            }
            return packet;
        }

        public static void SpawnBossFromClient(byte whoAmI, int type, int x, int y) => WriteToPacket(Instance.GetPacket(), (int)PacketId.SpawnPlantera, whoAmI, type, x, y).Send();
    }
}