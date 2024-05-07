using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateSkyblock.Content.Subworlds.DungeonRoomUtils
{
    public class GenLogger
    {
        internal readonly static string modSource = "[UltimateSkyblock]: ";

        public static void QuickLog(string message)
        {
            UltimateSkyblock.Instance.Logger.Info(message);
        }

        //This exists so I can print logs that stand out, since it's hard for me to read the logs for a specific thing.
        //I log the message twice, because Mod.Logger handles actually writing to the client.log file.
        //Console.WriteLine does not, and only writes to the console, but that's alright since I just need that to help me find debug stuff.
        public static void ColorLog(string message, ConsoleColor color = ConsoleColor.Gray)
        {
            var time = DateTime.Now;
            string second = ((time.Second < 10) ? "0" : "") + time.Second.ToString();
            Console.ForegroundColor = color;
            Console.WriteLine("[" + time.Hour + ":" + time.Minute + ":" + second + ":" + time.Millisecond + "] " + "[.Net ThreadPool Worker/INFO] " + modSource + message);
            Console.ResetColor();
            UltimateSkyblock.Instance.Logger.Info(message);
        }
    }
}
