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
using System.Reflection;

namespace OneBlock.Tiles.Extractinators
{
    public abstract class AutoExtractor_BaseEntity : ModTileEntity
    {
        protected abstract int Timer { get; }
        protected abstract int TileToBeValidOn { get; }
        protected abstract int LootMultiplier { get; }
        protected abstract int ConsumeMultiplier { get; }

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

							ExtractionItem.AutoExtractinatorUse(ItemID.Sets.ExtractinatorMode[extractorChest.item[i].type], tile.TileType, out int type, out int stack);

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
				Dust stackDust = Dust.NewDustPerfect(stackDustPostion, ModContent.DustType<ExtractinatorDust>(), velocity, newColor: stackColor);
				stackDust.noGravity = true;

				Vector2 slotsDustPosition = center + new Vector2(dustRadius, 0f);
				Color slotsColor = GetChestDustColor(slotsPercentFull, 1f);
				Dust slotsDust = Dust.NewDustPerfect(slotsDustPosition, ModContent.DustType<ExtractinatorDust>(), velocity, newColor: slotsColor);
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
    }

    public class ExtractinatorDust : ModDust
    {
        public int timer = 0;
        public override bool Update(Dust dust)
        {
            timer++;
            if (timer >= 30)
            {
                dust.active = false;
            }
            return true;
        }

        public override void OnSpawn(Dust dust)
        {
            dust.scale = 1f;
            dust.noGravity = true;
        }

    }
}
