using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using static UltimateSkyblock.Content.SkyblockWorldGen.MainWorld;
using UltimateSkyblock.Content.Configs;
using SubworldLibrary;
using System.Threading;
using Terraria.Audio;
using UltimateSkyblock.Content.UI.Guidebook;
using CombinationsMod.Content.Keybindings;
using UltimateSkyblock.Content.Items.Generic;

namespace UltimateSkyblock.Content.ModPlayers
{
    public class SkyblockPlayer : ModPlayer
    {
        ushort joinTimer = 0;
        bool locked = false;

        public override void PreUpdate()
        {
            //Join message to tell the player not to use small worlds.
            if (!locked)
            {
                if (joinTimer == 360)
                {
                    if (WorldSize == WorldSizes.Small && ModContent.GetInstance<SkyblockModConfig>().SmallWorldWarning && SubworldSystem.Current == null)
                    {
                        Main.NewText("----------" + "\nYou are currently on a small world\nFor the best experience, please create a large world, as the islands will be unnaturally close together\n[c/E136EE:This message can be disabled at any time through the Gameplay Config.]\n" + "----------");
                    }
                    locked = true;
                }
                else
                    joinTimer++;
            }
        }

        public override void OnEnterWorld()
        {
            locked = false;
            joinTimer = 0;

            for (int i = 0; i < Player.inventory.Length; i++)
            {
                Item item = Player.inventory[i];
                if (item.ModItem is Lantern lantern)
                {
                    lantern.locked = true;
                    lantern.teleportTimer = 0;
                    lantern.reuseTimer = 7200;
                }
            }

        }
    


        public override void PostUpdate()
        {
            if (KeybindSystem.OpenBookKeybind.JustPressed)
            {
                if (ModContent.GetInstance<GuidebookSystem>().IsUIOpen())
                {
                    SoundEngine.PlaySound(SoundID.MenuClose);
                    ModContent.GetInstance<GuidebookSystem>().HideMyUI();
                }
                else
                {
                    SoundEngine.PlaySound(SoundID.MenuOpen);
                    ModContent.GetInstance<GuidebookSystem>().ShowMyUI();
                }
            }

            //Kills player when falling out
            if (Player.position.ToTileCoordinates().Y >= Main.maxTilesY - 45 && SubworldSystem.Current == null)
            {
                Player.KillMe(PlayerDeathReason.ByCustomReason(Player.name + " fell out of the world"), 0, 0, false);
            }
        }
    }
}