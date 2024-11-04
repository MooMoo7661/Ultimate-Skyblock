using Terraria.Enums;
using Terraria.ObjectData;
using UltimateSkyblock.Content.Items.Placeable.Objects;
using UltimateSkyblock.Content.UI.MapDrawing;

namespace UltimateSkyblock.Content.GlobalClasses
{
    public class GlobalMapIconTile : GlobalTile
    {
        public override void SetStaticDefaults()
        {
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.Width = 3;
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 18 };
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<GolemAltarMapIconEntity>().Hook_AfterPlacement, -1, 0, false);
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.addTile(TileID.LihzahrdAltar);

            ItemID.Sets.ShimmerTransformToItem[ItemID.Waldo] = ModContent.ItemType<SunshinePaintingItem>();

            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.Width = 2;
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.Table, TileObjectData.newTile.Width, 0);
            TileObjectData.addTile(TileID.Heart);
        }
    }

    public class GolemAltarMapIconEntity : ModTileEntity
    {
        private static Asset<Texture2D> golem;

        public override void Load()
        {
            golem = ModContent.Request<Texture2D>("UltimateSkyblock/Content/UI/MapDrawing/Icons/IconGolemAltar");
        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                int width = 3;
                int height = 5;
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
            return tile.HasTile && tile.TileType == TileID.LihzahrdAltar;
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
        }
    }
}
