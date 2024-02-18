using SubworldLibrary;
using Terraria.DataStructures;
using Terraria;
using Terraria.ModLoader;
using Terraria.IO;
using Terraria.ModLoader.IO;
using UltimateSkyblock.Content.Subworlds;
using Terraria.ID;

namespace UltimateSkyblock.Content.ModSystems
{
    public class SubworldModSystem : ModSystem
	{
        public override void SaveWorldData(TagCompound tag)
        {
            tag["DownedPlantera"] = NPC.downedPlantBoss;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            NPC.downedPlantBoss = tag.ContainsKey("DownedPlantera");
        }

        public override void PreUpdateWorld()
        {
            if (SubworldSystem.IsActive<PlanteraSubworld>() || SubworldSystem.IsActive<MiningSubworld>())
            {
                if (SubworldSystem.IsActive<PlanteraSubworld>())
                SubworldSystem.hideUnderworld = true;

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