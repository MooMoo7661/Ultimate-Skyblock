using Microsoft.Xna.Framework;
using OneBlock.SkyblockWorldGen;
using StructureHelper;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using tModPorter;
using static OneBlock.SkyblockWorldGen.MainWorld;
using static Terraria.WorldGen;

internal static class WorldHelpers
{
    public static Point16 Hell; // Center of Main Hell Island
    public static Point16 Hallow; // Top left of Hallow Islands
    public static Point16 Spawn => new(Main.maxTilesX / 2, Main.maxTilesY / 3); // Spawn point on the Spawn Island
    public static Point16 Jungle => new(Main.maxTilesX / 2 + Main.maxTilesX / 7 + (int)(ScaleBasedOnWorldSizeX * 2), Main.maxTilesY / 3); // Center of the main jungle island
    public static Point16 Evil => new(Main.maxTilesX / 2 - Main.maxTilesX / 7 + (int)(ScaleBasedOnWorldSizeX * 1.3f), 100); // Center to spawn evil islands at
    public static Point16 Snow => new(Main.maxTilesX / 2 + Main.maxTilesX / 4 + (int)(ScaleBasedOnWorldSizeX * 1.3f), Main.maxTilesY / 3);

    // All of these are for quick and easy worldgen code that is less cluttered (hopefully).
    public static readonly string path = "SkyblockWorldGen/Structures/";
    public static readonly string hellPath = path + "HellIsland";
    public static readonly string forestPath = path + "ForestIsland";
    public static readonly string junglePath = path + "JungleIsland";
    public static readonly string templePath = path + "Temple";
    public static readonly string corruptPath = path + "Corrupt";
    public static readonly string crimsonPath = path + "Crimson";
    public static readonly string hivePath = path + "Hive";
    public static readonly string snowPath = path + "SnowIsland";
}