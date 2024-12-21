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
using RoR2.Audio;
using System.Reflection;
using System.Collections;
using R2API.Networking.Interfaces;

namespace SylmarDev.VoidCritVineBoom
{
    [BepInDependency("com.bepis.r2api.sound", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.bepis.r2api.prefab", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class Main : BaseUnityPlugin
    {
        public const string PluginAuthor = "SylmarDev";
        public const string PluginName = "VoidCritVineBoom";
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginVersion = "1.0.0";

        public bool shouldMuteOriginalSFX;

        public void Awake()
        {
            Log.Init(Logger);
            Log.Info($"{PluginGUID} // ver {PluginVersion}");
            Log.Info("Starting Process. . .");

            new VoidCritVineBoomConfig().Init(Paths.ConfigPath);
            shouldMuteOriginalSFX = VoidCritVineBoomConfig.muteOriginalVoidExecute.Value;

            NetworkingAPI.RegisterMessageType<SyncSound>();

            using (Stream manifestResourceStream = base.GetType().Assembly.GetManifestResourceStream("VoidCritVineBoom.Resources.vineboom.bnk"))
            {
                byte[] array = new byte[manifestResourceStream.Length];
                manifestResourceStream.Read(array, 0, array.Length);
                SoundAPI.SoundBanks.Add(array);
            }
            
            On.RoR2.EffectManager.SpawnEffect_GameObject_EffectData_bool += EffectManager_SpawnEffect_GameObject_EffectData_bool;

            if (shouldMuteOriginalSFX)
            {
                On.RoR2.EffectCatalog.GetEffectDef += EffectCatalog_GetEffectDef;
            }

            Log.Info("awake complete");
        }

        private IEnumerator DelayPostSound(float delayTime, Vector3 pos)
        {
            yield return new WaitForSeconds(delayTime);
            GameObject obj = new GameObject
            {
                transform =
                {
                    position = pos
                }
            };

            AkSoundEngine.PostEvent("Play_vineboom", obj);

            NetMessageExtensions.Send(new SyncSound(pos), (NetworkDestination) 1);
            yield break;
        }

        private void EffectManager_SpawnEffect_GameObject_EffectData_bool(On.RoR2.EffectManager.orig_SpawnEffect_GameObject_EffectData_bool orig, GameObject effectPrefab, EffectData effectData, bool transmit)
        {
            if (effectPrefab.name.Trim() == "CritGlassesVoidExecuteEffect")
            {                
                if (shouldMuteOriginalSFX)
                {
                    effectData.networkSoundEventIndex = NetworkSoundEventIndex.Invalid;
                }

                // play vine boom instead
                StartCoroutine(DelayPostSound(0f, effectData.origin));
            }

            orig(effectPrefab, effectData, transmit);
        }

        private EffectDef EffectCatalog_GetEffectDef(On.RoR2.EffectCatalog.orig_GetEffectDef orig, EffectIndex effectIndex)
        {
            if (effectIndex != (EffectIndex) 136) // 136 is the id of void crit execute effect
            {
                return orig(effectIndex);
            }

            var value = orig(effectIndex);

            var prefab = PrefabAPI.InstantiateClone(value.prefab, "mutedvoidexecute", false);
            prefab.GetComponent<EffectComponent>().soundName = "";

            return new EffectDef(prefab);
        }

        public void Update()
        {

        }
    }
}
