using Terraria.Audio;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.Localization;
using Terraria.ObjectData;
using UltimateSkyblock.Content.Items.Placeable;
using UltimateSkyblock.Content.Items.Placeable.MapMarkers;
using UltimateSkyblock.Content.Subworlds;
using UltimateSkyblock.Content.Tiles.Blocks;
using UltimateSkyblock.Content.UI.MapDrawing;

namespace UltimateSkyblock.Content.Tiles.Furniture.MapMarkers
{
    public class MushroomBiomeCore : ModTile
    {
        public override void SetStaticDefaults()
        {
            // Properties
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileFrameImportant[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;

            DustType = DustID.GlowingMushroom;

            RegisterItemDrop(ModContent.ItemType<MushroomBiomeCoreItem>(), 1);
            RegisterItemDrop(ModContent.ItemType<MushroomBiomeCoreItem>());

            // Placement
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.newTile.DrawYOffset = 6;
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<MushroomBiomeMapMarkerEntity>().Hook_AfterPlacement, -1, 0, true);
            TileObjectData.addTile(Type);

            // Etc
            AddMapEntry(new Color(5, 61, 181), Language.GetText("Mods.UltimateSkyblock.Tiles.MushroomMarker.MapEntry"));
        }
    }

    public class MushroomBiomeMapMarkerEntity : ModTileEntity
    {
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
            return tile != null && tile.HasTile && tile.TileType == ModContent.TileType<MushroomBiomeCore>();
        }

        public override void OnNetPlace() => NetMessage.SendData(MessageID.TileEntitySharing, number: ID, number2: Position.X, number3: Position.Y);

        public override void Update()
        {
            int i = Position.X;
            int j = Position.Y;
            if (!Framing.GetTileSafely(i, j).HasTile)
            {
                Kill(i, j);
            }

            if (Main.rand.NextBool(8))
            {
                int x = Position.X + Main.rand.Next(-35, 38);
                int y = Position.Y + Main.rand.Next(-20, 23);
                Tile tile = Framing.GetTileSafely(x, y);

                if (tile.HasTile && Main.tileSolid[tile.TileType])
                {
                    GenUtils.GetSurroundingTiles(x, y, out Tile left, out Tile right, out Tile top, out Tile bottom);
                    if ((tile.TileType == TileID.Mud && (!left.HasTile || !right.HasTile || !top.HasTile || !bottom.HasTile)) || tile.TileType == TileID.JungleGrass)
                    {
                        Framing.GetTileSafely(x, y).TileType = TileID.MushroomGrass;
                        if (Main.netMode == NetmodeID.Server)
                        {
                            NetMessage.SendTileSquare(-1, x, y);
                        }
                    }
                }
            }
        }
    }
}

