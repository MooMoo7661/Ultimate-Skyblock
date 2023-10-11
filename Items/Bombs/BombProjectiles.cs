using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.PlayerDrawLayer;
using static Humanizer.In;

namespace OneBlock.Items.Bombs
{
    public class FossilBombProjectile : BaseBombProjectile
    {
        public override string Texture => "OneBlock/Items/Bombs/FossilBomb";
        protected override int BlockID => TileID.DesertFossil;
    }
    public class ClayBombProjectile : BaseBombProjectile
    {
        public override string Texture => "OneBlock/Items/Bombs/ClayBomb";
        protected override int BlockID => TileID.ClayBlock;
    }
    public class SandBombProjectile : BaseBombProjectile
    {
        public override string Texture => "OneBlock/Items/Bombs/SandBomb";
        protected override int BlockID => TileID.Sand;
    }
    public class StoneBombProjectile : BaseBombProjectile
    {
        public override string Texture => "OneBlock/Items/Bombs/StoneBomb";
        protected override int BlockID => TileID.Stone;
    }
    public class SnowBombProjectile : BaseBombProjectile
    {
        public override string Texture => "OneBlock/Items/Bombs/SnowBomb";
        protected override int BlockID => TileID.SnowBlock;
    }


    public class StickyStoneBombProjectile : BaseStickyBombProjectile
    {
        public override string Texture => "OneBlock/Items/Bombs/StickyStoneBomb";
        protected override int BlockID => TileID.Stone;
    }
    public class StickySandBombProjectile : BaseStickyBombProjectile
    {
        public override string Texture => "OneBlock/Items/Bombs/StickySandBomb";
        protected override int BlockID => TileID.Sand;
    }
    public class StickyClayBombProjectile : BaseStickyBombProjectile
    {
        public override string Texture => "OneBlock/Items/Bombs/StickyClayBomb";
        protected override int BlockID => TileID.ClayBlock;
    }
    public class StickyFossilBombProjectile : BaseStickyBombProjectile
    {
        public override string Texture => "OneBlock/Items/Bombs/StickyFossilBomb";
        protected override int BlockID => TileID.DesertFossil;
    }
    public class StickySnowBombProjectile : BaseStickyBombProjectile
    {
        public override string Texture => "OneBlock/Items/Bombs/StickySnowBomb";
        protected override int BlockID => TileID.SnowBlock;
    }
}