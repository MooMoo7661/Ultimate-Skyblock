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
using OneBlock.Utility;

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
			Tile tile = Framing.GetTileSafely(Position.X, Position.Y);
			int left = Position.X;
			int top = Position.Y;
			if (tile.TileFrameX % 36 != 0) {
				left--;
			}

			if (tile.TileFrameY != 0) {
				top--;
			}

			int[] chestPositionXOffsets = new int[] { -2, 3 };
			Item[][] storageChests = new Item[chestPositionXOffsets.Length][];
			for (int i = 0; i < chestPositionXOffsets.Length; i++) {
				int xOffset = chestPositionXOffsets[i];
				int chest = Chest.FindChest(Position.X + xOffset, Position.Y);
				storageChests[i] = chest >= 0 ? Main.chest[chest].item : null;
			}

			timer++;
			if (timer >= Timer) {
				Chest extractorChest = Main.chest[Chest.FindChest(left, top)];
				Point extractorWordCoordinates = Position.ToWorldCoordinates().ToPoint();
				int i = 0;
				for (int z = 0; z < ConsumeMultiplier; z++) {
					for (; i < extractorChest.item.Length; i++) {
						if (extractorChest.item[i].type != ItemID.None && ItemID.Sets.ExtractinatorMode[extractorChest.item[i].type] != -1) {
							if (extractorChest.item[i].stack > 0)
								extractorChest.item[i].stack -= 1;

							if (extractorChest.item[i].stack <= 0)
								extractorChest.item[i].TurnToAir();

							Extract(ItemID.Sets.ExtractinatorMode[extractorChest.item[i].type], out int type, out int stack);

							TryDepositToChest(storageChests, type, stack * LootMultiplier, extractorWordCoordinates);

							break;
						}
					}
				}

				timer = 0;
			}

			for (int i = 0; i < chestPositionXOffsets.Length; i++) {
				Item[] inv = storageChests[i];
				if (inv == null)
					continue;

				inv.PercentFull(out float stackPercentFull, out float slotsPercentFull);
				int xOffset = chestPositionXOffsets[i];
				Vector2 center = (Position + new Point16(1 + xOffset, 0)).ToWorldCoordinates(0f, 0f);
				float dustRadius = 5f;
				Vector2 velocity = Vector2.Zero;

				Vector2 stackDustPostion = center + new Vector2(-dustRadius, 0f);
				Color stackColor = GetChestDustColor(stackPercentFull, 1f);
				Dust stackDust = Dust.NewDustPerfect(stackDustPostion, DustID.Marble, velocity, newColor: stackColor);
				stackDust.noGravity = true;

				Vector2 slotsDustPosition = center + new Vector2(dustRadius, 0f);
				Color slotsColor = GetChestDustColor(slotsPercentFull, 1f);
				Dust slotsDust = Dust.NewDustPerfect(slotsDustPosition, DustID.Marble, velocity, newColor: slotsColor);
				slotsDust.noGravity = true;
			}

		}
        private static Color GetChestDustColor(float percentFull, float alpha) {
            float red;
            float green;
            if (percentFull < 0.5f) {
                green = 1f;
                red = percentFull * 2f;
            }
            else {
				red = 1f;
                if (percentFull == 1f) {
                    green = 0f;
                }
                else {
                    green = 1f - (percentFull - 0.5f) * 1.6f + 0.2f;
                }
			}

            return new Color(red, green, 0f, alpha);
		}

		public static void TryDepositToChest(IEnumerable<Item[]> inventories, int itemType, int stack, Point extractorWordCoordinates) {
			if (itemType <= ItemID.None)
				return;

			if (stack <= 0)
				return;

			while (stack > 0) {
				Item item = new(itemType, stack);
				int itemStack = stack;
				if (item.stack > item.maxStack) {
					item.stack = item.maxStack;
                    itemStack = item.stack;
				}

                bool deposited = false;
                foreach (Item[] inv in inventories) {
					if (inv == null)
						continue;

					if (inv.Deposit(ref item, out int _)) {
						deposited = true;
						break;
					}
				}

                if (!deposited) {
					int number = Item.NewItem(null, extractorWordCoordinates.X, extractorWordCoordinates.Y, 1, 1, item.type, item.stack, noBroadcast: false, -1);

					if (Main.netMode == NetmodeID.MultiplayerClient) {
						NetMessage.SendData(MessageID.SyncItem, -1, -1, null, number, 1f);
					}

					item.stack = 0;
				}

				stack -= itemStack - item.stack;
			}
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
