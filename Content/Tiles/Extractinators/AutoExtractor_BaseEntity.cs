using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using UltimateSkyblock.Content.Utils;
using System.Linq;
using Terraria.ObjectData;
using System.IO;

namespace UltimateSkyblock.Content.Tiles.Extractinators
{
    public abstract class AutoExtractor_BaseEntity : ModTileEntity
    {
        protected abstract int Timer { get; }
        protected abstract int TileToBeValidOn { get; }
        protected abstract int LootMultiplier { get; }
        protected abstract int ConsumeMultiplier { get; }

        private int timer = 0;
		private int sendInfoTimer = sendInfoTimerWait;
		private const int sendInfoTimerWait = 60 * 5;
		public static Point16 TilePositionToMultiTileTopLeft(int x, int y, int multiTileWidth = 48, int multiTileHeight = 48) {
			Tile tile = Main.tile[x, y];
			int left = x;
			int top = y;
			int localTileFrameX = tile.TileFrameX % multiTileWidth;
			if (localTileFrameX != 0) {
				left -= localTileFrameX / 18;
			}

            int localTileFrameY = tile.TileFrameY % multiTileHeight;
			if (localTileFrameY != 0) {
				top -= localTileFrameY / 18;
			}

            return new Point16(left, top);
		}
        public static bool TryGetChest(Point16 topLeft, out int chest) => TryGetChest(topLeft.X, topLeft.Y, out chest);
        public static bool TryGetChest(int x, int y, out int chestId) {
            chestId = Chest.FindChest(x, y);
            if (chestId > -1)
                return true;

            return false;
		}
        public bool TryGetMyChest(out int chest) => TryGetChest(Position.X, Position.Y, out chest);
        private static int T1 {
            get {
                if (t1 == -1)
                    t1 = ModContent.TileType<AutoExtractorTier1Tile>();

                return t1;
            }
        }
		private static int t1 = -1;
		private static int T2 {
			get {
				if (t2 == -1)
					t2 = ModContent.TileType<AutoExtractorTier2Tile>();

				return t2;
			}
		}
		private static int t2 = -1;
		private static int T3 {
			get {
				if (t3 == -3)
					t3 = ModContent.TileType<AutoExtractorTier3Tile>();

				return t3;
			}
		}
		private static int t3 = -3;
		private static int T4 {
			get {
				if (t4 == -4)
					t4 = ModContent.TileType<AutoExtractorTier4Tile>();

				return t4;
			}
		}
		private static int t4 = -4;
		private static int T5 {
			get {
				if (t5 == -5)
					t5 = ModContent.TileType<AutoExtractorTier5Tile>();

				return t5;
			}
		}
		private static int t5 = -5;
        public static bool AutoExtractorTileType(int tileType) => tileType >= T1 && tileType <= T5;
		public static bool ValidTileTypeForStorageChest(int tileType) {
            switch (tileType) {
                case TileID.Containers:
                case TileID.Containers2:
                case TileID.Dressers:
                    return true;
                default:
                    if (AutoExtractorTileType(tileType))
                        return false;

                    return true;
            }
        }
		public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate)
        {
			if (Main.netMode == NetmodeID.MultiplayerClient) {
				NetMessage.SendTileSquare(Main.myPlayer, i, j, 3, 3);
				NetMessage.SendData(MessageID.TileEntityPlacement, number: i, number2: j, number3: Type);
				return -1;
			}

			int placedEntity = Place(i, j);
			return placedEntity;
		}

        public override bool IsTileValidForEntity(int x, int y)
        {
            var tile = Main.tile[x, y];
            return tile.TileType == TileToBeValidOn;
        }

