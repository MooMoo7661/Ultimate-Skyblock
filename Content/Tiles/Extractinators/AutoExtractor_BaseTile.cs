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
            AddMapEntry(new Color(200, 200, 200), CreateMapEntryName(), MapChestName);

            Func<int, int, int, int, int, int, int> postPlacementHook = (x, y, type, style, direction, alternate) =>
            {
                int location = Chest.AfterPlacement_Hook(x, y, type, style, direction, alternate);
                Entity.Hook_AfterPlacement(x, y, type, style, direction, alternate);

                return location;
            };

            // Placement
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(postPlacementHook, -1, 0, false);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 18 };
            TileObjectData.newTile.UsesCustomCanPlace = true;

            AnimationFrameHeight = 54;
            TileObjectData.newTile.DrawYOffset = 2;

            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);
        }

        public override bool CanPlace(int i, int j)
        {
            if (Framing.GetTileSafely(i, j).TileType == ExtractorTile)
            {
                return false;
            }

            return true;
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

        int drawFrame;

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            frame = Main.tileFrame[TileID.Extractinator];
            frameCounter = Main.tileFrameCounter[TileID.Extractinator];
        }

        public override bool RightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;
            Tile tile = Main.tile[i, j];
            Main.mouseRightRelease = false;
            int left = i;
            int top = j;
            if (tile.TileFrameX % 36 != 0)
            {
                left--;
            }

            if (tile.TileFrameY != 0)
            {
                top--;
            }

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

            bool isLocked = Chest.IsLocked(left, top);
            if (Main.netMode == NetmodeID.MultiplayerClient && !isLocked)
            {
                if (left == player.chestX && top == player.chestY && player.chest != -1)
                {
                    player.chest = -1;
                    Recipe.FindRecipes();
                    SoundEngine.PlaySound(SoundID.MenuClose);
                }
                else
                {
                    NetMessage.SendData(MessageID.RequestChestOpen, -1, -1, null, left, top);
                    Main.stackSplit = 600;
                }
            }
            else
            {
                if (isLocked)
                {
                    // Make sure to change the code in UnlockChest if you don't want the chest to only unlock at night.
                    int key = ItemID.GoldenKey;
                    if (player.ConsumeItem(key, includeVoidBag: true) && Chest.Unlock(left, top))
                    {
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                        {
                            NetMessage.SendData(MessageID.LockAndUnlock, -1, -1, null, player.whoAmI, 1f, left, top);
                        }
                    }
                }
                else
                {
                    int chest = Chest.FindChest(left, top);
                    if (chest != -1)
                    {
                        Main.stackSplit = 600;
                        if (chest == player.chest)
                        {
                            player.chest = -1;
                            SoundEngine.PlaySound(SoundID.MenuClose);
                        }
                        else
                        {
                            SoundEngine.PlaySound(player.chest < 0 ? SoundID.MenuOpen : SoundID.MenuTick);
                            player.OpenChest(left, top, chest);
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

        // This is not a hook, this is just a normal method used by the MouseOver and MouseOverFar hooks to avoid repeating code.
        public void MouseOverNearAndFarSharedLogic(Player player, int i, int j)
        {
            Tile tile = Main.tile[i, j];
            int left = i;
            int top = j;
            left -= tile.TileFrameX % 54 / 18;
            if (tile.TileFrameY % 36 != 0)
            {
                top--;
            }
            int chestIndex = Chest.FindChest(left, top);
            player.cursorItemIconID = -1;
            if (chestIndex < 0)
            {
                player.cursorItemIconText = "Auto-Extractor";
            }
            else
            {
                string defaultName = TileLoader.DefaultContainerName(tile.TileType, tile.TileFrameX, tile.TileFrameY); // This gets the ContainerName text for the currently selected language

                if (Main.chest[chestIndex].name != "")
                {
                    player.cursorItemIconText = Main.chest[chestIndex].name;
                }
                else
                {
                    player.cursorItemIconText = defaultName;
                }
                if (player.cursorItemIconText == defaultName)
                {
                    player.cursorItemIconID = ItemID.Chest;
                    player.cursorItemIconText = "";
                }
            }
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
        }

        public override void MouseOverFar(int i, int j)
        {
            Player player = Main.LocalPlayer;
            MouseOverNearAndFarSharedLogic(player, i, j);
            if (player.cursorItemIconText == "")
            {
                player.cursorItemIconEnabled = false;
                player.cursorItemIconID = 0;
            }
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            MouseOverNearAndFarSharedLogic(player, i, j);
            if (Main.tile[i, j].TileFrameY > 0)
            {
                player.cursorItemIconID = ItemID.Extractinator;
                player.cursorItemIconText = "";
            }
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            Entity.Kill(i, j);

            Tile tile = Framing.GetTileSafely(i, j);
            int left = i;
            int top = j;
            left -= tile.TileFrameX % 54 / 18;
            if (tile.TileFrameY % 36 != 0)
            {
                top--;
            }

            if (!Chest.DestroyChest(i, j))
            {
                Chest chest = Main.chest[Chest.FindChest(i, j)];
                for (int k = 0; k < chest.item.Length; k++)
                {
                    Item.NewItem(null, i * 16, j * 16, 1, 1, chest.item[k].type, chest.item[k].stack, noBroadcast: false, -1);
                    chest.item[k].TurnToAir();
                }
            }
        }
        //public override void KillMultiTile(int i, int j, int frameX, int frameY)
        //{
        //    Chest chest = Main.chest[Chest.FindChest(i, j)];

        //    if (chest != null)
        //    {
        //        for (int k = 0; k < chest.item.Length; k++)
        //        {
        //            Item.NewItem(null, i * 16, j * 16, 1, 1, chest.item[k].type, chest.item[k].stack, noBroadcast: false, -1);
        //            chest.item[k].TurnToAir();
        //        }
        //    }

        //    Chest.DestroyChest(i, j);

        //}

        public static string MapChestName(string name, int i, int j)
        {
            int left = i;
            int top = j;
            Tile tile = Main.tile[i, j];
            if (tile.TileFrameX % 36 != 0)
            {
                left--;
            }

            if (tile.TileFrameY != 0)
            {
                top--;
            }

            int chest = Chest.FindChest(left, top);
            if (chest < 0)
            {
                return "Auto-Extractor";
            }

            if (Main.chest[chest].name == "")
            {
                return name;
            }

            return name + ": " + Main.chest[chest].name;
        }
    }
}
