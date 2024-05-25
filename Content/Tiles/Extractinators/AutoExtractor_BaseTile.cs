using System;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TileFunctionLibrary.API;

namespace UltimateSkyblock.Content.Tiles.Extractinators
{
    public abstract class AutoExtractor_BaseTile : ModTile, IExtractinatorTile
    {
        protected abstract string TilesheetPath { get; }
        protected abstract int ExtractorTile { get; }
        protected abstract ModTileEntity Entity { get; }
        protected abstract Func<int> MyItemType { get; }
        
        private bool TryGetEntity(int x, int y, out AutoExtractor_BaseEntity entity) {
            Point16 topLeft = AutoExtractor_BaseEntity.TilePositionToMultiTileTopLeft(x, y);
            return TryGetEntity(topLeft, out entity);
		}
        private bool TryGetEntityChest(int x, int y, out int chest) => AutoExtractor_BaseEntity.TryGetChest(AutoExtractor_BaseEntity.TilePositionToMultiTileTopLeft(x, y), out chest);

		private bool TryGetEntity(Point16 position, out AutoExtractor_BaseEntity entity) {
            if (TileEntity.ByPosition.TryGetValue(position, out TileEntity tileEntity)) {
                if (tileEntity is AutoExtractor_BaseEntity autoExtractorEntity) {
                    entity = autoExtractorEntity;
                    return true;
                }
            }

			entity = null;
            return false;
		}
		public override void SetStaticDefaults()
        {
            // Properties
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileContainer[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.HasOutlines[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;
            TileID.Sets.BasicChest[Type] = true;
            TileID.Sets.InteractibleByNPCs[Type] = true;
            TileID.Sets.IsAContainer[Type] = true;
            TileID.Sets.HasOutlines[Type] = false;

            DustType = DustID.Stone;
            AdjTiles = new int[] { TileID.Extractinator };
            // Names
            AddMapEntry(new Color(200, 200, 200), CreateMapEntryName());

            Func<int, int, int, int, int, int, int> postPlacementHook = (i, j, type, style, direction, alternate) =>
            {
                Point16 topLeft = AutoExtractor_BaseEntity.TilePositionToMultiTileTopLeft(i, j);
                int x = topLeft.X;
                int y = topLeft.Y;
                int chestId = Chest.AfterPlacement_Hook(i, j, type, style, direction, alternate);
                int entityReturn = Entity.Hook_AfterPlacement(x, y, type, style, direction, alternate);
                if (entityReturn == -1)
                    chestId = -1;

                return chestId;
            };

			// Placement
			AnimationFrameHeight = 54;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(postPlacementHook, -1, 0, false);
            TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);
        }
        public override LocalizedText DefaultContainerName(int frameX, int frameY)
        {
            return CreateMapEntryName();
        }

        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
        {
            return true;
        }

        public override void ModifySmartInteractCoords(ref int width, ref int height, ref int frameWidth, ref int frameHeight, ref int extraY)
        {
            width = 3;
            height = 1;
            extraY = 0;
        }

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            frame = Main.tileFrame[TileID.Extractinator];
            frameCounter = Main.tileFrameCounter[TileID.Extractinator];
        }

