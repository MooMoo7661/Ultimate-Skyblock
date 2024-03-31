// Ignore Spelling: Keybind Keybindings

using Terraria.ModLoader;

namespace CombinationsMod.Content.Keybindings
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
            PageLeftKeybind = KeybindLoader.RegisterKeybind(Mod, "Page Left", "Left");
            PageRightKeybind = KeybindLoader.RegisterKeybind(Mod, "Page Right", "Right");
            CloseBookKeybind = KeybindLoader.RegisterKeybind(Mod, "Close Book", "Escape");
            WikiPageKeybind = KeybindLoader.RegisterKeybind(Mod, "Open Wiki Page", "L");
            OpenBookKeybind = KeybindLoader.RegisterKeybind(Mod, "Open Guidebook", "K");
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