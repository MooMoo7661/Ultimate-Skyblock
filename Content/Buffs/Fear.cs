using Terraria;
using Terraria.Graphics.Light;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace UltimateSkyblock.Content.Buffs
{
    public class Fear : ModBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            player.controlUp = false;
            player.controlDown = false;
            player.controlLeft = false;
            player.controlRight = false;
            player.controlJump = false;
            player.controlUseItem = false;
        }
    }
}
