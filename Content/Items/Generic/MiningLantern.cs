using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SubworldLibrary;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using UltimateSkyblock.Content.Buffs;
using UltimateSkyblock.Content.Items.Placeable;
using UltimateSkyblock.Content.ModPlayers;
using UltimateSkyblock.Content.Subworlds;

namespace UltimateSkyblock.Content.Items.Generic
{
    public class MiningLantern : ModItem
    {
        public int teleportTimer = 0;
        public bool locked = true;  
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
            Item.value = Item.sellPrice(silver:15);
            Item.holdStyle = ItemHoldStyleID.HoldLamp;
        }

        public override bool? UseItem(Player player)
        {
            locked = false;
            return true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
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
            if (!locked)
            {
                teleportTimer++;
                if (teleportTimer >= 45)
                {
                    locked = true;
                    teleportTimer = 0;
                    if (player == Main.LocalPlayer)
                    {
                        if (!SubworldSystem.AnyActive())
                            SubworldSystem.Enter<MiningSubworld>();
                        else
                            SubworldSystem.Exit();
                    }
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
