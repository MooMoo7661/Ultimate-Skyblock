using SubworldLibrary;
using Terraria.ModLoader.IO;
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

        public override void SaveWorldData(TagCompound tag)
        {
            tag["UltimateSkyblock:PlanteraDungeon"] = NPC.downedPlantBoss;
            tag["UltimateSkyblock:DownedGolem"] = NPC.downedGolemBoss;
            tag["UltimateSkyblock:DownedBoss3"] = NPC.downedBoss3;
            tag["UltimateSkyblock:Hardmode"] = Main.hardMode;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            NPC.downedPlantBoss = tag.GetBool("UltimateSkyblock:PlanteraDungeon");
            NPC.downedGolemBoss = tag.GetBool("UltimateSkyblock:DownedGolem");
            NPC.downedBoss3 = tag.GetBool("UltimateSkyblock:DownedBoss3");
            Main.hardMode = tag.GetBool("UltimateSkyblock:Hardmode");
        }

        public override void PreUpdateTime()
        {
            if (SubworldSystem.IsActive<MiningSubworld>() || SubworldSystem.IsActive<DungeonSubworld>())
            {
                Main.pumpkinMoon = false;
                Main.snowMoon = false;
                Main.raining = false;
                Main.dayTime = true;
                Main.eclipse = false;
            }
        }
    }
}