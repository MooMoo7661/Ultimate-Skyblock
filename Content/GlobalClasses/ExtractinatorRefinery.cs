using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OneBlock.Content.GlobalClasses
{
    public class ExtractinatorRefinery : GlobalItem
    {
        public override void ExtractinatorUse(int extractType, int extractinatorBlockType, ref int resultType, ref int resultStack)
        {
            switch(extractType)
            {
                case ItemID.Wood:
                    resultType = WorldGen.crimson ? ItemID.Shadewood : ItemID.Ebonwood;
                    resultStack = 1;
                    break;

                case ItemID.Shadewood or ItemID.Ebonwood:
                    resultType = ItemID.RichMahogany;
                    resultStack = 1; 
                    break;

                case ItemID.RichMahogany:
                    resultType = ItemID.BorealWood;
                    resultStack = 1;
                    break;

                case ItemID.BorealWood:
                    resultType = ItemID.PalmWood;
                    resultStack = 1;
                    break;

                case ItemID.PalmWood:
                    resultType = ItemID.AshWood;
                    resultStack = 1;
                    break;

                case ItemID.AshWood:
                    resultType = ItemID.Wood;
                    resultStack = 1;
                    break;

                case ItemID.Sandstone:
                    resultType = ItemID.SandBlock;
                    resultStack = 1;
                    break;

                case ItemID.IceBlock:
                    resultType = ItemID.SlushBlock;
                    resultStack = 1;
                    break;

                case ItemID.SlimeBlock:
                    resultType = ItemID.Gel;
                    resultStack = 1;
                    break;

                case ItemID.CrimstoneBlock:
                    resultType = ItemID.EbonstoneBlock;
                    resultStack = 1;
                    break;  

                case ItemID.EbonstoneBlock:
                    resultType = ItemID.CrimstoneBlock;
                    resultStack = 1;
                    break;

                case ItemID.Glass:
                    resultType = ItemID.None;
                    resultStack = 1;
                    SoundEngine.PlaySound(SoundID.Shatter);
                    break;
            }
        }
    }

    public class ExtractinatorRefineryAssigner : ModSystem
    {
        public override void PostSetupContent()
        {
            RegisterExtraction(ItemID.Wood);
            RegisterExtraction(ItemID.Ebonwood);
            RegisterExtraction(ItemID.Shadewood);
            RegisterExtraction(ItemID.RichMahogany);
            RegisterExtraction(ItemID.BorealWood);
            RegisterExtraction(ItemID.PalmWood);
            RegisterExtraction(ItemID.AshWood);

            RegisterExtraction(ItemID.Sandstone);
            RegisterExtraction(ItemID.IceBlock);
            RegisterExtraction(ItemID.CrimstoneBlock);
            RegisterExtraction(ItemID.EbonstoneBlock);

            RegisterExtraction(ItemID.Glass);
            RegisterExtraction(ItemID.SlimeBlock);
        }

        static void RegisterExtraction(int item) => ItemID.Sets.ExtractinatorMode[item] = item;
    }
}
