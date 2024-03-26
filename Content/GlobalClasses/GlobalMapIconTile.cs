using Terraria.ObjectData;
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
        }
    }

    public class GolemAltarMapIconEntity : ModTileEntity
    {
        private MapIcon icon;

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

            Texture2D golem = ModContent.Request<Texture2D>("UltimateSkyblock/Content/UI/MapDrawing/Icons/IconGolemAltar").Value;
            icon = new MapIcon(new(Position.X + 1.5f, Position.Y), golem, Color.White, 1.1f, 0.8f, "Lihzahrd Altar");
            TileIconDrawing.icons.Add(icon);
        }

        public override void OnKill()
        {
            TileIconDrawing.icons.Remove(icon);
        }
    }
}
