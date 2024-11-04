using System;
using System.Collections.Generic;
using System.IO;
using Terraria.Chat;
using Terraria.Map;
using UltimateSkyblock.Content.GlobalClasses;
using UltimateSkyblock.Content.Tiles.Furniture;
using UltimateSkyblock.Content.Tiles.Furniture.MapMarkers;

namespace UltimateSkyblock.Content.UI.MapDrawing
{
    public class TileIconDrawing : ModMapLayer
    {
        Dictionary<int, Asset<Texture2D>> MapMarkers = new Dictionary<int, Asset<Texture2D>>();

        public override void Load()
        {
            MapMarkers.Add(ModContent.TileEntityType<DesertBiomeCoreEntity>(), ModContent.Request<Texture2D>("UltimateSkyblock/Content/UI/MapDrawing/Icons/IconDesert"));
            MapMarkers.Add(ModContent.TileEntityType<CorruptBiomeMapMarkerEntity>(), ModContent.Request<Texture2D>("UltimateSkyblock/Content/UI/MapDrawing/Icons/IconEvilCorruption"));
            MapMarkers.Add(ModContent.TileEntityType<CrimsonBiomeMapMarkerEntity>(), ModContent.Request<Texture2D>("UltimateSkyblock/Content/UI/MapDrawing/Icons/IconEvilCrimson"));
            MapMarkers.Add(ModContent.TileEntityType<ForestMapMarkerEntity>(), ModContent.Request<Texture2D>("UltimateSkyblock/Content/UI/MapDrawing/Icons/IconForest"));
            MapMarkers.Add(ModContent.TileEntityType<HallowBiomeMapMarkerEntity>(), ModContent.Request<Texture2D>("UltimateSkyblock/Content/UI/MapDrawing/Icons/IconHallow"));
            MapMarkers.Add(ModContent.TileEntityType<JungleBiomeMapMarkerEntity>(), ModContent.Request<Texture2D>("UltimateSkyblock/Content/UI/MapDrawing/Icons/IconJungle"));
            MapMarkers.Add(ModContent.TileEntityType<MushroomBiomeMapMarkerEntity>(), ModContent.Request<Texture2D>("UltimateSkyblock/Content/UI/MapDrawing/Icons/IconMushroom"));
            MapMarkers.Add(ModContent.TileEntityType<SnowBiomeMapMarkerEntity>(), ModContent.Request<Texture2D>("UltimateSkyblock/Content/UI/MapDrawing/Icons/IconSnow"));
            MapMarkers.Add(ModContent.TileEntityType<GolemAltarMapIconEntity>(), ModContent.Request<Texture2D>("UltimateSkyblock/Content/UI/MapDrawing/Icons/IconGolemAltar"));
            MapMarkers.Add(ModContent.TileEntityType<PlanteraAltarMapIconEntity>(), ModContent.Request<Texture2D>("UltimateSkyblock/Content/UI/MapDrawing/Icons/IconPlantera"));
        }

        public override void Draw(ref MapOverlayDrawContext context, ref string text)
        {
            foreach(var TE in TileEntity.ByPosition.Values)
            {
                if (MapMarkers.ContainsKey(TE.type) && TE.IsTileValidForEntity(TE.Position.X, TE.Position.Y))
                {
                    var res = context.Draw(MapMarkers[TE.type].Value, new(TE.Position.ToVector2().X + 1, TE.Position.ToVector2().Y), Color.White, new SpriteFrame(1, 1, 0, 0), 1f, 1.3f, Alignment.Center);
                    if (res.IsMouseOver) { text = Lang.GetMapObjectName(MapHelper.TileToLookup(Main.tile[(int)TE.Position.ToVector2().X, (int)TE.Position.ToVector2().Y].TileType, 0)); }
                }
            }
        }
    }
}
