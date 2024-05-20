namespace UltimateSkyblock.Content.SkyblockWorldGen;
public static class WorldHelpers
{
    // Quick ref paths
    public static readonly string path = "Content/SkyblockWorldGen/Structures/";

    public static string evilPath
    {
        get
        {
            return "Content/SkyblockWorldGen/Structures/" + (WorldGen.crimson? "CrimsonIsland" : "CorruptIsland");
        }
    }

    public static readonly string hellPath = path + "HellIsland";
    public static readonly string forestPath = path + "ForestIsland";
    public static readonly string junglePath = path + "JungleIsland";
    public static readonly string templePath = path + "Temple";
    public static readonly string hivePath = path + "Hive";
    public static readonly string snowPath = path + "SnowIsland";
    public static readonly string desertPath = path + "DesertIsland";
    public static readonly string hallowPath = path + "HallowIsland";
}