using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using UltimateSkyblock.Content.Tiles.Blocks;

namespace UltimateSkyblock.Content.ModSystems
{
    public class TileMerging : ModSystem
    {
        public override void Load()
        {
            Main.tileMerge[TileID.Hellstone][TileID.Ash] = true;
            Main.tileMerge[TileID.Ash][ModContent.TileType<VolcanicStoneTile>()] = true;
            Main.tileMerge[TileID.Stone][ModContent.TileType<VolcanicStoneTile>()] = true;
            Main.tileMerge[TileID.LavaMoss][ModContent.TileType<VolcanicStoneTile>()] = true;
            Main.tileMerge[TileID.AshGrass][ModContent.TileType<VolcanicStoneTile>()] = true;

            TileID.Sets.PreventsSandfall[TileID.Sand] = true;
            TileID.Sets.PreventsSandfall[TileID.Slush] = true;
        }
    }
}
