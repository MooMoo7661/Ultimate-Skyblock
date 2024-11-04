using System.Linq;
using Terraria.Audio;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.Localization;
using Terraria.ModLoader.Default;
using Terraria.ObjectData;
using UltimateSkyblock.Content.Items.Placeable;
using UltimateSkyblock.Content.Items.Placeable.MapMarkers;
using UltimateSkyblock.Content.Items.Placeable.Objects;
using UltimateSkyblock.Content.Tiles.Blocks;
using UltimateSkyblock.Content.UI.MapDrawing;

namespace UltimateSkyblock.Content.Tiles.Furniture.MapMarkers
{
    public class HallowBiomeCore : ModTile
    {
        public override void SetStaticDefaults()
        {
            // Properties
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileFrameImportant[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;

            DustType = DustID.HallowedPlants;

            RegisterItemDrop(ModContent.ItemType<HallowBiomeCoreItem>(), 1);
            RegisterItemDrop(ModContent.ItemType<HallowBiomeCoreItem>());

            // Placement
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.newTile.DrawYOffset = 6;
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<HallowBiomeMapMarkerEntity>().Hook_AfterPlacement, -1, 0, true);
            TileObjectData.addTile(Type);

            // Etc
            AddMapEntry(new Color(255, 0, 0), Language.GetText("Mods.UltimateSkyblock.Tiles.HallowMarker.MapEntry"));
        } 
    }


    public class HallowBiomeMapMarkerEntity : ModTileEntity
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
            return tile != null && tile.HasTile && tile.TileType == ModContent.TileType<HallowBiomeCore>();
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
                int x = Position.X + Main.rand.Next(-8, 8);
                int y = Position.Y + Main.rand.Next(-8, 8);
                Tile tile = Framing.GetTileSafely(x, y);
                if (tile.HasTile && Main.tileSolid[tile.TileType])
                {
                    int type = tile.TileType switch
                    {
                        TileID.Stone => TileID.Pearlstone,
                        TileID.Grass => TileID.HallowedGrass,
                        TileID.Sand => TileID.Pearlsand,
                        TileID.Sandstone => TileID.HallowSandstone,
                        TileID.IceBlock => TileID.HallowedIce,
                        TileID.HardenedSand => TileID.HallowHardenedSand,
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

