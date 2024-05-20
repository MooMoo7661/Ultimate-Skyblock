using Microsoft.Xna.Framework;
using SubworldLibrary;
using Terraria;
using Terraria.Chat;
using Terraria.Localization;
using Terraria.ModLoader;
using UltimateSkyblock.Content.SkyblockWorldGen;

namespace UltimateSkyblock.Content.ModCommands
{
    public class WorldSizeCommand : ModCommand
    {
        public override CommandType Type
            => CommandType.Chat;

        public override string Command
            => "worldsize";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"[c/32FF82:World size: {SkyblockWorldGen.MainWorld.WorldSize}]"), Color.White);
        }
    }

    public class LocationCommand : ModCommand
    {
        public override CommandType Type
            => CommandType.Chat;

        public override string Command
            => "coords";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"[c/32FF82:Coordinates: {Main.LocalPlayer.Center.ToTileCoordinates()}]"), Color.White);
        }
    }
}