        public override bool RightClick(int i, int j)
        {
            Point16 topLeft = AutoExtractor_BaseEntity.TilePositionToMultiTileTopLeft(i, j);
            int x = topLeft.X;
            int y = topLeft.Y;

            Player player = Main.LocalPlayer;
            Main.mouseRightRelease = false;

            player.CloseSign();
            player.SetTalkNPC(-1);
            Main.npcChatCornerItem = 0;
            Main.npcChatText = "";
            if (Main.editChest)
            {
                SoundEngine.PlaySound(SoundID.MenuTick);
                Main.editChest = false;
                Main.npcChatText = string.Empty;
            }

            if (player.editedChestName)
            {
                NetMessage.SendData(MessageID.SyncPlayerChest, -1, -1, NetworkText.FromLiteral(Main.chest[player.chest].name), player.chest, 1f);
                player.editedChestName = false;
            }

            bool isLocked = Chest.IsLocked(x, y);
            if (Main.netMode == NetmodeID.MultiplayerClient && !isLocked)
            {
                if (x == player.chestX && y == player.chestY && player.chest != -1)
                {
                    player.chest = -1;
                    Recipe.FindRecipes();
                    SoundEngine.PlaySound(SoundID.MenuClose);
                }
                else
                {
                    NetMessage.SendData(MessageID.RequestChestOpen, -1, -1, null, x, y);
                    Main.stackSplit = 600;
                }
            }
            else
            {
                if (isLocked)
                {
                    //Chest for the AutoExtractors should never be locked.  Force it to unlock.
                    if (Chest.Unlock(x, y))
                    {
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                        {
                            NetMessage.SendData(MessageID.LockAndUnlock, -1, -1, null, player.whoAmI, 1f, x, y);
                        }
                    }
                }
                else
                {
                    if (AutoExtractor_BaseEntity.TryGetChest(topLeft, out int chestId)) {
						Main.stackSplit = 600;
						if (chestId == player.chest) {
							player.chest = -1;
							SoundEngine.PlaySound(SoundID.MenuClose);
						}
						else {
							SoundEngine.PlaySound(player.chest < 0 ? SoundID.MenuOpen : SoundID.MenuTick);
							player.OpenChest(x, y, chestId);
						}

						Recipe.FindRecipes();
					}
				}
            }

            return true;
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            Texture2D texture = ModContent.Request<Texture2D>(TilesheetPath).Value;

            // If you are using ModTile.SpecialDraw or PostDraw or PreDraw, use this snippet and add zero to all calls to spriteBatch.Draw
            // The reason for this is to accommodate the shift in drawing coordinates that occurs when using the different Lighting mode
            // Press Shift+F9 to change lighting modes quickly to verify your code works for all lighting modes
            Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);

            // Because height of third tile is different we change it
            int height = tile.TileFrameY % AnimationFrameHeight == 36 ? 18 : 16;

            // Offset along the Y axis depending on the current frame
            int frameYOffset = Main.tileFrame[Type] * AnimationFrameHeight;

            // Firstly we draw the original texture and then glow mask texture
            spriteBatch.Draw(
                texture,
                new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y + 2) + zero,
                new Rectangle(tile.TileFrameX, tile.TileFrameY + frameYOffset, 16, height),
                Lighting.GetColor(i, j), 0f, default, 1f, SpriteEffects.None, 0f);
            // Make sure to draw with Color.White or at least a color that is fully opaque
            // Achieve opaqueness by increasing the alpha channel closer to 255. (lowering closer to 0 will achieve transparency)

            // Return false to stop vanilla draw
            return false;
        }
		public override void MouseOverFar(int i, int j) {
			Player player = Main.LocalPlayer;
			player.cursorItemIconText = "";
		}

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
			player.cursorItemIconID = MyItemType();
			player.cursorItemIconText = "";
        }
		public override void KillMultiTile(int i, int j, int frameX, int frameY) {
			Entity.Kill(i, j);

			if (!TryGetEntityChest(i, j, out int chestID))
				return;

			Chest chest = Main.chest[chestID];

			if (!Chest.DestroyChest(chest.x, chest.y)) {
				EntitySource_TileBreak source = new EntitySource_TileBreak(i, j, "Breaking AutoExtractor");
				for (int k = 0; k < chest.item.Length; k++) {
					Item.NewItem(source, i * 16, j * 16, 1, 1, chest.item[k].type, chest.item[k].stack, noBroadcast: false, -1);
					chest.item[k].TurnToAir();
				}

				Chest.DestroyChestDirect(chest.x, chest.y, chestID);
			}
		}
		public override bool CanKillTile(int i, int j, ref bool blockDamaged) {
            if (TryGetEntityChest(i, j, out int chestId)) {
                Chest chest = Main.chest[chestId];
				bool canDestroyChest = Chest.CanDestroyChest(chest.x, chest.y);
				return canDestroyChest;
			}

            return true;
		}
    }
}
