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
using UltimateSkyblock.Content.NPCs;
using UltimateSkyblock.Content.Tiles.Walls;
using System;
using System.Net;

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
                    if ((WorldSize is WorldSizes.Small or WorldSizes.Large) && ModContent.GetInstance<SkyblockModConfig>().SmallWorldWarning && SubworldSystem.Current == null && UltimateSkyblock.IsSkyblock())
                    {
                        Main.NewText("----------" + "\nYou are currently on a " + WorldSize.ToString().ToLower() + " world\nFor the best experience, please create a medium world, as the islands will be " + (WorldSize == WorldSizes.Small ? "too close together." : "too far apart.") + "\n[c/E136EE:This message can be disabled at any time through the Gameplay Config.]\n" + "----------");
                    }
                    locked = true;
                }
                else
                    joinTimer++;
            }
        }

        public override void OnConsumeAmmo(Item weapon, Item ammo)
        {
            foreach (var val in UI.MapDrawing.TileIconDrawing.TEs)
                Main.NewText(val);
        }

        public override void PostUpdate()
        {
            WorldGen.PlantAlch();

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
            if (Main.myPlayer == Player.whoAmI && Player.position.ToTileCoordinates().Y >= Main.maxTilesY - 45 && SubworldSystem.Current == null)
            {
                Player.KillMe(PlayerDeathReason.ByCustomReason(Player.name + " fell out of the world"), 0, 0, false);
            }
        }
    }
}