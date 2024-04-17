using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SubworldLibrary;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using UltimateSkyblock.Content.Buffs;
using UltimateSkyblock.Content.Items.Placeable;
using UltimateSkyblock.Content.Subworlds;

namespace UltimateSkyblock.Content.Items.Generic
{
    public abstract class Lantern : ModItem
    {
        public int reuseTimer = 7200;
        public int teleportTimer = 0;
        public bool locked = true;
    }

    public class MiningLantern : Lantern
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 42;
            Item.useStyle = ItemUseStyleID.RaiseLamp;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useTurn = true;
            Item.maxStack = 1;
            Item.consumable = false;
            Item.rare = ItemRarityID.Orange;
            Item.value = Terraria.Item.buyPrice(gold: 5);
            Item.holdStyle = ItemHoldStyleID.HoldLamp;
        }

        public override bool? UseItem(Player player)
        {
            if (reuseTimer > 0)
                return false;

            locked = false;
            return true;
        }

        //public override bool AltFunctionUse(Player player)
        //{
        //    locked = false;
        //    reuseTimer = 0;
        //    return true;
        //}

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (reuseTimer > 0)
            tooltips.Add(new TooltipLine(UltimateSkyblock.Instance, "CooldownInfo", Language.GetText("Mods.UltimateSkyblock.GenericLocalizedText.LanternCooldown").WithFormatArgs(reuseTimer / 60).Value));
            else
                tooltips.Add(new TooltipLine(UltimateSkyblock.Instance, "CooldownInfo", Language.GetText("Mods.UltimateSkyblock.GenericLocalizedText.LanternCooldownFinished").WithFormatArgs(reuseTimer / 60).Value));

            if (SubworldSystem.IsActive<MiningSubworld>())
                tooltips.Add(new TooltipLine(UltimateSkyblock.Instance, "ReturnTooltip", Language.GetTextValue("Mods.UltimateSkyblock.GenericLocalizedText.ReturnTooltip")));
        }

        public override void HoldStyle(Player player, Rectangle heldItemFrame)
        {
            Vector2 pos = new Vector2(9, 22);
            pos.X *= player.direction;
            player.itemLocation = player.Center + pos;
            Item.scale = 0.7f;
            Lighting.AddLight(player.itemLocation - new Vector2(0, 3), 1, 1, 1);
        }

        public override void UpdateInventory(Player player)
        {
            if (reuseTimer > 0)
            reuseTimer--;

            if (!locked)
            {
                teleportTimer++;
                if (teleportTimer >= 45)
                {
                    reuseTimer = 7200;
                    locked = true;
                    teleportTimer = 0;
                    if (!SubworldSystem.AnyActive())
                        SubworldSystem.Enter<MiningSubworld>();
                    else
                        SubworldSystem.Exit();
                }
                else if (teleportTimer == 5)
                {
                    // Creating teleport dust.
                    // Teleports to the player's position, because all I want to do is make the dust without doing the work
                    SoundEngine.PlaySound(SoundID.Item6, player.position);
                    player.Teleport(player.position, TeleportationStyleID.RodOfDiscord);
                }
            }
        }
    }
}
