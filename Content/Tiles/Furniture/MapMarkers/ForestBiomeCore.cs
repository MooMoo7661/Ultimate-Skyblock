using Terraria.Audio;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.Localization;
using Terraria.ObjectData;
using UltimateSkyblock.Content.Items.Placeable;
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
            TileID.Sets.IgnoredByNpcStepUp[Type] = true;
            TileID.Sets.PreventsSandfall[Type] = true;
            TileID.Sets.AvoidedByMeteorLanding[Type] = true;

            DustType = DustID.Grass;

            // Placement
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.newTile.Width = 3;
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 18 };
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<ForestMapMarkerEntity>().Hook_AfterPlacement, -1, 0, false);
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.addTile(Type);

            // Etc
            AddMapEntry(new Color(30, 94, 20), Language.GetText("Mods.UltimateSkyblock.Tiles.ForestMarker.MapEntry"));
        }
    }

    public class ForestMapMarkerEntity : ModTileEntity
    {
        private MapIcon icon;
        private static Asset<Texture2D> forest;

        public override void Load()
        {
            forest = ModContent.Request<Texture2D>("UltimateSkyblock/Content/UI/MapDrawing/Icons/IconForest");
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
            return tile.HasTile && tile.TileType == ModContent.TileType<ForestBiomeCore>();
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

            icon = new MapIcon(new(Position.X + 1.5f, Position.Y), forest.Value, Color.White, 1.1f, 0.8f, "Forest Marker");
            TileIconDrawing.icons.Add(icon);


            if (Main.rand.NextBool(20))
            {
                int x = Position.X + Main.rand.Next(-8, 8);
                int y = Position.Y + Main.rand.Next(-8, 8);
                Tile tile = Framing.GetTileSafely(x, y);
                if (tile.HasTile)
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

                    WorldGen.PlaceTile(x, y, type, true, true);
                }
            }
        }

        public override void OnKill()
        {
            TileIconDrawing.icons.Remove(icon);
        }
    }
}

