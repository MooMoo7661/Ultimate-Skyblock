using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace OneBlock.Content.GlobalClasses
{
    public class StoneGeneratorDust : GlobalTile
    {
        public override void PostDraw(int i, int j, int type, SpriteBatch spriteBatch)
        {
            if (type != TileID.Stone && type != TileID.AccentSlab && type != TileID.Obsidian) { return; }

            Tile tileLeft = Main.tile[i - 1, j];
            Tile tileRight = Main.tile[i + 1, j];

            if (tileLeft.LiquidAmount == 0 || tileRight.LiquidAmount == 0) { return; }

            if (tileLeft.LiquidType == LiquidID.Lava && tileRight.LiquidType == LiquidID.Water || tileLeft.LiquidType == LiquidID.Water && tileRight.LiquidType == LiquidID.Lava)
            {
                Dust dust = Dust.NewDustDirect(new Vector2(i, j) * 16, 20, 20, DustID.Torch);
                dust.velocity *= 0.3f;
                dust.scale = 0.5f;
            }
        }
    }
}
