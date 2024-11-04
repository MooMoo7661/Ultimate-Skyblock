using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using UltimateSkyblock.Content.Tiles.Blocks;
using static UltimateSkyblock.Utils.FramingUtils;

namespace UltimateSkyblock.Content.ModSystems
{
    public class TileMerging : ModSystem
    {
        public override void SetStaticDefaults()
        {
            SetupTileMerge(TileID.Hellstone, TileID.Ash);
            SetupTileMerge(ModContent.TileType<DeepstoneBrickTile>(), ModContent.TileType<DeepstoneTile>());
            SetupTileMerge(ModContent.TileType<DeepstoneBrickTile>(), ModContent.TileType<HardenedDeepstoneTile>());

            SetupTileMerge(TileID.Adamantite, ModContent.TileType<DeepstoneTile>());
            SetupTileMerge(TileID.Titanium, ModContent.TileType<DeepstoneTile>());
            SetupTileMerge(TileID.Cobalt, ModContent.TileType<DeepstoneTile>());
            SetupTileMerge(TileID.Palladium, ModContent.TileType<DeepstoneTile>());
            SetupTileMerge(TileID.Mythril, ModContent.TileType<DeepstoneTile>());
            SetupTileMerge(TileID.Orichalcum, ModContent.TileType<DeepstoneTile>());

            SetupTileMerge(TileID.Adamantite, ModContent.TileType<HardenedDeepstoneTile>());
            SetupTileMerge(TileID.Titanium, ModContent.TileType<HardenedDeepstoneTile>());
            SetupTileMerge(TileID.Cobalt, ModContent.TileType<HardenedDeepstoneTile>());
            SetupTileMerge(TileID.Palladium, ModContent.TileType<HardenedDeepstoneTile>());
            SetupTileMerge(TileID.Mythril, ModContent.TileType<HardenedDeepstoneTile>());
            SetupTileMerge(TileID.Orichalcum, ModContent.TileType<HardenedDeepstoneTile>());

            TileID.Sets.PreventsSandfall[TileID.Sand] = true;
            TileID.Sets.PreventsSandfall[TileID.Slush] = true;
        }
    }
}
