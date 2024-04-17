using SubworldLibrary;
using Terraria.Audio;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.Localization;
using Terraria.ObjectData;
using UltimateSkyblock.Content.Items.Placeable;
using UltimateSkyblock.Content.Subworlds;
using UltimateSkyblock.Content.Tiles.Blocks;
using UltimateSkyblock.Content.UI.MapDrawing;

namespace UltimateSkyblock.Content.Tiles.Furniture
{
    public class DungeonDoor : ModTile
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

            DustType = DustID.t_LivingWood;            

            // Placement
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.Width = 7;
            TileObjectData.newTile.Height = 11;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.DrawYOffset = 0;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16 };
            TileObjectData.addTile(Type);

            RegisterItemDrop(ItemID.Wood);
            RegisterItemDrop(ItemID.Wood, 1);

            // Etc
            AddMapEntry(new Color(69, 43, 28), Language.GetText("Mods.UltimateSkyblock.Tiles.DungeonDoor"));
        }

        //public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        //{
        //    fail = true;
        //    noItem = true;
        //}

        public override bool RightClick(int i, int j)
        {
            Main.NewText("Dungeon Subworld coming soon!", Color.IndianRed);
            //SubworldSystem.Enter<DungeonSubworld>();
            return true;
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.cursorItemIconID = ItemID.Skull;
            player.cursorItemIconText = "";
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
        }
    }
}

