using StructureHelper;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using UltimateSkyblock.Content.Utils;
using static UltimateSkyblock.Content.SkyblockWorldGen.MainWorld;

public static class WorldHelpers
{
    public static Point16 Hell => new Point16(Main.maxTilesX / 2, Main.UnderworldLayer + 50);
    public static Point16 Hallow;
    public static Point16 Mushroom => dungeonLeft ? new Point16(Main.maxTilesX - Main.maxTilesX / 20, Main.maxTilesY / 2 - Main.maxTilesY / 5) : new Point16(Main.maxTilesX / 20, Main.maxTilesY / 2 - Main.maxTilesY / 5);
    public static Point16 Spawn => new(Main.maxTilesX / 2, Main.maxTilesY / 2 - Main.maxTilesY / 5); // Spawn point on the Spawn Island
    public static Point16 Jungle => new(Main.maxTilesX / 2 + Main.maxTilesX / 7 + (int)(ScaleBasedOnWorldSizeX * 2), Main.maxTilesY / 2 - Main.maxTilesY / 5); // Center of the main jungle island
    public static Point16 Evil => new(Main.maxTilesX / 2 - Main.maxTilesX / 7 + (int)(ScaleBasedOnWorldSizeX * 1.3f), 100); // Center to spawn evil islands at
    public static Point16 Desert => new(Main.maxTilesX / 4, Main.maxTilesY / 2 - Main.maxTilesY / 5);

    /// <summary>Bottom left of the Snow islands.</summary>
    public static Point16 Snow => new(Main.maxTilesX / 2 + Main.maxTilesX / 4 + (int) (ScaleBasedOnWorldSizeX* 1.3f), Main.maxTilesY / 2 - Main.maxTilesY / 4);

    // Quick ref paths
    public static readonly string path = "Content/SkyblockWorldGen/Structures/";
    public static readonly string hellPath = path + "HellIsland";
    public static readonly string forestPath = path + "ForestIsland";
    public static readonly string junglePath = path + "JungleIsland";
    public static readonly string templePath = path + "Temple";
    public static readonly string corruptPath = path + "Corrupt";
    public static readonly string crimsonPath = path + "Crimson";
    public static readonly string hivePath = path + "Hive";
    public static readonly string snowPath = path + "SnowIsland";
    public static readonly string desertPath = path + "DesertIsland";
}