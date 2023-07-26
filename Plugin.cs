using BepInEx;
using CrashUtils.WeaponManager.WeaponSetup;
using HarmonyLib;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace CoolCamUtils
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class CamGunLoader : BaseUnityPlugin
    {
        public static CamGunLoader CG;
        public static CameraGun CamGn;

        public static AssetBundle Assets;
        public static Harmony Harmony = new Harmony(PluginInfo.PLUGIN_GUID);


        private void Awake()
        {
            CG = this;
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            Harmony.PatchAll(typeof(CamGunLoader));
            Assets = AssetBundle.LoadFromFile(Path.Combine(ModPath(), "camgun"));


            CamGun.LoadAssets();
            GunAdditives.Register(new CamGun());


        }
        private void Update()
        {
            if (CamGn != null)
            {
                if (CamGn.LookAt)
                {
                    CamGn.Cam.transform.LookAt(CamGn.PlayerCam.transform);
                    CamGn.Cam.fieldOfView = 100 - Vector3.Distance(CamGn.PlayerCam.transform.position, CamGn.Cam.transform.position);
                    if (CamGn.Cam.fieldOfView <= 5)
                    {
                        CamGn.Cam.fieldOfView = 5;
                    }
                }
            }

        }

        public static string ModPath()
        {
            return Assembly.GetExecutingAssembly().Location.Substring(0, Assembly.GetExecutingAssembly().Location.LastIndexOf(Path.DirectorySeparatorChar));
        }
        

        
    }

    
}
