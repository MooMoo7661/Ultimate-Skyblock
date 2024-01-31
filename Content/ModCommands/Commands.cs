using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Chat;
using Terraria.Localization;
using Terraria.ModLoader;

namespace OneBlock.Content.ModCommands
{
    public class WorldSizeCommand : ModCommand
    {
        public override CommandType Type
            => CommandType.World;

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
    public class StructureFinder : ModCommand
    {
        public override CommandType Type
            => CommandType.World;

        public override string Command
            => "locate";

        public override string Usage
            => "/locate [biome or structure name]\nforest, dungeon, hell";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (args.Length == 0)
            {
                throw new UsageException("At least one argument was expected.");
            }

            if (args.Length > 1)
            {
                throw new UsageException("Only one argument was expected.");
            }

            string location = args[0].ToLower() switch
            {
                "forest" => $"Forest Islands: {WorldHelpers.Spawn.X}, {WorldHelpers.Spawn.Y}",
                "dungeon" => $"Dungeon Island: {Main.dungeonX}, {Main.dungeonY}",
                "hell" => $"Main Hell Island: {WorldHelpers.Hell.X}, {WorldHelpers.Hell.Y}",
                _ => $"[c/FF1919:No structure/biome found with the name \"{args[0]}\"]",
            };

            ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"{location}"), Color.White);
            caller.Reply($"{location}");
        }
    }
}