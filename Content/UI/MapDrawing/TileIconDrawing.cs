using System;
using System.Collections.Generic;
using System.IO;
using Terraria.Chat;
using Terraria.Map;
using UltimateSkyblock.Content.Tiles.Furniture.MapMarkers;

namespace UltimateSkyblock.Content.UI.MapDrawing
{
    public class TileIconDrawing : ModMapLayer
    {
        Dictionary<int, Asset<Texture2D>> MapMarkers = new Dictionary<int, Asset<Texture2D>>();
        public static List<MapIcon> icons = new List<MapIcon>();

        public static List<Point16> TEs = new List<Point16>();  

        public override void Load()
        {
            MapMarkers.Add(ModContent.TileEntityType<CorruptBiomeMapMarkerEntity>(), ModContent.Request<Texture2D>("UltimateSkyblock/Content/UI/MapDrawing/Icons/IconEvilCorruption"));
            MapMarkers.Add(ModContent.TileEntityType<CrimsonBiomeMapMarkerEntity>(), ModContent.Request<Texture2D>("UltimateSkyblock/Content/UI/MapDrawing/Icons/IconEvilCrimson"));
            MapMarkers.Add(ModContent.TileEntityType<DesertBiomeCoreEntity>(), ModContent.Request<Texture2D>("UltimateSkyblock/Content/UI/MapDrawing/Icons/IconDesert"));
            MapMarkers.Add(ModContent.TileEntityType<ForestMapMarkerEntity>(), ModContent.Request<Texture2D>("UltimateSkyblock/Content/UI/MapDrawing/Icons/IconForest"));
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
    
    public class MapIcon
    {
        public Vector2 Position;
        public Texture2D Texture;
        public Color DrawColor;
        public float SelectedScale;
        public float DeselectedScale;
        public string HoverName;

        public MapIcon(Vector2 position, Texture2D texture, Color drawColor, float selectedScale, float deselectedScale, string hoverName)
        {
            Position = position;
            Texture = texture;
            DrawColor = drawColor;
            SelectedScale = selectedScale;
            DeselectedScale = deselectedScale;
            HoverName = hoverName;
        }
    }
}
