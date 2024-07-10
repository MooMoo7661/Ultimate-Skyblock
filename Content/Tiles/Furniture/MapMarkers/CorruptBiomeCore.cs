using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.Localization;
using Terraria.ObjectData;
using UltimateSkyblock.Content.Items.Placeable;
using UltimateSkyblock.Content.Items.Placeable.MapMarkers;
using UltimateSkyblock.Content.Tiles.Blocks;
using UltimateSkyblock.Content.UI.MapDrawing;

namespace UltimateSkyblock.Content.Tiles.Furniture.MapMarkers
{
    public class CorruptBiomeCore : ModTile
    {
        public override void SetStaticDefaults()
        {
            // Properties
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileFrameImportant[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;

            DustType = DustID.Corruption;

            RegisterItemDrop(ModContent.ItemType<CorruptBiomeCoreItem>(), 1);
            RegisterItemDrop(ModContent.ItemType<CorruptBiomeCoreItem>());

            // Placement
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.newTile.DrawYOffset = 6;
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<CorruptBiomeMapMarkerEntity>().Hook_AfterPlacement, -1, 0, true);
            TileObjectData.addTile(Type);

            // Etc
            AddMapEntry(new Color(56, 16, 97), Language.GetText("Mods.UltimateSkyblock.Tiles.CorruptMarker.MapEntry"));
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (!effectOnly)
                ModContent.GetInstance<CorruptBiomeMapMarkerEntity>().Kill(i, j);
            base.KillTile(i, j, ref fail, ref effectOnly, ref noItem);
        }
    }

    public class CorruptBiomeMapMarkerEntity : ModTileEntity
    {
        // This code is called as a hook when the player places the host tile so that the turret tile entity may be placed.
        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate)
        {
            // If in multiplayer, tell the server to place the tile entity and DO NOT place it yourself. That would mismatch IDs.
            // Also tell the server that you placed tiles in whatever space the host tile occupies.
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                NetMessage.SendTileSquare(Main.myPlayer, i, j, 3, 3);
                NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i, j, Type);
                return -1;
            }

            // If in single player, just place the tile entity, no problems.
            int id = Place(i, j);
            return id;
        }

        public override bool IsTileValidForEntity(int x, int y)
        {
            var tile = Main.tile[x, y];
            return tile != null && tile.HasTile && tile.TileType == ModContent.TileType<CorruptBiomeCore>();
        }

        public override void OnNetPlace() => NetMessage.SendData(MessageID.TileEntitySharing, number: ID, number2: Position.X, number3: Position.Y);

        public override void OnKill()
        {
            ChatHelper.SendChatMessageToClient(NetworkText.FromLiteral("removed"), Color.White, 0);
            TileIconDrawing.TEs.Remove(Position);
        }

        public override void Update()
        {
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.TileEntitySharing, number: ID, number2: Position.X, number3: Position.Y);
            }

            if (!TileIconDrawing.TEs.Exists(_ => _ == Position))
            {
                ChatHelper.SendChatMessageToClient(NetworkText.FromLiteral("added"), Color.White, 0);
                TileIconDrawing.TEs.Add(Position);
            }

            if (!Main.tile[Position.X, Position.Y].HasTile)
                Kill(Position.X, Position.Y);

            if (Main.rand.NextBool(8))
            {
                int x = Position.X + Main.rand.Next(-8, 11);
                int y = Position.Y + Main.rand.Next(-8, 11);
                Tile tile = Framing.GetTileSafely(x, y);
                if (tile.HasTile)
                {
                    int type = tile.TileType switch
                    {
                        TileID.Stone => TileID.Ebonstone,
                        TileID.Grass => TileID.CorruptGrass,
                        TileID.Sand => TileID.Ebonsand,
                        TileID.Sandstone => TileID.CorruptSandstone,
                        TileID.IceBlock => TileID.CorruptIce,
                        TileID.JungleGrass => TileID.CorruptJungleGrass,
                        TileID.HardenedSand => TileID.CorruptHardenedSand,
                        _ => -1
                    };

                    if (type == -1)
                        return;


                    Framing.GetTileSafely(x, y).TileType = (ushort)type;
                    if (Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.SendTileSquare(-1, x, y);
                    }
                }
            }
        }
    }
}

