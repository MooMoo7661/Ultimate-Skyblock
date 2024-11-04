using Terraria.Audio;
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
    public class ForestBiomeCore : ModTile
    {
        public override void SetStaticDefaults()
        {
            // Properties
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileFrameImportant[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;

            DustType = DustID.Grass;

            RegisterItemDrop(ModContent.ItemType<ForestBiomeCoreItem>(), 1);
            RegisterItemDrop(ModContent.ItemType<ForestBiomeCoreItem>());

            // Placement
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.newTile.DrawYOffset = 6;
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<ForestMapMarkerEntity>().Hook_AfterPlacement, -1, 0, true);
            TileObjectData.addTile(Type);

            // Etc
            AddMapEntry(new Color(30, 94, 20), Language.GetText("Mods.UltimateSkyblock.Tiles.ForestMarker.MapEntry"));
        }
    }

    public class ForestMapMarkerEntity : ModTileEntity
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
            return tile != null && tile.HasTile && tile.TileType == ModContent.TileType<ForestBiomeCore>();
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
                int x = Position.X + Main.rand.Next(-8, 11);
                int y = Position.Y + Main.rand.Next(-8, 11);
                Tile tile = Framing.GetTileSafely(x, y);
                if (tile.HasTile && Main.tileSolid[tile.TileType])
                {
                    int type = tile.TileType switch
                    {
                        TileID.Crimstone or TileID.Ebonstone or TileID.Pearlstone => TileID.Stone,
                        TileID.CorruptGrass or TileID.CrimsonGrass or TileID.HallowedGrass => TileID.Grass,
                        TileID.Crimsand or TileID.Ebonsand or TileID.Pearlsand => TileID.Sand,
                        TileID.CrimsonSandstone or TileID.CorruptSandstone or TileID.HallowSandstone => TileID.Sandstone,
                        TileID.FleshIce or TileID.CorruptIce or TileID.HallowedIce => TileID.IceBlock,
                        TileID.CrimsonJungleGrass or TileID.CorruptJungleGrass => TileID.JungleGrass,
                        TileID.CrimsonHardenedSand or TileID.CorruptHardenedSand or TileID.HallowHardenedSand => TileID.HardenedSand,
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

