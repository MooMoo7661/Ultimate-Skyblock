using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Enums;
using Terraria.ObjectData;

namespace UltimateSkyblock.Content.Tiles.Autominers
{
    public class AutoMiner_BaseTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = false;
            Main.tileSolid[Type] = true;

            DustType = DustID.Stone;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<AutoMinerEntity>().Hook_AfterPlacement, -1, 0, false);
            TileObjectData.addTile(Type);

        }
    }

    public class AutoMinerEntity : ModTileEntity
    {
        public override bool IsTileValidForEntity(int x, int y)
        {
            var tile = Main.tile[x, y];
            return tile.TileType == ModContent.TileType<AutoMiner_BaseTile>();
        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                NetMessage.SendTileSquare(Main.myPlayer, i, j, 1, 1);
                NetMessage.SendData(MessageID.TileEntityPlacement, number: i, number2: j, number3: Type);
                return -1;
            }

            int placedEntity = Place(i, j);
            return placedEntity;
        }

        int timer = 60;
        public override void Update()
        {
            int x = Position.ToWorldCoordinates().ToTileCoordinates().X;
            int y = Position.ToWorldCoordinates().ToTileCoordinates().Y;

            if (!Framing.GetTileSafely(x, y).HasTile)
                Kill(x, y);

            if (!Framing.GetTileSafely(x, y + 1).HasTile)
                return;

            Tile tile = Framing.GetTileSafely(x, y + 1);

            if (timer <= 0)
            {
                
            }
            else
                timer--;

        }
    }
}
