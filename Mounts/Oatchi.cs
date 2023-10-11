using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace OneBlock.Mounts
{
    public class Oatchi : ModMount
    {// Since only a single instance of ModMountData ever exists, we can use player.mount._mountSpecificData to store additional data related to a specific mount.
     // Using something like this for gameplay effects would require ModPlayer syncing, but this example is purely visual.

        public override void SetStaticDefaults()
        {
            // Movement
            MountData.jumpHeight = 9; // How high the mount can jump.
            MountData.acceleration = 0.1f; // The rate at which the mount speeds up.
            MountData.jumpSpeed = 9f; // The rate at which the player and mount ascend towards (negative y velocity) the jump height when the jump button is presssed.
            MountData.blockExtraJumps = false; // Determines whether or not you can use a double jump (like cloud in a bottle) while in the mount.
            MountData.constantJump = false; // Allows you to hold the jump button down.
            MountData.heightBoost = 20; // Height between the mount and the ground
            MountData.fallDamage = 0.5f; // Fall damage multiplier.
            MountData.runSpeed = 12f; // The speed of the mount
            MountData.dashSpeed = 12f; // The speed the mount moves when in the state of dashing.
            MountData.flightTimeMax = 0; // The amount of time in frames a mount can be in the state of flying.

            // Misc
            MountData.fatigueMax = 0;
            MountData.buff = ModContent.BuffType<OatchiBuff>(); // The ID number of the buff assigned to the mount.

            // Effects
            MountData.spawnDust = DustID.Cloud; // The ID of the dust spawned when mounted or dismounted.

            // Frame data and player offsets
            MountData.totalFrames = 8; // Amount of animation frames for the mount
            MountData.playerYOffsets = Enumerable.Repeat(27, MountData.totalFrames).ToArray(); // Fills an array with values for less repeating code
            MountData.xOffset = 13;
            MountData.yOffset = 10;
            MountData.playerHeadOffset = 22;
            MountData.bodyFrame = 3;
            // Standing
            MountData.standingFrameCount = 4;
            MountData.standingFrameDelay = 12;
            MountData.standingFrameStart = 0;
            // Running
            MountData.runningFrameCount = 4;
            MountData.runningFrameDelay = 36;
            MountData.runningFrameStart = 0;
            // Flying
            MountData.flyingFrameCount = 0;
            MountData.flyingFrameDelay = 0;
            MountData.flyingFrameStart = 0;
            // In-air
            MountData.inAirFrameCount = 1;
            MountData.inAirFrameDelay = 12;
            MountData.inAirFrameStart = 2;
            // Idle
            MountData.idleFrameCount = 4;
            MountData.idleFrameDelay = 24;
            MountData.idleFrameStart = 0;
            MountData.idleFrameLoop = true;
            // Swim
            MountData.swimFrameCount = MountData.inAirFrameCount;
            MountData.swimFrameDelay = MountData.inAirFrameDelay;
            MountData.swimFrameStart = MountData.inAirFrameStart;

            if (!Main.dedServ)
            {
                MountData.textureWidth = MountData.backTexture.Width() + 20;
                MountData.textureHeight = MountData.backTexture.Height();
            }
        }

        public override void UpdateEffects(Player player)
        {
            // This code spawns some dust if we are moving fast enough.
            if (Math.Abs(player.velocity.X) > 4f)
            {
                Rectangle rect = player.getRect();

                Dust.NewDust(new Vector2(rect.X, rect.Y), rect.Width, rect.Height, DustID.Cloud);
            }
        }

        public override void SetMount(Player player, ref bool skipDust)
        {
            // When this mount is mounted, we initialize _mountSpecificData with a new CarSpecificData object which will track some extra visuals for the mount.

            // This code bypasses the normal mount spawning dust and replaces it with our own visual.
            if (!Main.dedServ)
            {
                for (int i = 0; i < 16; i++)
                {
                    Dust.NewDustPerfect(player.Center + new Vector2(80, 0).RotatedBy(i * Math.PI * 2 / 16f), MountData.spawnDust);
                }

                skipDust = true;
            }
        }

        public override bool Draw(List<DrawData> playerDrawData, int drawType, Player drawPlayer, ref Texture2D texture, ref Texture2D glowTexture, ref Vector2 drawPosition, ref Rectangle frame, ref Color drawColor, ref Color glowColor, ref float rotation, ref SpriteEffects spriteEffects, ref Vector2 drawOrigin, ref float drawScale, float shadow)
        {
            return true;
        }
    }
}
