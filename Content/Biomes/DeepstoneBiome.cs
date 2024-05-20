using System;
using SubworldLibrary;
using Terraria.Graphics.Capture;
using UltimateSkyblock.Content.Tiles.Blocks;

namespace UltimateSkyblock.Content.Biomes
{
    public class DeepstoneBiome : ModBiome
    {
        public override ModWaterStyle WaterStyle => ModContent.GetInstance<DeepstoneWaterStyle>();
        public override ModUndergroundBackgroundStyle UndergroundBackgroundStyle => ModContent.GetInstance<DeepstoneUndergroundBackground>();
        public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Normal;

        public override int Music => MusicLoader.GetMusicSlot("UltimateSkyblock/Content/Sounds/Music/DeepstoneBiome");

        public override int BiomeTorchItemType => ItemID.Torch;
        public override int BiomeCampfireItemType => ItemID.Campfire;

        public override string BestiaryIcon => base.BestiaryIcon;
        public override string BackgroundPath => base.BackgroundPath;
        public override Color? BackgroundColor => base.BackgroundColor;
        public override string MapBackground => BackgroundPath;

        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;

        public override bool IsBiomeActive(Player player)
        {
            return ModContent.GetInstance<DeepstoneCount>().blockCount >= 120 && !player.ZoneUnderworldHeight;
        }

        public override void OnInBiome(Player player)
        {
            if (player.ZoneNormalCaverns) player.AddBuff(BuffID.Darkness, 301);
        }

        public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => base.SurfaceBackgroundStyle;
    }

    public class DeepstoneCount : ModSystem
    {
        public int blockCount;
        
        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
        {
            blockCount = tileCounts[ModContent.TileType<DeepstoneTile>()];
        }
    }
}
