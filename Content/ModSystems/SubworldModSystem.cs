using SubworldLibrary;
using UltimateSkyblock.Content.Subworlds;

namespace UltimateSkyblock.Content.ModSystems
{
    public class SubworldModSystem : ModSystem
	{
        public override void PreUpdateWorld()
        {
            if (SubworldSystem.IsActive<MiningSubworld>())
            {
                // Update mechanisms
                Wiring.UpdateMech();

                // Update tile entities
                TileEntity.UpdateStart();
                foreach (TileEntity te in TileEntity.ByID.Values)
                {
                    te.Update();
                }
                TileEntity.UpdateEnd();

                // Update liquid
                if (++Liquid.skipCount > 1)
                {
                    Liquid.UpdateLiquid();
                    Liquid.skipCount = 0;
                }
            }
        }
    }
}