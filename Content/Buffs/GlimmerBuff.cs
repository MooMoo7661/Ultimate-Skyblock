using Terraria;
using Terraria.Graphics.Light;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace UltimateSkyblock.Content.Buffs
{
    public class GlimmerBuff : ModBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HasBuff(BuffID.Darkness))
            {
                player.ClearBuff(BuffID.Darkness);
                player.nightVision = true;

                if (Main.rand.NextBool(10))
                {
                    Dust dust = Dust.NewDustDirect(player.position + new Vector2(Main.rand.Next(-5, 5), Main.rand.Next(-5, 5)), 20, 20, DustID.PurpleTorch);
                    dust.noGravity = true;
                    dust.noLight = true;
                }
            }
        }
    }
}
