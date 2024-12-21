using System;
using BepInEx;
using R2API.Utils;
using IL.RoR2.WwiseUtils;
using On.RoR2.WwiseUtils;
using RoR2.WwiseUtils;
using UnityEngine;
using RoR2;
using R2API;
using R2API.Networking;
using System.IO;
using EntityStates.AffixEarthHealer;

namespace SylmarDev.VoidCritVineBoom
{
    [BepInDependency("com.bepis.r2api.sound", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class Main : BaseUnityPlugin
    {
        public const string PluginAuthor = "SylmarDev";
        public const string PluginName = "VoidCritVineBoom";
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginVersion = "1.0.0";

        public void Awake()
        {
            Log.Init(Logger);
            Log.Info($"{PluginGUID} // ver {PluginVersion}");
            Log.Info("Starting Process. . .");

            NetworkingAPI.RegisterMessageType<SyncSound>();
            Log.Debug("Registered Syncsound");

            // TODO: Return to this later

            using (Stream manifestResourceStream = base.GetType().Assembly.GetManifestResourceStream("vineboom.bnk"))
            {
                byte[] array = new byte[manifestResourceStream.Length];
                manifestResourceStream.Read(array, 0, array.Length);
                SoundAPI.SoundBanks.Add(array);
            }

            On.RoR2.HealthComponent.TakeDamageProcess += HealthComponent_TakeDamageProcess;


            Log.Info("awake complete");
        }

        private void HealthComponent_TakeDamageProcess(On.RoR2.HealthComponent.orig_TakeDamageProcess orig, HealthComponent self, DamageInfo damageInfo)
        {
            
        }

        public void Update()
        {

        }
    }
}
