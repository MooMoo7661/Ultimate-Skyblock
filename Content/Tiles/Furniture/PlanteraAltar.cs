using System.IO;
using Terraria;
using Terraria.Enums;
using Terraria.Localization;
using Terraria.Map;
using Terraria.ModLoader.Default;
using Terraria.ObjectData;
using UltimateSkyblock.Content.Items.Placeable;
using UltimateSkyblock.Content.Tiles.Environment;
using UltimateSkyblock.Content.UI.MapDrawing;
using static System.Net.Mime.MediaTypeNames;

namespace UltimateSkyblock.Content.Tiles.Furniture
{
    public class PlanteraAltar : ModTile
    {
        public override void SetStaticDefaults()
        {
            // Properties
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileFrameImportant[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;
            TileID.Sets.IgnoredByNpcStepUp[Type] = true;
            TileID.Sets.PreventsSandfall[Type] = true;
            TileID.Sets.AvoidedByMeteorLanding[Type] = true;
            TileID.Sets.PreventsTileReplaceIfOnTopOfIt[Type] = true;
            TileID.Sets.PreventsTileRemovalIfOnTopOfIt[Type] = true;

            DustType = DustID.Mud;

            // Placement
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
            TileObjectData.newTile.Width = 3;
            TileObjectData.newTile.Height = 5;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16, 18 };
            //TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<PlanteraAltarMapIconEntity>().Hook_AfterPlacement, -1, 0, false);
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.addTile(Type);

            // Etc
            AddMapEntry(new Color(255, 88, 255), Language.GetText("Mods.UltimateSkyblock.Tiles.PlanteraAltar.MapEntry"));
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        { 
            fail = true;
            noItem = true;
        }

        public override bool RightClick(int i, int j)
        {
            if (!Main.hardMode)
                return false;

            bool plantera = false;
            for (int k = 0; k < Main.maxNPCs; k++)
            {
                NPC npc = Main.npc[k];
                if (npc != null)
                {
                    if (npc.type == NPCID.Plantera && npc.active)
                    {
                        plantera = true;
                    }
                }
            }

            if (!plantera)
            {
                int spawnPosY = Main.rand.Next(2) switch
                {
                    0 => 1200,
                    1 => -1200,
                    _ => 0
                };

                int spawnPosX = Main.rand.Next(2) switch
                {
                    0 => 1200,
                    1 => -1200,
                    _ => 0
                };


                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int npcID = NPC.NewNPC(Entity.GetSource_NaturalSpawn(), i * 16 + spawnPosX, j * 16 + spawnPosY, NPCID.Plantera);
                    Main.npc[npcID].netUpdate2 = true;
                }
                else if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    UltimateSkyblock.SpawnBossFromClient((byte)Main.LocalPlayer.whoAmI, NPCID.Plantera, i * 16 + spawnPosX, j * 16 + spawnPosY);
                }
            }

            return true;
        }

        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
        {
            int uniqueAnimationFrame = Main.tileFrame[Type];
            frameXOffset = uniqueAnimationFrame * 54; // replace this with your tile size (5 * 18 + 2 i think)
        }

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            if (++frameCounter >= 10)
            {
                frameCounter = 0;
                frame = ++frame % 4;
            }
        }

        public override void MouseOverFar(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = -1;
            player.cursorItemIconText = "Summon Plantera";

        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = -1;
            player.cursorItemIconText = "Summon Plantera";
        }
    }

    public class PlanteraAltarMapIconEntity : ModTileEntity
    {
        private MapIcon icon;
        private static Asset<Texture2D> plantera;

        public override void Load()
        {
            plantera = Mod.Assets.Request<Texture2D>("Content/UI/Guidebook/Assets/PlanteraSWIcon");
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
            return tile.HasTile && tile.TileType == ModContent.TileType<PlanteraAltar>();
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

            icon = new MapIcon(new(Position.X + 1.5f, Position.Y), plantera.Value, Color.White, 1.1f, 0.8f, "Plantera Altar");
            TileIconDrawing.icons.Add(icon);
        }

        public override void OnKill()
        {
            TileIconDrawing.icons.Remove(icon);
        }
    }
}
