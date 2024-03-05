using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using static UltimateSkyblock.Content.SkyblockWorldGen.MainWorld;
using UltimateSkyblock.Content.Configs;
using SubworldLibrary;

namespace UltimateSkyblock.Content.ModPlayers
{
    public class SkyblockPlayer : ModPlayer
    {
        public override void PreUpdate()
        {
            if (Player.position.ToTileCoordinates().Y >= Main.maxTilesY - 45 && SubworldSystem.Current == null)
            {
                if (ModContent.GetInstance<SkyblockModConfig>().TeleportToTopOfWorldOnDeath)
                {
                    Player.Teleport(new Vector2(Player.position.X, 200), TeleportationStyleID.ShellphoneSpawn);
                }
                else
                {
                    Player.KillMe(PlayerDeathReason.ByCustomReason(Player.name + " fell out of the world"), 0, 0, false);
                }
            }
        }

        public override void OnEnterWorld()
        {
            if (WorldSize == WorldSizes.Small && ModContent.GetInstance<SkyblockModConfig>().SmallWorldWarning && SubworldSystem.Current == null)
            {
                Main.NewText("[c/E136EE:It has been detected that this is a small world. For the best experience, please create a medium or large world, as the world generation will be extremely bad and limited.]\n[c/E136EE:This message can be disabled at any time through the Gameplay Config.]");
            }
        }
    }
}