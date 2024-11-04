// Ignore Spelling: Keybind Keybindings

using Terraria.ModLoader;

namespace UltimateSkyblock.Content.Keybinds
{
    public class KeybindSystem : ModSystem
    {
        public static ModKeybind PageLeftKeybind { get; private set; }
        public static ModKeybind PageRightKeybind { get; private set; }
        public static ModKeybind CloseBookKeybind { get; private set; }
        public static ModKeybind WikiPageKeybind { get; private set; }
        public static ModKeybind OpenBookKeybind { get; private set; }

        public override void Load()
        {
            Mod.Logger.Info("Registered keybinds for book UI");
            PageLeftKeybind = KeybindLoader.RegisterKeybind(Mod, "PageLeft", "Left");
            PageRightKeybind = KeybindLoader.RegisterKeybind(Mod, "PageRight", "Right");
            CloseBookKeybind = KeybindLoader.RegisterKeybind(Mod, "CloseBook", "Escape");
            WikiPageKeybind = KeybindLoader.RegisterKeybind(Mod, "OpenWikiPage", "L");
            OpenBookKeybind = KeybindLoader.RegisterKeybind(Mod, "OpenGuidebook", "K");
        }

        public override void Unload()
        {
            PageLeftKeybind = null;
            PageRightKeybind = null;
            CloseBookKeybind = null;
            WikiPageKeybind = null;
            OpenBookKeybind = null;
        }
    }
}