        private int GetChestID() => Chest.FindChest(Position.X, Position.Y);
		public override void OnNetPlace() => NetMessage.SendData(MessageID.TileEntitySharing, number: ID, number2: Position.X, number3: Position.Y);
        private List<Chest> storageChests = new();
		public override void Update()
        {
            timer++;
            if (timer >= Timer)
            {
				int[] chestPositionXOffsets = new int[] { -2, 3 };
				int[] chestPositionYOffsets = new int[] { 1, 0 };
				storageChests = new();
				for (int i = 0; i < chestPositionXOffsets.Length; i++) {
					int xOffset = chestPositionXOffsets[i];
					for (int j = 0; j < chestPositionYOffsets.Length; j++) {
						int yOffset = chestPositionYOffsets[j];
						int chestX = Position.X + xOffset;
						int chestY = Position.Y + yOffset;
						int chestId = Chest.FindChest(chestX, chestY);
						if (chestId > -1) {
							Tile tile = Main.tile[chestX, chestY];
							if (tile.HasTile && ValidTileTypeForStorageChest(tile.TileType)) {
								storageChests.Add(Main.chest[chestId]);
								break;
							}
						}
					}
				}

				int chest = GetChestID();
				if (chest != -1)
                {
					Chest extractorChest = Main.chest[chest];
					Point extractorWordCoordinates = Position.ToWorldCoordinates().ToPoint();
					int i = 0;
					for (int z = 0; z < ConsumeMultiplier; z++) {
						for (; i < extractorChest.item.Length; i++) {
							if (extractorChest.item[i].type != ItemID.None && ItemID.Sets.ExtractinatorMode[extractorChest.item[i].type] != -1) {
								if (extractorChest.item[i].stack > 0)
									extractorChest.item[i].stack -= 1;

								if (extractorChest.item[i].stack <= 0)
									extractorChest.item[i].TurnToAir();

								ExtractionItem.AutoExtractinatorUse(ItemID.Sets.ExtractinatorMode[extractorChest.item[i].type], TileToBeValidOn, out int type, out int stack);

								TryDepositToChest(storageChests.Select(c => c.item), type, stack * LootMultiplier, extractorWordCoordinates);

								break;
							}
						}
					}
				}
                else {
					ModContent.GetInstance<UltimateSkyblock>().Logger.Info($"Failed to find chest for the AutoExtractor at ({Position.X}, {Position.Y})");
				}

                timer = 0;
            }

			if (Main.netMode == NetmodeID.Server) {
				sendInfoTimer++;
				if (sendInfoTimer >= sendInfoTimerWait) {
					sendInfoTimer = 0;
					foreach (Chest chest in storageChests) {
						Item[] inv = chest.item;
						inv.PercentFull(out float stackPercentFull, out float slotsPercentFull);
						ChestIndicatorInfo.WriteAndSendChestLocation(chest.x, chest.y, stackPercentFull, slotsPercentFull);
					}
				}
			}
        }

