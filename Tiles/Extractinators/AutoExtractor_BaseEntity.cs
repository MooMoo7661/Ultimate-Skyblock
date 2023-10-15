using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria;
using Microsoft.Xna.Framework;

namespace OneBlock.Tiles.Extractinators
{
    public abstract class AutoExtractor_BaseEntity : ModTileEntity
    {
        protected abstract int Timer { get; }
        protected abstract int TileToBeValidOn { get; }
        protected abstract int LootMultiplier { get; }
        protected abstract int ConsumeMultiplier { get; }

        protected abstract bool HighPlatinum { get; }


        private int timer = 0;
        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                // Sync the entire multitile's area.  Modify "width" and "height" to the size of your multitile in tiles
                int width = 2;
                int height = 2;
                NetMessage.SendTileSquare(Main.myPlayer, i, j, width, height);

                // Sync the placement of the tile entity with other clients
                // The "type" parameter refers to the tile type which placed the tile entity, so "Type" (the type of the tile entity) needs to be used here instead
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
            return tile.TileType == TileToBeValidOn;
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
            try
            {
                Tile tile = Framing.GetTileSafely(Position.X, Position.Y);
                int left = Position.X;
                int top = Position.Y;
                if (tile.TileFrameX % 36 != 0)
                {
                    left--;
                }

                if (tile.TileFrameY != 0)
                {
                    top--;
                }

                Chest chest = Main.chest[Chest.FindChest(left, top)];

                timer++;
                if (timer >= Timer)
                {
                    int chest2 = Chest.FindChest(Position.X - 2, Position.Y);
                    for (int z = 0; z < ConsumeMultiplier; z++)
                    {
                        for (int i = 0; i < chest.item.Length; i++)
                        {
                            if (chest.item[i].type != ItemID.None && ItemID.Sets.ExtractinatorMode[chest.item[i].type] != -1)
                            {
                                if (chest.item[i].stack > 0)
                                    chest.item[i].stack -= 1;


                                if (chest.item[i].stack <= 0)
                                    chest.item[i].TurnToAir();

                                Extract(ItemID.Sets.ExtractinatorMode[chest.item[i].type], out int type, out int stack);

                                TryDepositToChest(type, stack * LootMultiplier);

                                break;
                            }
                        }
                    }

                    timer = 0;
                }

                Dust dust = Dust.NewDustPerfect(new Vector2(Position.X * 16 - 32, Position.Y * 16), DustID.GemSapphire);
                dust.noGravity = true;
                dust.velocity *= 0;
            }
            catch
            {

            }

        }
        public static bool NullOrAir(Item item) => item?.IsAir ?? true;
        public void TryDepositToChest(int itemType, int stack)
        {
            if (itemType <= ItemID.None)
                return;

            if (stack <= 0)
                return;

            int chest = Chest.FindChest(Position.X - 2, Position.Y);
            Point extractorWordCoordinates = Position.ToWorldCoordinates().ToPoint();
            Item[] inv = chest > 0 ? Main.chest[chest].item : null;
            while (stack > 0)
            {
                Item item = new(itemType, stack);
                int itemStack = stack;
                if (item.stack > item.maxStack)
                {
                    itemStack = item.maxStack;
                    itemStack -= item.maxStack;
                }

                if (!Deposit(inv, ref item, out int _))
                {
                    int number = Item.NewItem(null, extractorWordCoordinates.X, extractorWordCoordinates.Y, 1, 1, item.type, item.stack, noBroadcast: false, -1);

                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        NetMessage.SendData(MessageID.SyncItem, -1, -1, null, number, 1f);
                    }
                }

                stack -= itemStack - item.stack;
            }
        }
        public static bool Deposit(Item[] inv, ref Item item, out int index)
        {
            if (NullOrAir(item))
            {
                index = inv.Length;
                return false;
            }

            if (item.favorited)
            {
                index = inv.Length;
                return false;
            }

            if (Restock(inv, ref item, out index))
                return true;

            index = 0;
            while (index < inv.Length && !inv[index].IsAir)
            {
                index++;
            }

            if (index == inv.Length)
                return false;

            inv[index] = item.Clone();
            if (item.stack == item.maxStack)
                DoCoins(inv, index);

            item.TurnToAir();

            return true;
        }
        public static bool Restock(Item[] inv, ref Item item, out int index)
        {
            for (int i = 0; i < inv.Length; i++)
            {
                Item bagItem = inv[i];
                if (!NullOrAir(bagItem) && bagItem.type == item.type && bagItem.stack < bagItem.maxStack)
                {
                    if (ItemLoader.TryStackItems(bagItem, item, out _))
                    {
                        if (item.stack < 1)
                        {
                            item.TurnToAir();
                            index = i;
                            if (bagItem.stack == bagItem.maxStack)
                                DoCoins(inv, i);

                            return true;
                        }
                        else
                        {
                            DoCoins(inv, i);
                        }
                    }
                }
            }

            index = inv.Length;
            return false;
        }

        public static void DoCoins(Item[] inv, int slot)
        {
            Item item = inv[slot];
            if (item.type < ItemID.CopperCoin || item.type > ItemID.GoldCoin)
                return;

            if (item.stack != 100)
                return;

            item.SetDefaults(item.type + 1);
            for (int i = 0; i < inv.Length; i++)
            {
                Item coin = inv[i];
                if (IsTheSameAs(item, coin) && i != slot && coin.stack < coin.maxStack)
                {
                    coin.stack++;
                    item.TurnToAir(true);
                    item.active = false;
                    DoCoins(inv, i);

                    break;
                }
            }
        }
        private static bool IsTheSameAs(Item item, Item compareItem)
        {
            if (item.netID == compareItem.netID)
                return item.type == compareItem.type;

            return false;
        }
        public void Extract(int extractType, out int type, out int stack)
        {
            // Stolen vanilla code lmao

            int num = 5000;
            int num2 = 25;
            int num3 = 50;
            int num4 = -1;
            if (extractType == 1)
            {
                num /= 3;
                num2 *= 2;
                num3 = 20;
                num4 = 10;
            }
            int num5 = -1;
            int num6 = 1;
            if (num4 != -1 && Main.rand.NextBool(num4))
            {
                num5 = 3380;
                if (Main.rand.NextBool(5))
                {
                    num6 += Main.rand.Next(2);
                }
                if (Main.rand.NextBool(10))
                {
                    num6 += Main.rand.Next(3);
                }
                if (Main.rand.NextBool(15))
                {
                    num6 += Main.rand.Next(4);
                }
            }
            else if (Main.rand.NextBool(2))
            {
                int platinumChance = 12000;

                if (HighPlatinum)
                    platinumChance = 40;

                if (Main.rand.NextBool(platinumChance))
                {
                    num5 = 74;
                    if (Main.rand.NextBool(14))
                    {
                        num6 += Main.rand.Next(0, 2);
                    }
                    if (Main.rand.NextBool(14))
                    {
                        num6 += Main.rand.Next(0, 2);
                    }
                    if (Main.rand.NextBool(14))
                    {
                        num6 += Main.rand.Next(0, 2);
                    }
                }
                else if (Main.rand.NextBool(800))
                {
                    num5 = 73;
                    if (Main.rand.NextBool(6))
                    {
                        num6 += Main.rand.Next(1, 21);
                    }
                    if (Main.rand.NextBool(6))
                    {
                        num6 += Main.rand.Next(1, 21);
                    }
                    if (Main.rand.NextBool(6))
                    {
                        num6 += Main.rand.Next(1, 21);
                    }
                    if (Main.rand.NextBool(6))
                    {
                        num6 += Main.rand.Next(1, 21);
                    }
                    if (Main.rand.NextBool(6))
                    {
                        num6 += Main.rand.Next(1, 20);
                    }
                }
                else if (Main.rand.NextBool(60))
                {
                    num5 = 72;
                    if (Main.rand.NextBool(4))
                    {
                        num6 += Main.rand.Next(5, 26);
                    }
                    if (Main.rand.NextBool(4))
                    {
                        num6 += Main.rand.Next(5, 26);
                    }
                    if (Main.rand.NextBool(4))
                    {
                        num6 += Main.rand.Next(5, 26);
                    }
                    if (Main.rand.NextBool(4))
                    {
                        num6 += Main.rand.Next(5, 25);
                    }
                }
                else
                {
                    num5 = 71;
                    if (Main.rand.NextBool(3))
                    {
                        num6 += Main.rand.Next(10, 26);
                    }
                    if (Main.rand.NextBool(3))
                    {
                        num6 += Main.rand.Next(10, 26);
                    }
                    if (Main.rand.NextBool(3))
                    {
                        num6 += Main.rand.Next(10, 26);
                    }
                    if (Main.rand.NextBool(3))
                    {
                        num6 += Main.rand.Next(10, 25);
                    }
                }
            }
            else if (num != -1 && Main.rand.NextBool(num))
            {
                num5 = 1242;
            }
            else if (num2 != -1 && Main.rand.NextBool(num2))
            {
                num5 = Main.rand.Next(6) switch
                {
                    0 => 181,
                    1 => 180,
                    2 => 177,
                    3 => 179,
                    4 => 178,
                    _ => 182,
                };
                if (Main.rand.NextBool(20))
                {
                    num6 += Main.rand.Next(0, 2);
                }
                if (Main.rand.NextBool(30))
                {
                    num6 += Main.rand.Next(0, 3);
                }
                if (Main.rand.NextBool(40))
                {
                    num6 += Main.rand.Next(0, 4);
                }
                if (Main.rand.NextBool(50))
                {
                    num6 += Main.rand.Next(0, 5);
                }
                if (Main.rand.NextBool(60))
                {
                    num6 += Main.rand.Next(0, 6);
                }
            }
            else if (num3 != -1 && Main.rand.NextBool(num3))
            {
                num5 = 999;
                if (Main.rand.NextBool(20))
                {
                    num6 += Main.rand.Next(0, 2);
                }
                if (Main.rand.NextBool(30))
                {
                    num6 += Main.rand.Next(0, 3);
                }
                if (Main.rand.NextBool(40))
                {
                    num6 += Main.rand.Next(0, 4);
                }
                if (Main.rand.NextBool(50))
                {
                    num6 += Main.rand.Next(0, 5);
                }
                if (Main.rand.NextBool(60))
                {
                    num6 += Main.rand.Next(0, 6);
                }
            }
            else if (Main.rand.NextBool(3))
            {
                if (Main.rand.NextBool(5000))
                {
                    num5 = 74;
                    if (Main.rand.NextBool(10))
                    {
                        num6 += Main.rand.Next(0, 3);
                    }
                    if (Main.rand.NextBool(10))
                    {
                        num6 += Main.rand.Next(0, 3);
                    }
                    if (Main.rand.NextBool(10))
                    {
                        num6 += Main.rand.Next(0, 3);
                    }
                    if (Main.rand.NextBool(10))
                    {
                        num6 += Main.rand.Next(0, 3);
                    }
                    if (Main.rand.NextBool(10))
                    {
                        num6 += Main.rand.Next(0, 3);
                    }
                }
                else if (Main.rand.NextBool(400))
                {
                    num5 = 73;
                    if (Main.rand.NextBool(5))
                    {
                        num6 += Main.rand.Next(1, 21);
                    }
                    if (Main.rand.NextBool(5))
                    {
                        num6 += Main.rand.Next(1, 21);
                    }
                    if (Main.rand.NextBool(5))
                    {
                        num6 += Main.rand.Next(1, 21);
                    }
                    if (Main.rand.NextBool(5))
                    {
                        num6 += Main.rand.Next(1, 21);
                    }
                    if (Main.rand.NextBool(5))
                    {
                        num6 += Main.rand.Next(1, 20);
                    }
                }
                else if (Main.rand.NextBool(30))
                {
                    num5 = 72;
                    if (Main.rand.NextBool(3))
                    {
                        num6 += Main.rand.Next(5, 26);
                    }
                    if (Main.rand.NextBool(3))
                    {
                        num6 += Main.rand.Next(5, 26);
                    }
                    if (Main.rand.NextBool(3))
                    {
                        num6 += Main.rand.Next(5, 26);
                    }
                    if (Main.rand.NextBool(3))
                    {
                        num6 += Main.rand.Next(5, 25);
                    }
                }
                else
                {
                    num5 = 71;
                    if (Main.rand.NextBool(2))
                    {
                        num6 += Main.rand.Next(10, 26);
                    }
                    if (Main.rand.NextBool(2))
                    {
                        num6 += Main.rand.Next(10, 26);
                    }
                    if (Main.rand.NextBool(2))
                    {
                        num6 += Main.rand.Next(10, 26);
                    }
                    if (Main.rand.NextBool(2))
                    {
                        num6 += Main.rand.Next(10, 25);
                    }
                }
            }
            else
            {
                num5 = Main.rand.Next(8) switch
                {
                    0 => 12,
                    1 => 11,
                    2 => 14,
                    3 => 13,
                    4 => 699,
                    5 => 700,
                    6 => 701,
                    _ => 702,
                };
                if (Main.rand.NextBool(20))
                {
                    num6 += Main.rand.Next(0, 2);
                }
                if (Main.rand.NextBool(30))
                {
                    num6 += Main.rand.Next(0, 3);
                }
                if (Main.rand.NextBool(40))
                {
                    num6 += Main.rand.Next(0, 4);
                }
                if (Main.rand.NextBool(50))
                {
                    num6 += Main.rand.Next(0, 5);
                }
                if (Main.rand.NextBool(60))
                {
                    num6 += Main.rand.Next(0, 6);
                }

            }

            if (num5 > 0)
            {
                type = num5;
                stack = num6;
                return;
            }

            type = -1;
            stack = 0;
        }
    }
}
