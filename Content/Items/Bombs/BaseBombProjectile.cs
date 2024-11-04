using Terraria.Audio;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using UltimateSkyblock.Content.Utils;

namespace UltimateSkyblock.Content.Items.Bombs
{
    public abstract class BaseBombProjectile : ModProjectile
    {
        protected abstract int BlockID { get; }

        private const int DefaultWidthHeight = 15;
        private const int ExplosionWidthHeight = 250;
        private bool colliding = false;
        public override void SetDefaults()
        {
            // While the sprite is actually bigger than 15x15, we use 15x15 since it lets the projectile clip into tiles as it bounces. It looks better.
            Projectile.width = DefaultWidthHeight;
            Projectile.height = DefaultWidthHeight;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.aiStyle = -1;
            // 5 second fuse.
            Projectile.timeLeft = 300;

            // These help the projectile hitbox be centered on the projectile sprite.
            DrawOffsetX = -2;
            DrawOriginOffsetY = -5;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            // Vanilla explosions do less damage to Eater of Worlds in expert mode, so we will too.
            if (Main.expertMode)
            {
                if (target.type >= NPCID.EaterofWorldsHead && target.type <= NPCID.EaterofWorldsTail)
                {
                    modifiers.FinalDamage /= 5;
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            //Projectile.soundDelay = 10;

            //// This code makes the projectile very bouncy.
            //if (Projectile.velocity.X != oldVelocity.X && Math.Abs(oldVelocity.X) > 1f)
            //{
            //    Projectile.velocity.X = oldVelocity.X * -0.2f;
            //}
            //if (Projectile.velocity.Y != oldVelocity.Y && Math.Abs(oldVelocity.Y) > 1f)
            //{
            //    Projectile.velocity.Y = 0f;
            //}
            return false;
        }

        public override void AI()
        {

            // The projectile is in the midst of exploding during the last 3 updates.
            if (Projectile.owner == Main.myPlayer && Projectile.timeLeft <= 3)
            {
                Projectile.tileCollide = false;
                // Set to transparent. This projectile technically lives as transparent for about 3 frames
                Projectile.alpha = 255;

                // change the hitbox size, centered about the original projectile center. This makes the projectile damage enemies during the explosion.
                Projectile.Resize(ExplosionWidthHeight, ExplosionWidthHeight);

                Projectile.damage = 250;
                Projectile.knockBack = 10f;
            }
            else
            {
                // Smoke and fuse dust spawn. The position is calculated to spawn the dust directly on the fuse.
                if (Main.rand.NextBool())
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 1f);
                    dust.scale = 0.1f + Main.rand.Next(5) * 0.1f;
                    dust.fadeIn = 1.5f + Main.rand.Next(5) * 0.1f;
                    dust.noGravity = true;
                    dust.position = Projectile.Center + new Vector2(1, 0).RotatedBy(Projectile.rotation - 2.1f, default) * 10f;

                    dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 1f);
                    dust.scale = 1f + Main.rand.Next(5) * 0.1f;
                    dust.noGravity = true;
                    dust.position = Projectile.Center + new Vector2(1, 0).RotatedBy(Projectile.rotation - 2.1f, default) * 10f;
                }
            }

            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] > 10f)
            {
                Projectile.ai[0] = 10f;
                // Roll speed dampening. 
                if (Projectile.velocity.Y == 0f && Projectile.velocity.X != 0f)
                {
                    Projectile.velocity.X = Projectile.velocity.X * 0.96f;

                    if (Projectile.velocity.X > -0.01 && Projectile.velocity.X < 0.01)
                    {
                        Projectile.velocity.X = 0f;
                        Projectile.netUpdate = true;
                    }
                }

                Projectile.velocity.Y = Projectile.velocity.Y + 0.2f;

            }

            if (Projectile.velocity.Y > 8f)
            {
                Projectile.velocity.Y = 8f;
            }

            Projectile.rotation += Projectile.velocity.X * 0.1f;
        }

        public override void OnKill(int timeLeft)
        {
            // Play explosion sound
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);

            // reset size to normal width and height.
            Projectile.Resize(DefaultWidthHeight, DefaultWidthHeight);

            // Finally, actually explode the tiles and walls. Run this code only for the owner
            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.Resize(22, 22);

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Point pt = Projectile.Center.ToTileCoordinates();
                    Projectile.Kill_DirtAndFluidProjectiles_RunDelegateMethodPushUpForHalfBricks(pt, 4.2f, SpreadDirt);
                }
            }

            SpreadDirt((int)Projectile.position.X, (int)Projectile.position.Y);
        }

        public bool SpreadDirt(int x, int y)
        {
            if (Vector2.Distance(DelegateMethods.v2_1, new Vector2(x, y)) > DelegateMethods.f_1)
            {
                return false;
            }

            if (WorldGen.PlaceTile(x, y, BlockID, true))
            {
                Vector2 position = new Vector2(x * 16, y * 16);
                int num = 0;
                for (int i = 0; i < 3; i++)
                {
                    Dust dust = Dust.NewDustDirect(position, 16, 16, num, 0f, 0f, 100, Color.Transparent, 2.2f);
                    dust.noGravity = true;
                    dust.velocity.Y -= 1.2f;
                    dust.velocity *= 4f;
                    Dust dust2 = Dust.NewDustDirect(position, 16, 16, num, 0f, 0f, 100, Color.Transparent, 1.3f);
                    dust2.velocity.Y -= 1.2f;
                    dust2.velocity *= 2f;
                }
                int num2 = y + 1;
                if (Main.tile[x, num2] != null && !TileID.Sets.Platforms[Main.tile[x, num2].TileType] && Main.tile[x, num2].TopSlope || Main.tile[x, num2].IsHalfBlock)
                {
                    WorldGen.SlopeTile(x, num2);
                    if (Main.netMode != 0)
                    {
                        NetMessage.SendData(17, -1, -1, null, 14, x, num2);
                    }
                }
                num2 = y - 1;
                if (Main.tile[x, num2] != null && !TileID.Sets.Platforms[Main.tile[x, num2].TileType] && Main.tile[x, num2].BottomSlope)
                {
                    WorldGen.SlopeTile(x, num2);
                    if (Main.netMode != 0)
                    {
                        NetMessage.SendData(17, -1, -1, null, 14, x, num2);
                    }
                }
                for (int j = x - 1; j <= x + 1; j++)
                {
                    for (int k = y - 1; k <= y + 1; k++)
                    {
                        Tile tile = Main.tile[j, k];
                        if (!tile.HasTile || num == tile.TileType || tile.TileType != 2 && tile.TileType != 23 && tile.TileType != 60 && tile.TileType != 70 && tile.TileType != 109 && tile.TileType != 199 && tile.TileType != 477 && tile.TileType != 492)
                        {
                            continue;
                        }
                        bool flag = true;
                        for (int l = j - 1; l <= j + 1; l++)
                        {
                            for (int m = k - 1; m <= k + 1; m++)
                            {
                                if (!WorldGen.SolidTile(l, m))
                                {
                                    flag = false;
                                }
                            }
                        }
                        if (flag)
                        {
                            WorldGen.KillTile(j, k, fail: true);
                            if (Main.netMode != 0)
                            {
                                NetMessage.SendData(17, -1, -1, null, 0, j, k, 1f);
                            }
                        }
                    }
                }
                return true;
            }
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendTileSquare(-1, x, y);
            }
            return false;
        }
    }
}