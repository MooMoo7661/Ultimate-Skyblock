using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using UltimateSkyblock.Content.Items.Placeable;
using UltimateSkyblock.Content.Tiles.Blocks;

namespace UltimateSkyblock.Content.Biomes
{
    public class DeepstoneBiome : ModBiome
    {
        public override ModWaterStyle WaterStyle => ModContent.GetInstance<DeepstoneWaterStyle>();
        public override ModUndergroundBackgroundStyle UndergroundBackgroundStyle => ModContent.GetInstance<DeepstoneUndergroundBackground>();
        public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Normal;

        public override int Music => MusicLoader.GetMusicSlot("UltimateSkyblock/Sounds/Music/DeepstoneBiome");

        public override int BiomeTorchItemType => ItemID.Torch;
        public override int BiomeCampfireItemType => ItemID.Campfire;

        public override string BestiaryIcon => base.BestiaryIcon;
        public override string BackgroundPath => base.BackgroundPath;
        public override Color? BackgroundColor => base.BackgroundColor;
        public override string MapBackground => BackgroundPath;

        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeMedium;

        public override bool IsBiomeActive(Player player)
        {
            return ModContent.GetInstance<DeepstoneCount>().blockCount >= 40 &&
                player.position.ToTileCoordinates().Y > Main.maxTilesY / 2;
        }

        public override void OnInBiome(Player player)
        {
            player.AddBuff(BuffID.Darkness, 301);
        }

    }

    public class DeepstoneCount : ModSystem
    {
        public int blockCount;
        
        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
        {
            blockCount = tileCounts[ModContent.TileType<DeepstoneTile>()] + tileCounts[ModContent.TileType<DeepstoneGravelTile>()];
        }
    }
}
