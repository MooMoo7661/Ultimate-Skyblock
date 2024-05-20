using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using UltimateSkyblock.Content.Subworlds;
using UltimateSkyblock.Content.Tiles.Environment.Foliage;
using UltimateSkyblock.Content.Items.Placeable.Tiles;
using UltimateSkyblock.Content.Items.Generic;
using Terraria.ObjectData;
using Terraria.Enums;

namespace UltimateSkyblock.Content.Tiles.Furniture;

public class GlimmerTonicDecoration : ModTile
{
    public override void SetStaticDefaults()
    {
        Main.tileNoAttach[Type] = true;
        Main.tileLavaDeath[Type] = true;
        Main.tileFrameImportant[Type] = true;
        Main.tileNoFail[Type] = true;
        Main.tileSolid[Type] = false;
        
        TileID.Sets.AvoidedByMeteorLanding[Type] = true;

        DustType = DustID.Glass;

        HitSound = SoundID.Shatter;
        RegisterItemDrop(ModContent.ItemType<GlimmerTonic>());
        RegisterItemDrop(ModContent.ItemType<GlimmerTonic>(), 0);
        RegisterItemDrop(ModContent.ItemType<GlimmerTonic>(), 1);

        //TileObjectData.newTile.FullCopyFrom(TileID.Bottles);
        TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);
        TileObjectData.newTile.DrawYOffset = 0;
        TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
        TileObjectData.newTile.UsesCustomCanPlace = true;
        TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.PlatformNonHammered | AnchorType.SolidWithTop, 1, 0);
        TileObjectData.addTile(Type);

        AddMapEntry(new Color(63, 31, 24));
    }
}