        public static void TryDepositToChest(IEnumerable<Item[]> inventories, int itemType, int stack, Point extractorWordCoordinates)
        {
            if (itemType <= ItemID.None)
                return;

            if (stack <= 0)
                return;

            while (stack > 0)
            {
                Item item = new(itemType, stack);
                int itemStack = stack;
                if (item.stack > item.maxStack)
                {
                    item.stack = item.maxStack;
                    itemStack = item.stack;
                }

                bool deposited = false;
                foreach (Item[] inv in inventories)
                {
                    if (inv == null)
                        continue;

                    if (inv.Deposit(ref item, out int _))
                    {
                        deposited = true;
                        break;
                    }
                }

                if (!deposited)
                {
                    int number = Item.NewItem(null, extractorWordCoordinates.X, extractorWordCoordinates.Y, 1, 1, item.type, item.stack, noBroadcast: false, -1);

                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        NetMessage.SendData(MessageID.SyncItem, -1, -1, null, number, 1f);
                    }

                    item.stack = 0;
                }

                stack -= itemStack - item.stack;
            }
        }
    }

	public struct ChestIndicatorInfo {
		public float StackPercentFull;
		public float SlotsPercentFull;
		private static byte FloatToByte(float f) => (byte)(f * (float)byte.MaxValue);
		private static float ByteToFloat(byte b) => (float)b / (float)byte.MaxValue;
		public static void WriteAndSendChestLocation(int x, int y, float stackPercentFull, float slotsPercentFull) {
			ModPacket packet = UltimateSkyblock.Instance.GetPacket();
			packet.Write((byte)UltimateSkyblock.PacketId.ChestIndicatorInfo);
			packet.Write(x);
			packet.Write(y);
			packet.Write(FloatToByte(stackPercentFull));
			packet.Write(FloatToByte(slotsPercentFull));
			packet.Send();
		}
		public static void Read(BinaryReader reader) {
			Point chestLocation = new Point(reader.ReadInt32(), reader.ReadInt32());
			ChestIndicatorInfo info = new ChestIndicatorInfo(reader);
			if (GlobalChest.ChestPercentFullInfo.ContainsKey(chestLocation)) {
				GlobalChest.ChestPercentFullInfo[chestLocation] = info;
			}
			else {
				GlobalChest.ChestPercentFullInfo.Add(chestLocation, info);
			}
		}
		private ChestIndicatorInfo(BinaryReader reader) {
			StackPercentFull = ByteToFloat(reader.ReadByte());
			SlotsPercentFull = ByteToFloat(reader.ReadByte());
		}
	}

	public class GlobalChest : GlobalTile {
		private static Asset<Texture2D> chestIndicatorTexture;
		public override void Load() {
			chestIndicatorTexture = ModContent.Request<Texture2D>("UltimateSkyblock/Content/Tiles/Extractinators/ExtractinatorIndicatorDot");
		}

		public static Dictionary<Point, ChestIndicatorInfo> ChestPercentFullInfo = new();
		public override void PostDraw(int i, int j, int type, SpriteBatch spriteBatch) {
            if (!AutoExtractor_BaseEntity.ValidTileTypeForStorageChest(type))
                return;

            Tile tile = Main.tile[i, j];
			if (!tile.HasTile)
				return;

			TileObjectData data = TileObjectData.GetTileData(tile);
			if (data == null)
				return;

			if (tile.TileFrameY % (data.Height * 18) != 0)
                return;

            int localFrameX = tile.TileFrameX % (data.Width * 18);
			if (localFrameX > 18)
                return;

			int chestX = i;
            if (localFrameX == 0) {
                //Left
                if (i <= 0)
                    return;

                Tile left = Main.tile[i - 1, j];
                if (!AutoExtractor_BaseEntity.AutoExtractorTileType(left.TileType))
                    return;
            }
            else {
                //Right
                if (i >= Main.maxTilesX - 1)
                    return;

                Tile right = Main.tile[i + 1 ,j];
				if (!AutoExtractor_BaseEntity.AutoExtractorTileType(right.TileType))
					return;

				chestX--;
			}

			if (!TryGetChestFill(chestX, j, out float stackPercentFull, out float slotsPercentFull))
				return;
			
			Vector2 center = new Point(chestX + 1, j).ToWorldCoordinates(0f, 0f);
			float indicatorRadius = 5f;

			Vector2 zero = (Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange, Main.offScreenRange)) + new Vector2(-3f, -3f);
			Vector2 stackIndicatorPosition = center + new Vector2(-indicatorRadius, 0f);
			Color stackColor = GetChestDustColor(stackPercentFull, 1f);
			spriteBatch.Draw(chestIndicatorTexture.Value, stackIndicatorPosition - Main.screenPosition + zero, stackColor);

			Vector2 slotsIndicatorPosition = center + new Vector2(indicatorRadius, 0f);
			Color slotsColor = GetChestDustColor(slotsPercentFull, 1f);
			spriteBatch.Draw(chestIndicatorTexture.Value, slotsIndicatorPosition - Main.screenPosition + zero, slotsColor);
		}
		private bool TryGetChestFill(int x, int y, out float stackPercentFull, out float slotsPercentFull) {
			if (Main.netMode == NetmodeID.MultiplayerClient) {
				if (ChestPercentFullInfo.TryGetValue(new Point(x, y), out ChestIndicatorInfo info)) {
					stackPercentFull = info.StackPercentFull;
					slotsPercentFull = info.SlotsPercentFull;
					return true;
				}
			}
			else {
				if (AutoExtractor_BaseEntity.TryGetChest(x, y, out int chestId)) {
					Chest chest = Main.chest[chestId];
					Item[] inv = chest.item;
					inv.PercentFull(out stackPercentFull, out slotsPercentFull);
					return true;
				}
			}

			stackPercentFull = 0f;
			slotsPercentFull = 0f;
			return false;
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
	}

	//Don't Delete.  For testing chests and AutoExtractors - andro951
	/*
    public class TestingModSystem : ModSystem {
		public override void PostUpdateEverything() {
			for (int i = 0; i < Main.chest.Length; i++) {
                Chest chest = Main.chest[i];
                if (chest == null || chest.item == null)
                    continue;

                Point chestPosition = new(chest.x, chest.y);
                Dust testDust = Dust.NewDustPerfect(chestPosition.ToWorldCoordinates(), ModContent.DustType<ExtractinatorDust>(), Vector2.Zero, newColor: Color.Yellow);
                testDust.noGravity = true;
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
	*/
}
