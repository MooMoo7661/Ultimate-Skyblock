﻿using Terraria.Audio;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.Localization;
using Terraria.ObjectData;
using UltimateSkyblock.Content.Items.Placeable;
using UltimateSkyblock.Content.Tiles.Blocks;
using UltimateSkyblock.Content.UI.MapDrawing;

namespace UltimateSkyblock.Content.Tiles.Furniture.MapMarkers
{
    public class SnowBiomeCore : ModTile
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

            DustType = DustID.SnowBlock;

            // Placement
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.newTile.Width = 3;
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 18 };
            //TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<SnowBiomeMapMarkerEntity>().Hook_AfterPlacement, -1, 0, false);
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.addTile(Type);

            // Etc
            AddMapEntry(new Color(179, 242, 233), Language.GetText("Mods.UltimateSkyblock.Tiles.SnowMarker.MapEntry"));
        }
    }

    public class SnowBiomeMapMarkerEntity : ModTileEntity
    {
        private MapIcon icon;
        private static Asset<Texture2D> snow;

        public override void Load()
        {
            snow = ModContent.Request<Texture2D>("UltimateSkyblock/Content/UI/MapDrawing/Icons/IconSnow");
        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                int width = 3;
                int height = 3;
                NetMessage.SendTileSquare(Main.myPlayer, i, j, width, height);
                NetMessage.SendData(MessageID.TileEntityPlacement, number: i, number2: j, number3: Type);
            }

            // ModTileEntity.Place() handles checking if the entity can be placed, then places it for you
            // Set "tileOrigin" to the same value you set TileObjectData.newTile.Origin to in the ModTile
            Point16 tileOrigin = new Point16(1, 1);
            int placedEntity = Place(i - tileOrigin.X, j - tileOrigin.Y);
            return placedEntity;
        }

        public override bool IsTileValidForEntity(int x, int y)
        {
            var tile = Main.tile[x, y];
            return tile.HasTile && tile.TileType == ModContent.TileType<SnowBiomeCore>();
        }


        public override void OnNetPlace()
        {
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.TileEntitySharing, number: ID, number2: Position.X, number3: Position.Y);
            }
        }

        public override void Update()
        {
            int i = Position.X;
            int j = Position.Y;
            if (!Framing.GetTileSafely(i, j).HasTile)
            {
                Kill(i, j);
            }

            icon = new MapIcon(new(Position.X + 1.5f, Position.Y), snow.Value, Color.White, 1.1f, 0.8f, "Snow Marker");
            TileIconDrawing.icons.Add(icon);

            if (Main.rand.NextBool(60))
            {
                int x = Position.X + Main.rand.Next(-8, 8);
                int y = Position.Y + Main.rand.Next(-8, 8);
                Tile tile = Framing.GetTileSafely(x, y);

                if (tile.HasTile)
                {
                    int type = tile.TileType switch
                    {
                        TileID.BreakableIce => TileID.IceBlock,
                        TileID.Dirt => TileID.SnowBlock,
                        _ => -1
                    };

                    if (type == -1)
                        return;

                    WorldGen.PlaceTile(x, y, type, true, true);
                }
                else if (tile.LiquidType == LiquidID.Water && tile.LiquidAmount >= 50)
                {
                    tile.LiquidAmount = 0;
                    WorldGen.PlaceTile(x, y, TileID.BreakableIce, true, true);
                }
            }
        }

        public override void OnKill()
        {
            TileIconDrawing.icons.Remove(icon);
        }
    }
}

