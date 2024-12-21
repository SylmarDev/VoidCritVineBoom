using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SylmarDev.VoidCritVineBoom
{
    public class VoidCritVineBoomConfig
    {
        public static ConfigEntry<bool> muteOriginalVoidExecute;

        public void Init(string configPath)
        {
            var config = new ConfigFile(Path.Combine(configPath, Main.PluginGUID + ".cfg"), true);

            muteOriginalVoidExecute = config.Bind("Tweaks", "Mute Original Void Crit SFX", true, "Set to true to mute the vanilla Void Crit proc SFX");
        }
    }
}
