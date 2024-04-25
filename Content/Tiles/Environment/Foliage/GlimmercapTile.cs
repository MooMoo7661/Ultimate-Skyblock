using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.Metadata;
using Terraria.ID;
using Terraria.Localization;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using UltimateSkyblock.Content.Tiles.Blocks;
using Terraria.DataStructures;
using UltimateSkyblock.Content.Items.Generic;

namespace UltimateSkyblock.Content.Tiles.Environment.Foliage
{
    public class GlimmercapTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileWaterDeath[Type] = true;
            Main.tileObsidianKill[Type] = true;
            Main.tileCut[Type] = true;
            Main.tileLighted[Type] = true;
            TileMaterials.SetForTileId(Type, TileMaterials._materialsByName["Plant"]);
            Main.tileSpelunker[Type] = true;
            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(121, 84, 229), name);

            RegisterItemDrop(ModContent.ItemType<Glimmercap>(), 1);
            RegisterItemDrop(ModContent.ItemType<Glimmercap>());
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.WaterDeath = true;
            TileObjectData.newTile.AnchorValidTiles = new int[] { ModContent.TileType<DeepstoneTile>() };
            TileObjectData.addTile(Type);

            HitSound = SoundID.Grass;
            DustType = DustID.Ambient_DarkBrown;
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 121f / 255;
            g = 84f / 255;
            b = 229f / 255;
        }
    }
}
