using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.IO;

namespace UltimateSkyblock.Content.Subworlds.DungeonRoomUtils
{
    public class DynamicStructureSystem
    {
        //This exists because anything related to getting info about a structure file is internal in Structure Helper, which I may add, is incredibly annoying.
        //So yeah I just copied and pasted everything I needed here.

        public static Dictionary<string, TagCompound> DynamicStructureDataCache = new Dictionary<string, TagCompound>();

        public static bool LoadFile(string path, Mod mod, bool fullPath = false)
        {
            TagCompound tagCompound;
            if (!fullPath)
            {
                Stream fileStream = mod.GetFileStream(path);
                tagCompound = TagIO.FromStream(fileStream);
                fileStream.Close();
            }
            else
            {
                tagCompound = TagIO.FromFile(path);
            }

            DynamicStructureDataCache.Add(path, tagCompound);
            return true;
        }

        public static TagCompound GetTag(string path, Mod mod, bool fullPath = false)
        {
            if (!DynamicStructureDataCache.ContainsKey(path))
            {
                if (!LoadFile(path, mod, fullPath))
                {
                    return null;
                }

                return DynamicStructureDataCache[path];
            }

            return DynamicStructureDataCache[path];
        }

        public static bool GetDimensions(string path, Mod mod, ref Point16 dims, bool fullPath = false)
        {
            TagCompound tag = GetTag(path, mod, fullPath);
            dims = new Point16(tag.GetInt("Width") + 1, tag.GetInt("Height") + 1);
            return true;
        }
    }
}
