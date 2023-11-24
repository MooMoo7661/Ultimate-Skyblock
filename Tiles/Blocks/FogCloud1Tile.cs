using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ObjectData;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System;
using OneBlock.Items.Placeable;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Enums;

namespace OneBlock.Tiles.Blocks
{
    public class FogCloud1Tile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileMerge[Type][Type] = true;
            DustType = DustID.Cloud;
            RegisterItemDrop(ModContent.ItemType<FogCloud1>());
            Main.tileSolid[Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<FogCloud1TE>().Hook_AfterPlacement, -1, 0, false);
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.addTile(Type);
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            ModContent.GetInstance<FogCloud1TE>().Kill(i, j);
            noItem = true;
        }

        public override bool CanPlace(int i, int j)
        {
            return true;
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            return ModContent.GetInstance<OneBlockModConfig>().RenderFogCloudTiles;
        }
    }

    public class FogCloud1TE : ModTileEntity
    {
        public int timer = 0;
        public override bool IsTileValidForEntity(int x, int y)
        {
            Tile tile = Main.tile[x, y];

            return tile.HasTile && tile.TileType == ModContent.TileType<FogCloud1Tile>();
        }
        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                NetMessage.SendData(msgType: MessageID.TileEntityPlacement, number: i, number2: j, number3: Type);
            return Place(i, j);
        }

        public override void OnNetPlace()
        {
            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendData(MessageID.TileEntitySharing, number: ID, number2: Position.X, number3: Position.Y);
        }

        public override void Update()
        {
            timer++;
            if (timer >= 30)
            {
                int i = Position.X;
                int j = Position.Y;

                Vector2 vector = new Point(i, j).ToWorldCoordinates();
                int type = 1202;
                float scale = 8f + Main.rand.NextFloat() * 1.6f;
                Vector2 position = vector + new Vector2(0f, -18f);
                Vector2 velocity = Main.rand.NextVector2Circular(0.7f, 0.25f) * 0.4f + Main.rand.NextVector2CircularEdge(1f, 0.4f) * 0.1f;
                velocity *= 4f;
                Gore.NewGorePerfect(new EntitySource_TileUpdate(i, j), position, velocity, type, scale);
                timer = 0;
            }
        }
    }
}